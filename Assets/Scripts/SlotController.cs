using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
	public List<SlotReel> reels;

	public void SpinAll()
	{
		foreach (SlotReel reel in reels)
		{
			reel.StartSpin();
		}
	}
}
