using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotResultChecker : MonoBehaviour
{
	[SerializeField] SlotReelManager reelManager;   // リール管理
	[SerializeField] MedalManager medalManager;     // メダル管理
	[SerializeField] TextMeshProUGUI resultText;    // 結果表示用テキスト
	[SerializeField] float messageDuration = 2f;    // メッセージ表示時間
	[SerializeField] AudioManager audioManager;     // 効果音管理

 　 void Start()
	{
		// 結果テキストの初期化
		if (resultText != null)
		{
			resultText.text = "";
		}
	}


	// ストップ後に役判定して当たり/ハズレを処理
	public void CheckResult()
	{
		SymbolData[,] grid = reelManager.GetStoppedGrid(); // 3x3配列
		int rows = grid.GetLength(0);
		int cols = grid.GetLength(1);

		if (rows < 3 || cols < 3) return;

		int totalPayout = 0;

		// 横ライン判定
		for (int r = 0; r < 3; r++)
		{
			SymbolType[] line = new SymbolType[] { grid[r, 0].type, grid[r, 1].type, grid[r, 2].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
				totalPayout += payout;
		}

		// 縦ライン判定
		for (int c = 0; c < 3; c++)
		{
			SymbolType[] line = new SymbolType[] { grid[0, c].type, grid[1, c].type, grid[2, c].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
				totalPayout += payout;
		}

		// 斜めライン判定
		{
			SymbolType[] line = new SymbolType[] { grid[0, 0].type, grid[1, 1].type, grid[2, 2].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
				totalPayout += payout;

			line = new SymbolType[] { grid[0, 2].type, grid[1, 1].type, grid[2, 0].type };
			payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
				totalPayout += payout;
		}

		// メダル加算とメッセージ処理
		if (totalPayout > 0)
		{
			medalManager.AddMedals(totalPayout);
			StartCoroutine(ShowMessage($"当たり！ +{totalPayout}枚"));

			// SE再生（当たり）
			if (audioManager != null)
				audioManager.PlayWinSE();
		}
		else
		{
			// SE再生（ハズレ）
			if (audioManager != null)
				audioManager.PlayLoseSE();
		}
	}

	// Bird はワイルドカード扱い（他シンボルに変身可能）
	private SymbolType[] ProcessBirdWild(SymbolType[] line)
	{
		SymbolType[] result = new SymbolType[line.Length];

		// Bird以外のシンボルを優先的にターゲットにする
		SymbolType target = SymbolType.None;
		foreach (var s in line)
		{
			if (s != SymbolType.Bird && s != SymbolType.None)
			{
				target = s;
				break;
			}
		}

		// Bird をターゲットシンボルに変換
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == SymbolType.Bird)
				result[i] = (target != SymbolType.None) ? target : SymbolType.Bird;
			else
				result[i] = line[i];
		}

		return result;
	}

	// ラインの役判定（払い出し枚数を返す）
	private int EvaluateLine(SymbolType[] line)
	{
		// None が混ざってたら役なし
		foreach (var s in line)
			if (s == SymbolType.None)
				return 0;

		int payout = 0;

		// 役の判定
		if (line[0] == SymbolType.Bird && line[1] == SymbolType.Bird && line[2] == SymbolType.Bird)
			payout = 500;
		else if (line[0] == SymbolType.Seven_Red && line[1] == SymbolType.Seven_Red && line[2] == SymbolType.Seven_Red)
			payout = 100;
		else if (line[0] == SymbolType.Seven_Blue && line[1] == SymbolType.Seven_Blue && line[2] == SymbolType.Seven_Blue)
			payout = 100;
		else if (line[0] == SymbolType.Bar_White && line[1] == SymbolType.Bar_White && line[2] == SymbolType.Bar_White)
			payout = 10;
		else if (line[0] == SymbolType.Bar_Orange && line[1] == SymbolType.Bar_Orange && line[2] == SymbolType.Bar_Orange)
			payout = 30;
		else if (line[0] == SymbolType.Bar_Red && line[1] == SymbolType.Bar_Red && line[2] == SymbolType.Bar_Red)
			payout = 50;
		else if (line[0].ToString().Contains("Bar") && line[1].ToString().Contains("Bar") && line[2].ToString().Contains("Bar"))
			payout = 5;

		return payout;
	}

	// メッセージを一時的に表示する処理
	private IEnumerator ShowMessage(string msg)
	{
		resultText.text = msg;
		yield return new WaitForSeconds(messageDuration);
		resultText.text = "";
	}
}
