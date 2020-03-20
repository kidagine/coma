using System.Collections;
using UnityEngine;

public class KeyPickable : MonoBehaviour, IPickable
{
    [SerializeField] private BoxCollider2D _triggerCollider;
    [SerializeField] private BoxCollider2D _normalCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    private readonly int _throwForce = 900;
    private GameObject _player;
    private Transform _shadow;
    private Vector2 _throwDirection;
    private Vector2 _lastThrowPosition;
    private bool _isThrown;

    void Start()
    {
        _shadow = transform.GetChild(0);
    }

    public void Picked(GameObject player)
    {
        _player = player;
        _shadow.gameObject.SetActive(false);
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
        _lastThrowPosition = transform.position;
        _throwDirection = throwDirection;
        StartCoroutine(ThrowCoroutine());
    }

    IEnumerator ThrowCoroutine()
    {
        _isThrown = true;
        transform.SetParent(null);
        _normalCollider.enabled = true;

        yield return new WaitForSeconds(0.2f);
        _shadow.gameObject.SetActive(true);
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector2.zero;
        _spriteRenderer.sortingLayerName = "Midground";
        gameObject.layer = LayerMask.NameToLayer("Default");
        _normalCollider.enabled = false;
        _triggerCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pit"))
        {
            StartCoroutine(ReAppearCoroutine());
        }
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Damaged(_player);
        }
    }

    IEnumerator ReAppearCoroutine()
    {
        AudioManager.Instance.Play("ItemFall");
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.Play("ItemReAppear");
        transform.position = _lastThrowPosition;
        _spriteRenderer.enabled = true;
    }

    public PickableType GetPickableType()
    {
        return PickableType.Key;
    }
}
