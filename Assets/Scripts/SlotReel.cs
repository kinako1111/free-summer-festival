using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
	[SerializeField] float spinSpeed = 20f;        // スピン速度
	[SerializeField] float spinDuration = 2f;      // スピンの継続時間（秒）
	[SerializeField] Transform reelTransform;
	[SerializeField] SymbolManager symbolManager;

	private bool isSpinning = false;              
	private GameObject currentSymbolGO;           
	private SymbolInstance currentSymbol;          

	private List<SymbolData> stoppedSymbols = new List<SymbolData>(); // 停止したシンボルの履歴

	// リールが回転中かどうかを外部から参照するプロパティ
	public bool IsSpinning => isSpinning;

	void Start()
	{
		InitReel();
	}

	public void SetSpinSettings(float speed, float duration)
	{
		spinSpeed = speed;
		spinDuration = duration;
	}

	// リールを初期化
	public void InitReel()
	{
		SpawnSymbol();
	}

	// リールの回転を開始する
	public void StartSpin()
	{
		if (!isSpinning)
			StartCoroutine(SpinRoutine());
	}

	// 停止履歴から指定数だけシンボルを取得
	public List<SymbolData> GetLastNSymbols(int n)
	{
		int count = Mathf.Min(n, stoppedSymbols.Count);
		return stoppedSymbols.GetRange(0, count);
	}

	// シンボルをランダムに生成して配置
	private void SpawnSymbol()
	{
		SymbolData data = symbolManager.GetRandomSymbolData();
		if (data.symbolPrefab == null) return;

		if (currentSymbolGO != null)
			Destroy(currentSymbolGO);

		currentSymbolGO = Instantiate(data.symbolPrefab, reelTransform);
		currentSymbolGO.transform.localPosition = Vector3.zero;

		currentSymbol = currentSymbolGO.AddComponent<SymbolInstance>();
		currentSymbol.data = data;
	}

	private IEnumerator SpinRoutine()
	{
		isSpinning = true;
		float elapsed = 0f;
		float interval = 1f / spinSpeed; 

		// 一定時間ランダムにシンボルを切り替える
		while (elapsed < spinDuration)
		{
			SpawnSymbol();
			elapsed += interval;
			yield return new WaitForSeconds(interval);
		}

		// 最終的に止まったシンボルを履歴に保存
		stoppedSymbols.Insert(0, currentSymbol.data);

		isSpinning = false;
	}
}
