using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	[SerializeField] private SceneName _sceneName;
	[SerializeField] private Transform _playerOnLoadScenePoint;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerMovement player = collision.GetComponent<PlayerMovement>();
		if (player != null)
		{
			GameManager.Instance.LoadScene(_sceneName, _playerOnLoadScenePoint.position);
		}
	}
}
