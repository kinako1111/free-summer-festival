using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : MonoBehaviour
{
	[Header("出現させたいシンボルと確率")]
	public List<SymbolData> symbols = new List<SymbolData>();

	// 確率で SymbolData を返す
	public SymbolData GetRandomSymbolData()
	{
		float total = 0f;
		foreach (var symbol in symbols)
		{
			total += symbol.probability;
		}

		float rand = Random.Range(0f, total);
		float cumulative = 0f;

		foreach (var symbol in symbols)
		{
			cumulative += symbol.probability;
			if (rand <= cumulative)
				return symbol;
		}

		// 念のため最後を返す
		return symbols[symbols.Count - 1];
	}
}
