using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotGameManager : MonoBehaviour
{
	[SerializeField] SlotReelManager reelManager;        // リール管理
	[SerializeField] MedalManager medalManager;          // メダル管理
	[SerializeField] SlotResultChecker slotResultChecker;// 結果判定
	[SerializeField] Button spinOnceButton;              // 1回スピンボタン
	[SerializeField] Button autoSpinButton;              // オートスピンボタン
	[SerializeField] float autoSpinDelay = 0.5f;         // オートスピン間隔
	[SerializeField] AudioManager audioManager;          // 効果音管理

	private bool isAutoSpinning = false; 
	private bool isSpinning = false;     

	private void Start()
	{
		spinOnceButton.onClick.AddListener(OnSpinOnceButtonClicked);
		autoSpinButton.onClick.AddListener(OnAutoSpinButtonClicked);
	}

	// オートスピンの ON/OFF を切り替える
	void ToggleAutoSpin()
	{
		isAutoSpinning = !isAutoSpinning;
		if (isAutoSpinning && !isSpinning)
			StartCoroutine(SpinRoutine());
	}

	// スロットのスピン処理
	private IEnumerator SpinRoutine()
	{
		// 既にスピン中なら処理しない
		if (isSpinning) yield break;

		// メダルを消費できなければスピン中止
		if (!medalManager.SpendMedals())
		{
			Debug.Log("メダルが足りないのでスピンできません。");
			yield break;
		}

		isSpinning = true;

		// SE再生（スピン開始）
		if (audioManager != null)
			audioManager.PlaySpinSE();

		// リールをすべて回転開始
		reelManager.SpinAllReels();

		// 全リールが停止するまで待機
		while (!reelManager.AreAllReelsStopped())
			yield return null;

		// 停止後に当たり判定を実行
		slotResultChecker.CheckResult();

		// 結果メッセージの表示時間を確保
		yield return new WaitForSeconds(1f);

		isSpinning = false;

		// オートスピン中なら次のスピンを自動開始
		if (isAutoSpinning)
		{
			yield return new WaitForSeconds(autoSpinDelay);
			StartCoroutine(SpinRoutine());
		}
	}
	private void OnSpinOnceButtonClicked()
	{
		StartCoroutine(SpinRoutine());
	}
	private void OnAutoSpinButtonClicked()
	{
		ToggleAutoSpin();
	}
}
