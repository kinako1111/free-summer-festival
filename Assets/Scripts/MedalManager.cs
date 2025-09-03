using UnityEngine;
using TMPro;

public class MedalManager : MonoBehaviour
{
	[Header("初期設定")]
	[SerializeField] int startMedals = 1000;   // 最初の所持メダル
	[SerializeField] int minBet = 1;           // 最低BET枚数
	[SerializeField] int maxBet = 10;          // 最大BET枚数

	[Header("UI参照")]
	[SerializeField] TextMeshProUGUI medalText; // 所持メダル表示用
	[SerializeField] TextMeshProUGUI betText;   // BET枚数表示用

	private int currentMedals;
	private int currentBet;

	private void Start()
	{
		currentMedals = startMedals;

		// 前回のBET枚数をロード（なければminBet）
		currentBet = PlayerPrefs.GetInt("LastBet", minBet);

		UpdateUI();
	}

	// メダルを増やす（当たり発生時）
	public void AddMedals(int basePayout)
	{
		int payout = basePayout * currentBet;
		currentMedals += payout;
		UpdateUI();
	}

	// スピン時にBET分のメダルを消費
	public bool SpendMedals()
	{
		if (currentMedals >= currentBet)
		{
			currentMedals -= currentBet;
			UpdateUI();
			return true;
		}
		else
		{
			Debug.Log("メダルが足りません！");
			return false;
		}
	}

	// BETボタン処理
	public void OnBet()
	{
		currentBet++;

		if (currentBet > maxBet)
			currentBet = minBet; // 最大超えたら最小に戻る

		PlayerPrefs.SetInt("LastBet", currentBet);
		PlayerPrefs.Save();

		UpdateUI();
	}

	public int GetCurrentMedals() => currentMedals;
	public int GetCurrentBet() => currentBet;

	private void UpdateUI()
	{
		if (medalText != null)
			medalText.text = $"所持メダル: {currentMedals}";

		if (betText != null)
			betText.text = $"BET: {currentBet}";
	}
}
