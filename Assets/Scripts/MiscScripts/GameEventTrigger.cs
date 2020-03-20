using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
	[SerializeField] private Switch _switch;
	[SerializeField] private GameObject _enemiesToSpawn;
	[SerializeField] private GameObject _enemiesToDespawn;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			_enemiesToSpawn.SetActive(true);
			_enemiesToDespawn.SetActive(false);
			_switch.OpenSwitch(false);
		}
	}
}
