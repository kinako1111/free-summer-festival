using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
	[Header("リールのシンボル親")]
	public Transform reelTransform;

	[Header("絵柄の管理クラス")]
	public SymbolManager symbolManager;

	[Header("リール構成")]
	public int visibleCount = 3;          // 表示する絵柄数
	public float symbolHeight = 1.5f;     // 1個分の高さ

	private float spinSpeed = 20f;        // 共通スピード（symbols/秒）
	private float spinDuration = 2f;      // このリールの回転時間
	private bool isSpinning = false;
	private List<GameObject> symbols = new List<GameObject>();

	void Start()
	{
		InitReel();
	}

	public void SetSpinSettings(float speed, float duration)
	{
		spinSpeed = speed;
		spinDuration = duration;
	}

	void InitReel()
	{
		ClearSymbols();
		for (int i = 0; i < visibleCount; i++)
		{
			SpawnSymbolAt(i);
		}
	}

	void ClearSymbols()
	{
		foreach (var obj in symbols)
		{
			Destroy(obj);
		}
		symbols.Clear();
	}

	void SpawnSymbolAt(int index)
	{
		GameObject go = Instantiate(symbolManager.GetRandomSymbol(), reelTransform);
		go.transform.localPosition = new Vector3(0, -index * symbolHeight, 0);
		symbols.Add(go);
	}

	public void StartSpin()
	{
		if (!isSpinning)
			StartCoroutine(SpinRoutine());
	}

	IEnumerator SpinRoutine()
	{
		isSpinning = true;

		float elapsed = 0f;
		float interval = 1f / spinSpeed;

		while (elapsed < spinDuration)
		{
			if (symbols.Count > 0)
			{
				Destroy(symbols[symbols.Count - 1]);
				symbols.RemoveAt(symbols.Count - 1);
			}

			GameObject newSymbol = Instantiate(symbolManager.GetRandomSymbol(), reelTransform);
			newSymbol.transform.localPosition = new Vector3(0, symbolHeight, 0);

			foreach (var symbol in symbols)
			{
				symbol.transform.localPosition -= new Vector3(0, symbolHeight, 0);
			}

			symbols.Insert(0, newSymbol);

			elapsed += interval;
			yield return new WaitForSeconds(interval);
		}

		while (symbols.Count > visibleCount)
		{
			Destroy(symbols[symbols.Count - 1]);
			symbols.RemoveAt(symbols.Count - 1);
		}

		for (int i = 0; i < symbols.Count; i++)
		{
			symbols[i].transform.localPosition = new Vector3(0, -i * symbolHeight, 0);
		}

		isSpinning = false;

		// 警告回避用
		yield break;
	}

}
