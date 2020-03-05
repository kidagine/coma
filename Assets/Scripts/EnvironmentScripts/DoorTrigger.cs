using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	[SerializeField] private SceneNames _sceneName;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerMovement player = collision.GetComponent<PlayerMovement>();
		if (player != null)
		{
			GameManager.Instance.LoadScene(_sceneName);
		}
	}
}
