using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Transform _pickUpPoint;
    [SerializeField] private Animator _animator = default;
    private GameObject _pickableObject;
    private GameObject _throwableObject;
    private GameObject _interactableObject;
    private bool _canAttack = true;


    void Awake()
    {
        if (GlobalSettings.hasWeaponEquiped)
        {
            _weaponObject.SetActive(true);
        }
    }

    public void PickUp()
    {
        if (_pickableObject != null)
        {
            IPickable pickable = _pickableObject.GetComponent<IPickable>();
            switch (pickable.GetPickableType())
            {
                case PickableType.Weapon:
                    EquipWeapon();
                    break;
                case PickableType.Key:
                    PickUpKey();
                    break;
            }
            pickable.Picked();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickable pickable))
        {
            UIManager.Instance.ShowUIPrompt(collision.transform);
            _pickableObject = collision.gameObject;
        }
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.GetInteractableType() == InteractableType.Door && _throwableObject != null)
            {
                UIManager.Instance.ShowUIPrompt(collision.transform);
                _interactableObject = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickable pickable))
        {
            UIManager.Instance.HideUIPrompt();
            _pickableObject = null;
        }
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            UIManager.Instance.HideUIPrompt();
            _interactableObject = null;
        }
    }

    public void Interact()
    {
        if (_interactableObject != null)
        {
            IInteractable interactable = _interactableObject.GetComponent<IInteractable>();
            switch (interactable.GetInteractableType())
            {
                case InteractableType.Door:
                    AudioManager.Instance.Play("UseKey");
                    Destroy(_throwableObject);
                    break;
            }
            interactable.Interact();
        }
    }

    public void EquipWeapon()
    {
        UIManager.Instance.HideUIPrompt();
        UIManager.Instance.ShowUIItem();
        AudioManager.Instance.Play("ShowPickup");
        GlobalSettings.hasWeaponEquiped = true;
        _weaponObject.SetActive(true);
        GlobalSettings.hasPickedBrokenDagger = true;
    }

    public void PickUpKey()
    {
        AudioManager.Instance.Play("PickUp");
        _throwableObject = _pickableObject;
        _throwableObject.transform.SetParent(_pickUpPoint);
        _throwableObject.transform.position = _pickUpPoint.position;
    }

    public void Throw()
    {
        if (_throwableObject != null)
        {
            AudioManager.Instance.Play("Throw");
            Vector2 throwDirection = _playerMovement.CurrentDirection;
            _throwableObject.GetComponent<IPickable>().Throw(throwDirection);
            _throwableObject = null;
        }
    }

    public void Attack()
    {
		if (_canAttack && _throwableObject == null)
		{
            AudioManager.Instance.Play("WeaponAttack");
			_canAttack = false;
			StartCoroutine(AttackCooldown(0.35f));
		}
	}

    IEnumerator AttackCooldown(float cooldownTime)
    {
		_animator.SetBool("IsAttacking", true);
		_playerMovement.enabled = false;
        _playerWeapon.AttackState(true);
        yield return new WaitForSeconds(cooldownTime);
        _animator.SetBool("IsAttacking", false);
        _playerWeapon.AttackState(false);
        _playerMovement.enabled = true;
		_canAttack = true;
    }
}
