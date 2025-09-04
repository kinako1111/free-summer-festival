using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip betSE;
	public AudioClip spinSE;
	public AudioClip winSE;
	public AudioClip loseSE;

	private AudioSource audioSource;

	void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void PlayBetSE()
	{
		if (betSE != null) audioSource.PlayOneShot(betSE);
	}

	public void PlaySpinSE()
	{
		if (spinSE != null) audioSource.PlayOneShot(spinSE);
	}

	public void PlayWinSE()
	{
		if (winSE != null) audioSource.PlayOneShot(winSE);
	}

	public void PlayLoseSE()
	{
		if (loseSE != null) audioSource.PlayOneShot(loseSE);
	}
}
