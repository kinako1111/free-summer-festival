using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
	[Header("リール上に表示されるImageコンポーネント（上→下）")]
	public List<Image> slots; 

	[Header("シンボル一覧")]
	public List<Symbol> symbolList;

	[Header("スピン設定")]
	public float spinDuration = 2f;     // リールの回転時間
	public float spinInterval = 0.05f;  // 回転の速さ（短いほど速い）

	private bool isSpinning = false;

	void Start()
	{
		InitSymbols();
	}

	void InitSymbols()
	{
		foreach (var slot in slots)
		{
			slot.sprite = GetRandomSymbol().sprite;
		}
	}

	public void StartSpin()
	{
		if (!isSpinning)
		StartCoroutine(SpinRoutine());
	}

	IEnumerator SpinRoutine()
	{
		isSpinning = true;
		float timer = 0f;

		while (timer < spinDuration)
		{
			foreach (var slot in slots)
			{
				Symbol symbol = GetRandomSymbol();
				slot.sprite = symbol.sprite;
			}

			timer += spinInterval;
			yield return new WaitForSeconds(spinInterval);
		}

		// 最終停止時のシンボル
		foreach (var slot in slots)
		{
			Symbol symbol = GetRandomSymbol();
			slot.sprite = symbol.sprite;
		}

		isSpinning = false;
	}

	Symbol GetRandomSymbol()
	{
		int index = Random.Range(0, symbolList.Count);
		return symbolList[index];
	}
}
