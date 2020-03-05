using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames { MainMenu = 0, Room01 = 1, Room02 = 2 };

public class GameManager : MonoBehaviour
{
	private Vector2 _playerPositionOnLoad;

	public static GameManager Instance { get; private set; }


	void Awake()
	{
		DontDestroyOnLoad(gameObject);
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

	public void LoadScene(SceneNames sceneName)
	{
		SceneManager.LoadScene((int) sceneName);
	}

	public void LoadScene(SceneNames sceneName, Vector2 playerPositionOnLoad)
	{
		_playerPositionOnLoad = playerPositionOnLoad;
		SceneManager.LoadScene((int)sceneName);
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
		if (playerTransform != null)
		{
			playerTransform.position = _playerPositionOnLoad;
		}
	}
}
