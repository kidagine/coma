using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Animator _animator = default;
	private bool _isAttacking;


    void Awake()
    {
        if (GlobalSettings.hasWeaponEquiped)
        {
            _weaponObject.SetActive(true);
        }
    }

    public void PickUp()
    {
        GlobalSettings.hasWeaponEquiped = true;
        _weaponObject.SetActive(true);
    }

    public void Attack()
    {
		if (!_isAttacking)
		{
			_isAttacking = true;
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
		_isAttacking = false;
    }
}
