using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Animator _animator = default;


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
        //Replace with idea of player weapon


        StartCoroutine(MovementCooldown(0.35f));

    }

    IEnumerator MovementCooldown(float cooldownTime)
    {
        _playerMovement.enabled = false;
        _animator.speed = 1;
        _animator.SetBool("IsAttacking", true);
        _playerWeapon.AttackState(true);
        yield return new WaitForSeconds(cooldownTime);
        _animator.SetBool("IsAttacking", false);
        _playerWeapon.AttackState(false);
        _playerMovement.enabled = true;
    }
}
