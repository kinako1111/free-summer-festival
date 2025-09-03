using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotResultChecker : MonoBehaviour
{
	[SerializeField] private SlotReelManager reelManager;
	[SerializeField] private MedalManager medalManager;
	[SerializeField] private TextMeshProUGUI resultText;
	[SerializeField] private float messageDuration = 2f;

	private void Start()
	{
		if (resultText != null)
			resultText.text = "";
	}

	public void CheckResult()
	{
		SymbolData[,] grid = reelManager.GetStoppedGrid(); // 3x3配列
		int rows = grid.GetLength(0);
		int cols = grid.GetLength(1);

		if (rows < 3 || cols < 3) return;

		int totalPayout = 0;
		HashSet<int> hitReelIndices = new HashSet<int>();

		// 横ライン
		for (int r = 0; r < 3; r++)
		{
			SymbolType[] line = new SymbolType[] { grid[r, 0].type, grid[r, 1].type, grid[r, 2].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
			{
				totalPayout += payout;
				hitReelIndices.Add(0);
				hitReelIndices.Add(1);
				hitReelIndices.Add(2);
			}
		}

		// 縦ライン
		for (int c = 0; c < 3; c++)
		{
			SymbolType[] line = new SymbolType[] { grid[0, c].type, grid[1, c].type, grid[2, c].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
			{
				totalPayout += payout;
				hitReelIndices.Add(c);
			}
		}

		// 斜めライン
		{
			// 左上→右下
			SymbolType[] line = new SymbolType[] { grid[0, 0].type, grid[1, 1].type, grid[2, 2].type };
			int payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
			{
				totalPayout += payout;
				hitReelIndices.Add(0);
				hitReelIndices.Add(1);
				hitReelIndices.Add(2);
			}

			// 右上→左下
			line = new SymbolType[] { grid[0, 2].type, grid[1, 1].type, grid[2, 0].type };
			payout = EvaluateLine(ProcessBirdWild(line));
			if (payout > 0)
			{
				totalPayout += payout;
				hitReelIndices.Add(0);
				hitReelIndices.Add(1);
				hitReelIndices.Add(2);
			}
		}

		// パーティクルは今置いとく
		// foreach (int reelIndex in hitReelIndices)
		//     reelManager.reels[reelIndex].PlayHitEffect();

		// メダル加算とメッセージ表示
		if (totalPayout > 0)
		{
			medalManager.AddMedals(totalPayout);
			StartCoroutine(ShowMessage($"当たり！ +{totalPayout}枚"));
		}
	}

	// Birdはワイルド、Noneは無視
	private SymbolType[] ProcessBirdWild(SymbolType[] line)
	{
		SymbolType[] result = new SymbolType[line.Length];

		// Bird以外でNoneでない最初のシンボルを探す
		SymbolType target = SymbolType.None;
		foreach (var s in line)
		{
			if (s != SymbolType.Bird && s != SymbolType.None)
			{
				target = s;
				break;
			}
		}

		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == SymbolType.Bird)
			{
				// Noneしかなかった場合はそのままBird
				result[i] = (target != SymbolType.None) ? target : SymbolType.Bird;
			}
			else
			{
				result[i] = line[i];
			}
		}

		return result;
	}

	// Noneが含まれると評価しない
	private int EvaluateLine(SymbolType[] line)
	{
		foreach (var s in line)
			if (s == SymbolType.None)
				return 0;

		int payout = 0;

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

	private IEnumerator ShowMessage(string msg)
	{
		resultText.text = msg;
		yield return new WaitForSeconds(messageDuration);
		resultText.text = "";
	}
}
