using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelManager : MonoBehaviour
{
	[SerializeField] List<SlotReel> reels;			 // 管理するリールのリスト
	[SerializeField] float reelSpinDelay = 0.3f;	 // 各リールを回し始める間隔

	// 全リールを順番に回す
	public void SpinAllReels()
	{
		StartCoroutine(SpinReelsCoroutine());
	}

	// 全リールが停止したかどうかを確認
	public bool AreAllReelsStopped()
	{
		foreach (var reel in reels)
			if (reel.IsSpinning) return false;
		return true;
	}

	// 各リールの停止履歴から 3×3 のシンボルグリッドを作成
	public SymbolData[,] GetStoppedGrid()
	{
		int rows = 3;              
		int cols = reels.Count;   
		SymbolData[,] grid = new SymbolData[rows, cols];

		for (int c = 0; c < cols; c++)
		{
			// 各リールから直近の3つのシンボルを取得
			List<SymbolData> last = reels[c].GetLastNSymbols(rows);

			for (int r = 0; r < rows; r++)
			{
				// 取得できなければ None を埋める
				grid[r, c] = (r < last.Count) ? last[r] : new SymbolData { type = SymbolType.None };
			}
		}

		return grid;
	}

	// リールを1本ずつ順番にスピン開始させるしくみ
	private IEnumerator SpinReelsCoroutine()
	{
		foreach (SlotReel reel in reels)
		{
			reel.StartSpin();
			yield return new WaitForSeconds(reelSpinDelay);
		}
	}
}
