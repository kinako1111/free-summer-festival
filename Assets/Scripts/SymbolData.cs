using UnityEngine;

public enum SymbolType
{
	None,       // 空欄用
	Bird,
	Seven_Red,
	Seven_Blue,
	Seven_Any,
	Bar_White,
	Bar_Orange,
	Bar_Red,
	Bar_Any
}

[System.Serializable]
public class SymbolData
{
	public GameObject symbolPrefab;      // 出現させるシンボル
	public SymbolType type;              // シンボルの種類
	[Range(0f, 1f)] public float probability; // 出現確率
}
