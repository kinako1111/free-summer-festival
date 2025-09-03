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
	public int visibleCount = 1; // 表示は1つだけ

	[Header("回転設定")]
	public float spinSpeed = 20f;
	public float spinDuration = 2f;

	[Header("停止時に光らせるフレーム")]
	public Transform frameTransform;       // Inspectorでフレームをアタッチ
	public ParticleSystem hitEffect;       // Inspectorでパーティクルをアタッチ

	private bool isSpinning = false;
	private GameObject currentSymbolGO;
	private SymbolInstance currentSymbol;

	private List<SymbolData> stoppedSymbols = new List<SymbolData>(); // 停止履歴

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

	public void InitReel()
	{
		SpawnSymbol();
	}

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

	public void StartSpin()
	{
		if (!isSpinning)
			StartCoroutine(SpinRoutine());
	}

	private IEnumerator SpinRoutine()
	{
		isSpinning = true;
		float elapsed = 0f;
		float interval = 1f / spinSpeed;

		while (elapsed < spinDuration)
		{
			SpawnSymbol();
			elapsed += interval;
			yield return new WaitForSeconds(interval);
		}

		// 停止時のシンボルを履歴に追加
		stoppedSymbols.Insert(0, currentSymbol.data);

		isSpinning = false;
	}

	public List<SymbolData> GetLastNSymbols(int n)
	{
		int count = Mathf.Min(n, stoppedSymbols.Count);
		return stoppedSymbols.GetRange(0, count);
	}

	// --- パーティクル演出 ---
	public void PlayHitEffect()
	{
		if (hitEffect != null && frameTransform != null)
		{
			hitEffect.transform.position = frameTransform.position;
			hitEffect.Play();
		}
	}
	public void PlayHitEffectAtPosition(Vector3 pos)
	{
		if (hitEffect != null)
		{
			hitEffect.transform.position = pos;
			hitEffect.Play();
		}
	}
}
