using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotGameManager : MonoBehaviour
{
    [Header("リール関連")]
    [SerializeField] private SlotReel[] slotReels;   // リール群
    [SerializeField] private float autoSpinDelay = 0.5f;

    [Header("ボタン関連")]
    [SerializeField] private Button spinOnceButton;  // 1回スピンボタン
    [SerializeField] private Button autoSpinButton;  // オートスピンボタン

    [Header("メダル管理")]
    [SerializeField] private MedalManager medalManager; // MedalManager を参照

    private bool isAutoSpinning = false; // オートスピン中かどうか
    private Coroutine autoSpinCoroutine; // オートスピン用のコルーチン

    private void Start()
    {
        spinOnceButton.onClick.AddListener(SpinOnce);
        autoSpinButton.onClick.AddListener(ToggleAutoSpin);
    }

    // --- 1回だけスピン ---
    private void SpinOnce()
    {
        SpinAll();
    }

    // --- オートスピン開始/停止切り替え ---
    private void ToggleAutoSpin()
    {
        if (isAutoSpinning)
        {
            // 停止処理
            isAutoSpinning = false;
            if (autoSpinCoroutine != null)
                StopCoroutine(autoSpinCoroutine);
        }
        else
        {
            // 開始処理
            isAutoSpinning = true;
            autoSpinCoroutine = StartCoroutine(AutoSpinRoutine());
        }
    }

    // --- オートスピン処理 ---
    private IEnumerator AutoSpinRoutine()
    {
        while (isAutoSpinning)
        {
            // リール全部止まってるなら回す
            if (AllReelsStopped())
            {
                // BET分のメダルを消費
                if (!medalManager.SpendMedals())
                {
                    Debug.Log("メダル不足でオートスピン停止");
                    isAutoSpinning = false;
                    yield break; // ループ終了
                }

                // スピン開始
                foreach (var reel in slotReels)
                {
                    reel.StartSpinning();
                }
            }

            // 次のチェックまで待機
            yield return new WaitForSeconds(autoSpinDelay);
        }
    }

    // --- 全リールを回す処理（手動用） ---
    private void SpinAll()
    {
        if (!medalManager.SpendMedals())
        {
            Debug.Log("メダル不足でスピンできません！");
            return;
        }

        foreach (var reel in slotReels)
        {
            reel.StartSpinning();
        }
    }

    // --- 全リールが止まっているかチェック ---
    private bool AllReelsStopped()
    {
        foreach (var reel in slotReels)
        {
            if (reel.IsSpinning) return false;
        }
        return true;
    }
}
