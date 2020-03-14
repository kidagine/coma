using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName { Logo = 0, MainMenu = 1, LazarusDungeon	 = 2 };

public class GameManager : MonoBehaviour
{
	private PlayerInput _playerInput;
	private Vector2 _currentPlayerPositionOnLoad;
	private Vector2 _lastPlayerPositionOnLoad;

	public static GameManager Instance { get; private set; }
	public bool IsGameStateFrozen { get; private set; }

	void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
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
		IsGameStateFrozen = true;
		_playerInput.enabled = false;
		Time.timeScale = 0.0f;
	}

	public void UnFreezeGameState()
	{
		IsGameStateFrozen = false;
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
		Player player = FindObjectOfType<Player>();
		_playerInput = FindObjectOfType<PlayerInput>();

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
