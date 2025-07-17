using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolPrefab
{
	public string name;             // シンボルの名前（例: "redBar"）
	public GameObject prefab;       // 対応するPrefab
}

public class SymbolManager : MonoBehaviour
{
	[Header("使用するシンボル一覧")]
	public List<SymbolPrefab> symbolPrefabs;

	// ランダムにシンボルのPrefabを返す
	public GameObject GetRandomSymbol()
	{
		int index = Random.Range(0, symbolPrefabs.Count);
		return symbolPrefabs[index].prefab;
	}

	// 名前からPrefabを取得
	public GameObject GetSymbolByName(string symbolName)
	{
		foreach (var symbol in symbolPrefabs)
		{
			if (symbol.name == symbolName)
			{
				return symbol.prefab;
			}
		}
		Debug.LogWarning($"SymbolManager: 名前 {symbolName} のPrefabが見つかりませんでした");
		return null;
	}
}
