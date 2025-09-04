using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
	[SerializeField] Button startButton; 

	private void Start()
	{
		if (startButton != null)
		{
			startButton.onClick.AddListener(OnStartButtonClicked);
		}
	}

	private void OnStartButtonClicked()
	{
		SceneManager.LoadScene("SlotScene");
	}
}
