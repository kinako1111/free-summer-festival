using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolPrefab
{
	public string name;             // シンボルの名前
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
		return null;
	}

	// 名前からインデックスを取得（任意追加）
	public int GetSymbolIndexByName(string symbolName)
	{
		for (int i = 0; i < symbolPrefabs.Count; i++)
		{
			if (symbolPrefabs[i].name == symbolName)
			{
				return i;
			}
		}
		return -1; // 見つからなければ -1
	}
}
