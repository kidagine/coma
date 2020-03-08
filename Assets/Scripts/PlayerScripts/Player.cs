using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
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
        _animator.SetTrigger("Attack");
        Vector2 attackOrigin = new Vector2(transform.position.x + _playerMovement.CurrentDirection.x, transform.position.y + _playerMovement.CurrentDirection.y);
        RaycastHit2D hit = Physics2D.Raycast(attackOrigin, _playerMovement.CurrentDirection, 1.0f);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Obstacle attackable))
            {
                attackable.Destroy();
            }
        }
        Debug.DrawRay(attackOrigin, _playerMovement.CurrentDirection, Color.red);
        StartCoroutine(MovementCooldown(0.25f));
    }

    IEnumerator MovementCooldown(float cooldownTime)
    {
        _playerMovement.enabled = false;
        yield return new WaitForSeconds(cooldownTime);
        _playerMovement.enabled = true;
    }
}
