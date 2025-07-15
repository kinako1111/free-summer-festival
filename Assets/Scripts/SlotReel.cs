using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotReel : MonoBehaviour
{
	[Header("���[����ɕ\�������Image�R���|�[�l���g�i�と���j")]
	public List<Image> slots; 

	[Header("�V���{���ꗗ")]
	public List<Symbol> symbolList;

	[Header("�X�s���ݒ�")]
	public float spinDuration = 2f;     // ���[���̉�]����
	public float spinInterval = 0.05f;  // ��]�̑����i�Z���قǑ����j

	private bool isSpinning = false;

	void Start()
	{
		InitSymbols();
	}

	void InitSymbols()
	{
		foreach (var slot in slots)
		{
			slot.sprite = GetRandomSymbol().sprite;
		}
	}

	public void StartSpin()
	{
		if (!isSpinning)
		StartCoroutine(SpinRoutine());
	}

	IEnumerator SpinRoutine()
	{
		isSpinning = true;
		float timer = 0f;

		while (timer < spinDuration)
		{
			foreach (var slot in slots)
			{
				Symbol symbol = GetRandomSymbol();
				slot.sprite = symbol.sprite;
			}

			timer += spinInterval;
			yield return new WaitForSeconds(spinInterval);
		}

		// �ŏI��~���̃V���{��
		foreach (var slot in slots)
		{
			Symbol symbol = GetRandomSymbol();
			slot.sprite = symbol.sprite;
		}

		isSpinning = false;
	}

	Symbol GetRandomSymbol()
	{
		int index = Random.Range(0, symbolList.Count);
		return symbolList[index];
	}
}
