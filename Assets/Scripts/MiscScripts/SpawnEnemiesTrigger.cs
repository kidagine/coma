using System.Collections;
using UnityEngine;

public class SpawnEnemiesTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _enemiesObject;
	[SerializeField] private GameObject _door1;
	[SerializeField] private GameObject _door2;
	[SerializeField] private GameObject _doorSprite1;
	[SerializeField] private GameObject _doorSprite2;
	[SerializeField] private int _totalEnemies;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out Player player))
		{
			_enemiesObject.SetActive(true);
			StartCoroutine(SetDoorActivation(false));
		}
	}

	public void DecreaseEnemies()
	{
		_totalEnemies--;
		if (_totalEnemies <= 0)
		{
			StartCoroutine(SetDoorActivation(true));
		}
	}

	IEnumerator SetDoorActivation(bool isActive)
	{
		yield return new WaitForSeconds(1.0f);
		if (isActive)
		{
			AudioManager.Instance.Play("ClearRoom");
		}
		else
		{
			AudioManager.Instance.Play("LockRoom");
		}
		_door1.SetActive(isActive);
		_door2.SetActive(isActive);
		_doorSprite1.SetActive(isActive);
		_doorSprite2.SetActive(isActive);
	}
}
