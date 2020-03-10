using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	private enum DoorType { VerticalDoor, HorizontalDoor };
	[SerializeField] private DoorType _doorType;
	[SerializeField] private Transform _playerOnLoadScenePoint;
	[SerializeField] private Transform _cameraOnLoadScenePoint;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			if (_doorType == DoorType.HorizontalDoor)
			{
				player.transform.position = new Vector2(_playerOnLoadScenePoint.position.x, player.transform.position.y);
			}
			else
			{
				player.transform.position = new Vector2(player.transform.position.x, _playerOnLoadScenePoint.position.y);
			}
			Camera.main.transform.position = _cameraOnLoadScenePoint.position;
		}
	}
}
