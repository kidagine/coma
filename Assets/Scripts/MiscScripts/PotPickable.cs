using System.Collections;
using UnityEngine;

public class PotPickable : MonoBehaviour, IPickable
{
    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private BoxCollider2D _triggerCollider;
    [SerializeField] private BoxCollider2D _normalCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    private readonly int _throwForce = 600;
    private Vector2 _throwDirection;
    private bool _isThrown;

    public void Picked()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Player");
        _spriteRenderer.sortingLayerName = "Foreground";
        _triggerCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_isThrown)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.gravityScale = 4;
            _rigidbody.AddForce(_throwDirection * _throwForce);
            _isThrown = false;
        }
    }

    public void Throw(Vector2 throwDirection)
    {
        _throwDirection = throwDirection;
        StartCoroutine(ThrowCoroutine());
    }

    IEnumerator ThrowCoroutine()
    {
        _isThrown = true;
        transform.SetParent(null);
        _normalCollider.enabled = true;

        yield return new WaitForSeconds(0.2f);
        _obstacle.Destroy();
    }

    public PickableType GetPickableType()
    {
        return PickableType.Key;
    }
}

