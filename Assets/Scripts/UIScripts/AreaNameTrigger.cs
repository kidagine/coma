using UnityEngine;

public class AreaNameTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _AreaNameUI;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			_AreaNameUI.SetActive(true);
		}
	}
}
