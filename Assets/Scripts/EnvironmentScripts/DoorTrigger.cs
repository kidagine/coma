using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	[SerializeField] private SceneName _sceneName;
	[SerializeField] private Transform _playerOnLoadScenePoint;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player player = collision.GetComponent<Player>();
		if (player != null)
		{
			GameManager.Instance.LoadScene(_sceneName, _playerOnLoadScenePoint.position);
		}
	}
}
