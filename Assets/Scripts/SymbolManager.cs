using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : MonoBehaviour
{
	// 出現させたいシンボルと確率
	public List<SymbolData> symbols = new List<SymbolData>();

	public SymbolData GetRandomSymbolData()
	{
		float total = 0f;

		// 全シンボルの確率を合計する
		foreach (var symbol in symbols)
		{
			total += symbol.probability;
		}

		// 合計確率 の範囲で乱数を出す
		float rand = Random.Range(0f, total);
		float cumulative = 0f;

		// 確率の帯を累積しながら乱数が入る場所を探す
		foreach (var symbol in symbols)
		{
			cumulative += symbol.probability;

			// 乱数がこのシンボルの帯の範囲に入っていれば確定
			if (rand <= cumulative)
				return symbol;
		}
		return symbols[symbols.Count - 1];
	}
}
