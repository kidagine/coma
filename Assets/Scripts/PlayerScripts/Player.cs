using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    [SerializeField] private PlayerExpUI _playerExpUI;
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _pickUpPoint;
    [SerializeField] private Animator _animator = default;
    private readonly int _knockbackForce = 10;
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
            pickable.Picked(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPickable pickable))
        {
            UIManager.Instance.ShowUIPrompt(collision.transform, "Press X to pick up");
            _pickableObject = collision.gameObject;
        }
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.GetInteractableType() == InteractableType.Door && _throwableObject != null && collision.GetComponent<Door>().IsLocked())
            {
                UIManager.Instance.ShowUIPrompt(collision.transform, "Press X to open");
                _interactableObject = collision.gameObject;
            }
            if (interactable.GetInteractableType() == InteractableType.Bonfire)
            {
                UIManager.Instance.ShowUIPrompt(collision.transform, "Press X to rest");
                _interactableObject = collision.gameObject;
            }
        }
        if (collision.TryGetComponent(out IEnemy enemy))
        {
            Damaged(collision.gameObject);
        }
        if (collision.CompareTag("Arrow"))
        {
            Damaged(collision.gameObject);
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

    private void Damaged(GameObject enemyObject)
    {
        AudioManager.Instance.Play("Damaged");
        _playerStats.Health--;
        _playerHealthUI.SetHearts(_playerStats.Health);
        if (_playerStats.Health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        KnockBack(enemyObject);
    }

    private void KnockBack(GameObject attackObject)
    {
        _playerMovement.enabled = false;
        Vector2 direction = (transform.position - attackObject.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));
        _rigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        _playerMovement.enabled = true;
        _rigidbody.velocity = Vector2.zero;
    }

    public void Heal()
    {
        AudioManager.Instance.Play("Healed");
        if (_playerStats.Health < 3)
        {
            _playerStats.Health++;
            _playerHealthUI.SetHearts(_playerStats.Health);
        }
    }

    public void IncreaseExp(int expAcquired)
    {
        _playerStats.CurrentExp += expAcquired;
        _playerExpUI.SetExp(_playerStats.CurrentExp);
        if (_playerStats.CurrentExp >= _playerStats.ExpCap)
        {
            _playerStats.CurrentExp = 0;
            _playerExpUI.SetExp(_playerStats.CurrentExp);
            _playerStats.ExpCap += 20;
            _playerExpUI.LevelUp(_playerStats.ExpCap);
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
                    UIManager.Instance.HideUIPrompt();
                    Destroy(_throwableObject);
                    break;
                case InteractableType.Bonfire:
                    UIManager.Instance.HideUIPrompt();
                    AudioManager.Instance.FadeOut("HubWorld");
                    AudioManager.Instance.Play("Bonfire");
                    _playerMovement.enabled = false;
                    _animator.SetTrigger("Rest");
                    Heal();
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
