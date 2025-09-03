using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelManager : MonoBehaviour
{
	public List<SlotReel> reels;
	public float spinSpeed = 20f;
	public float spinDuration = 2f;
	public float reelSpinDelay = 0.3f;

	public void SpinAllReels()
	{
		StartCoroutine(SpinReelsCoroutine());
	}

	private IEnumerator SpinReelsCoroutine()
	{
		foreach (SlotReel reel in reels)
		{
			reel.StartSpin();
			yield return new WaitForSeconds(reelSpinDelay);
		}
	}

	public bool AreAllReelsStopped()
	{
		foreach (var reel in reels)
			if (reel.IsSpinning) return false;
		return true;
	}

	// 停止履歴から3×3グリッド作成
	public SymbolData[,] GetStoppedGrid()
	{
		int rows = 3;
		int cols = reels.Count;
		SymbolData[,] grid = new SymbolData[rows, cols];

		for (int c = 0; c < cols; c++)
		{
			List<SymbolData> last = reels[c].GetLastNSymbols(rows);
			for (int r = 0; r < rows; r++)
				grid[r, c] = (r < last.Count) ? last[r] : new SymbolData { type = SymbolType.None };
		}

		return grid;
	}
}
