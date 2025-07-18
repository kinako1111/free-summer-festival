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
	public float symbolHeight = 1.5f;

	[Header("回転設定")]
	public float spinSpeed = 20f;
	public float spinDuration = 2f; // ← public化して他から参照可能に

	private int visibleCount = 1;
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

	public void InitReel()
	{
		ClearSymbols();
		for (int i = 0; i < visibleCount; i++)
		{
			SpawnSymbolAt(i);
		}
	}

	public void ClearSymbols()
	{
		foreach (var obj in symbols)
		{
			Destroy(obj);
		}
		symbols.Clear();
	}

	public void SpawnSymbolAt(int index)
	{
		GameObject go = Instantiate(symbolManager.GetRandomSymbol(), reelTransform);
		go.transform.localPosition = new Vector3(0, -index * symbolHeight, 0);
		symbols.Add(go);
	}

	public void StartSpinning()
	{
		if (!isSpinning)
			StartCoroutine(SpinRoutine());
	}

	public void StartSpin() // 呼び出し側で柔軟に使えるようエイリアス
	{
		StartSpinning();
	}

	public bool IsSpinning => isSpinning;

	private IEnumerator SpinRoutine()
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
			symbols.Insert(0, newSymbol);

			for (int i = 0; i < symbols.Count; i++)
			{
				symbols[i].transform.localPosition = new Vector3(0, -i * symbolHeight, 0);
			}

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
	}
}
