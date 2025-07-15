using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
	[Header("スロットのリール")]
	public List<SlotReel> slotReels;

	public void SpinAll()
	{
		foreach (SlotReel reel in slotReels)
		{
			reel.StartSpin();
		}
	}
}
