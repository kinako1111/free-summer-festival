using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelManager : MonoBehaviour
{
	public List<SlotReel> reels;

	[Header("���ʃX�s���ݒ�")]
	public float spinSpeed = 20f;
	public float spinDuration = 2f;
	public float reelSpinDelay = 0.3f;

	void Start()
	{
		foreach (SlotReel reel in reels)
		{
			reel.SetSpinSettings(spinSpeed, spinDuration);
			reel.InitReel(); // �� �e���[����3���̂ݐ��������
		}
	}

	public void SpinAllReels()
	{
		StartCoroutine(SpinReelsCoroutine());
	}

	IEnumerator SpinReelsCoroutine()
	{
		foreach (SlotReel reel in reels)
		{
			reel.StartSpin();
			yield return new WaitForSeconds(reelSpinDelay);
		}
	}

	public bool AreAllReelsStopped()
	{
		foreach (SlotReel reel in reels)
		{
			if (reel.IsSpinning)
				return false;
		}
		return true;
	}
}
