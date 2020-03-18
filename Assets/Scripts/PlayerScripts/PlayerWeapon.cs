using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _equipedSprite;
    [SerializeField] private Sprite _attackSprite;
    [SerializeField] private BoxCollider2D _boxCollider2D;
    private bool _isAttacking;


    public void AttackState(bool state)
    {
        if (state)
        {
            _isAttacking = true;
            _spriteRenderer.sprite = _attackSprite;
        }
        else
        {
            _boxCollider2D.enabled = true;
            _isAttacking = false;
            _spriteRenderer.sprite = _equipedSprite;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isAttacking)
        {
            if (collision.TryGetComponent(out Obstacle attackable))
            {
                _boxCollider2D.enabled = false;
                attackable.Destroy();
                _isAttacking = false;
            }
        }
    }
}
