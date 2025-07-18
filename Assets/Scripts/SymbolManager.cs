using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolPrefab
{
	public string name;             // �V���{���̖��O
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
		return null;
	}

	// ���O����C���f�b�N�X���擾�i�C�Ӓǉ��j
	public int GetSymbolIndexByName(string symbolName)
	{
		for (int i = 0; i < symbolPrefabs.Count; i++)
		{
			if (symbolPrefabs[i].name == symbolName)
			{
				return i;
			}
		}
		return -1; // ������Ȃ���� -1
	}
}
