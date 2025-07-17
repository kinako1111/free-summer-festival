using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelManager : MonoBehaviour
{
	[Header("全体設定")]
	public float spinSpeed = 20f;          // 共通回転スピード（symbols/秒）
	public float minSpinTime = 2f;         // 最短スピン時間
	public float maxSpinTime = 3.5f;       // 最長スピン時間

	[Header("対象リール一覧")]
	public List<SlotReel> reels;

	void Start()
	{
		StartCoroutine(SpinAllReelsWithDelay());
	}

	IEnumerator SpinAllReelsWithDelay()
	{
		foreach (var reel in reels)
		{
			float randomDuration = Random.Range(minSpinTime, maxSpinTime);
			reel.SetSpinSettings(spinSpeed, randomDuration);
			reel.StartSpin();
			yield return new WaitForSeconds(0.3f); // ずらしてスタート
		}
	}
}
