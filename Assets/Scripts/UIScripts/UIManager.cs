using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject _prompt;
	[SerializeField] private SceneName _sceneName;

	public static UIManager Instance { get; private set; }


	void Awake()
	{
		CheckInstance();
	}

	private void CheckInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void ShowUIPrompt()
	{
		_prompt.SetActive(true);
	}

	public void HideUIPrompt()
	{
		_prompt.SetActive(false);
	}
}
