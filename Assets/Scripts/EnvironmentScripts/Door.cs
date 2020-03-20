using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
	[SerializeField] private BoxCollider2D _boxCollider;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Sprite _openDoor;
	[SerializeField] private bool _isLocked;

	public void Interact()
	{
		_isLocked = false;
		_spriteRenderer.sprite = _openDoor;
		_boxCollider.enabled = false;
	}

	public InteractableType GetInteractableType()
	{
		return InteractableType.Door;
	}

	public bool IsLocked()
	{
		return _isLocked;
	}
}
