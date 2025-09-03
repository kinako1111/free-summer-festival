using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotGameManager : MonoBehaviour
{
	[SerializeField] private SlotReelManager reelManager;
	[SerializeField] private MedalManager medalManager;
	[SerializeField] private SlotResultChecker slotResultChecker;

	[SerializeField] private Button spinOnceButton;
	[SerializeField] private Button autoSpinButton;
	[SerializeField] private float autoSpinDelay = 0.5f;

	private bool isAutoSpinning = false;
	private bool isSpinning = false;

	private void Start()
	{
		spinOnceButton.onClick.AddListener(() => StartCoroutine(SpinRoutine()));
		autoSpinButton.onClick.AddListener(ToggleAutoSpin);
	}

	private void ToggleAutoSpin()
	{
		isAutoSpinning = !isAutoSpinning;

		// AutoSpin を ON にした瞬間に最初のスピン
		if (isAutoSpinning && !isSpinning)
			StartCoroutine(SpinRoutine());
	}

	private IEnumerator SpinRoutine()
	{
		if (isSpinning) yield break;

		if (!medalManager.SpendMedals())
		{
			Debug.Log("メダルが足りないのでスピンできません。");
			yield break;
		}

		isSpinning = true;

		// スピン開始
		reelManager.SpinAllReels();

		// リールがすべて止まるまで待つ
		while (!reelManager.AreAllReelsStopped())
			yield return null;

		// 当たり判定
		slotResultChecker.CheckResult();

		// 1秒待機してメッセージを表示する時間を確保
		yield return new WaitForSeconds(1f);

		isSpinning = false;

		// AutoSpin 中なら次のスピンへ
		if (isAutoSpinning)
		{
			yield return new WaitForSeconds(autoSpinDelay);
			StartCoroutine(SpinRoutine());
		}
	}
}
