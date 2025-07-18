using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolData
{
	public GameObject symbolPrefab; // 出現させるシンボル（GameObject）
	[Range(0f, 1f)] public float probability; // 出現確率（0〜1の範囲）
}

public class SymbolManager : MonoBehaviour
{
	[Header("出現させたいシンボルと確率")]
	public List<SymbolData> symbols = new List<SymbolData>();

	// シンボルをランダムで1つ返す（確率に応じて）
	public GameObject GetRandomSymbol()
	{
		float total = 0f;

		foreach (var symbol in symbols)
		{
			total += symbol.probability;
		}

		if (total <= 0f)
		{
			Debug.LogWarning("SymbolManager: 合計確率が0以下です");
			return null;
		}

		float rand = Random.Range(0f, total);
		float cumulative = 0f;

		foreach (var symbol in symbols)
		{
			cumulative += symbol.probability;
			if (rand <= cumulative)
			{
				return symbol.symbolPrefab;
			}
		}

		// 念のため
		return symbols[symbols.Count - 1].symbolPrefab;
	}

	// 登録されたシンボル数を返す（SlotReelなどから使う用）
	public int GetSymbolCount()
	{
		return symbols.Count;
	}

	// インデックスで指定したシンボルを返す
	public GameObject GetSymbolByIndex(int index)
	{
		if (index >= 0 && index < symbols.Count)
		{
			return symbols[index].symbolPrefab;
		}
		return null;
	}
}
