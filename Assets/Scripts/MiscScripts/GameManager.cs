using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName { Logo = 0, MainMenu = 1, Room01 = 2, Room02 = 3 };

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	private Vector2 _currentPlayerPositionOnLoad;
	private Vector2 _lastPlayerPositionOnLoad;

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

	public void FreezeGameState()
	{
		_playerInput.enabled = false;
		Time.timeScale = 0.0f;
	}

	public void UnFreezeGameState()
	{
		_playerInput.enabled = true;
		Time.timeScale = 1.0f;
	}

	public void LoadScene(SceneName sceneName)
	{
		SceneManager.LoadScene((int) sceneName);
	}

	public void LoadScene(SceneName sceneName, Vector2 playerPositionOnLoad)
	{
		_currentPlayerPositionOnLoad = playerPositionOnLoad;
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
		PlayerMovement player = FindObjectOfType<PlayerMovement>();
		if (player != null)
		{
			Transform playerTransform = player.transform;

			if (_lastPlayerPositionOnLoad != Vector2.zero)
			{
				playerTransform.position = _lastPlayerPositionOnLoad;
			}
			_lastPlayerPositionOnLoad = _currentPlayerPositionOnLoad;
		}
	}
}
