using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
	[Header("�X���b�g�̃��[��")]
	public List<SlotReel> slotReels;

	public void SpinAll()
	{
		foreach (SlotReel reel in slotReels)
		{
			reel.StartSpin();
		}
	}
}
