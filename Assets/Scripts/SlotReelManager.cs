using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReelManager : MonoBehaviour
{
	[Header("�S�̐ݒ�")]
	public float spinSpeed = 20f;          // ���ʉ�]�X�s�[�h�isymbols/�b�j
	public float minSpinTime = 2f;         // �ŒZ�X�s������
	public float maxSpinTime = 3.5f;       // �Œ��X�s������

	[Header("�Ώۃ��[���ꗗ")]
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
			yield return new WaitForSeconds(0.3f); // ���炵�ăX�^�[�g
		}
	}
}
