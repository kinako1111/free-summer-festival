using UnityEngine;
using UnityEngine.UI;

public class SlotGameManager : MonoBehaviour
{
	[SerializeField] private SlotReel[] slotReels;
	[SerializeField] private Button spinOnceButton;
	[SerializeField] private Button autoSpinButton;
	[SerializeField] private float autoSpinDelay = 0.5f;

	private bool isAutoSpinning = false;

	void Start()
	{
		spinOnceButton.onClick.AddListener(SpinOnce);
		autoSpinButton.onClick.AddListener(ToggleAutoSpin);
	}

	void Update()
	{
		if (isAutoSpinning && AllReelsStopped())
		{
			Invoke(nameof(SpinAll), autoSpinDelay);
		}
	}

	private void SpinOnce()
	{
		SpinAll();
	}

	private void ToggleAutoSpin()
	{
		isAutoSpinning = !isAutoSpinning;
		if (isAutoSpinning && AllReelsStopped())
		{
			SpinAll();
		}
	}

	private void SpinAll()
	{
		foreach (var reel in slotReels)
		{
			reel.StartSpinning();
		}
	}

	private bool AllReelsStopped()
	{
		foreach (var reel in slotReels)
		{
			if (reel.IsSpinning) return false;
		}
		return true;
	}
}
