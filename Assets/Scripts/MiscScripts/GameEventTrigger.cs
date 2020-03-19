using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
	[SerializeField] private Switch _switch;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			_switch.OpenSwitch(false);
		}
	}
}
