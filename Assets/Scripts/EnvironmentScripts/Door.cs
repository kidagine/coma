using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
	[SerializeField] private BoxCollider2D _boxCollider;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Sprite _openDoor;


	public void Interact()
	{
		_spriteRenderer.sprite = _openDoor;
		_boxCollider.enabled = false;
	}

	public InteractableType GetInteractableType()
	{
		return InteractableType.Door;
	}
}
