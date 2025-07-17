using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolPrefab
{
	public string name;             // �V���{���̖��O�i��: "redBar"�j
	public GameObject prefab;       // �Ή�����Prefab
}

public class SymbolManager : MonoBehaviour
{
	[Header("�g�p����V���{���ꗗ")]
	public List<SymbolPrefab> symbolPrefabs;

	// �����_���ɃV���{����Prefab��Ԃ�
	public GameObject GetRandomSymbol()
	{
		int index = Random.Range(0, symbolPrefabs.Count);
		return symbolPrefabs[index].prefab;
	}

	// ���O����Prefab���擾
	public GameObject GetSymbolByName(string symbolName)
	{
		foreach (var symbol in symbolPrefabs)
		{
			if (symbol.name == symbolName)
			{
				return symbol.prefab;
			}
		}
		Debug.LogWarning($"SymbolManager: ���O {symbolName} ��Prefab��������܂���ł���");
		return null;
	}
}
