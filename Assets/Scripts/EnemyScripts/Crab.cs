using System;
using System.Collections;
using UnityEngine;

public class Crab : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private readonly float _movementChangeCoolDown = 1.2f;
    private readonly int _knockbackForce = 10;
    private readonly int _expAcquired = 10;
    private  int _walkSpeed = 2;
    private float _currentMovementChangeCoolDown;
    private int _health = 2;
    private bool _isFrozen;


    private void Start()
    {
        _currentMovementChangeCoolDown = _movementChangeCoolDown;
        _rigidbody.velocity = Vector2.right * _walkSpeed;
    }

    void Update()
    {
        if (!_isFrozen)
        {
            _currentMovementChangeCoolDown -= Time.deltaTime;
            if (_currentMovementChangeCoolDown <= 0)
            {
                ChangeMovementDirection();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeMovementDirection();
    }

    private void ChangeMovementDirection()
    {
        _walkSpeed *= -1;
        _rigidbody.velocity = Vector2.right * _walkSpeed;
        _currentMovementChangeCoolDown = _movementChangeCoolDown;
    }

    public void Damaged(GameObject playerObject)
    {
        AudioManager.Instance.Play("EnemyDamaged");
        _health--;
        if (_health <= 0)
        {
            AudioManager.Instance.Play("EnemyDies");
            playerObject.GetComponent<Player>().IncreaseExp(_expAcquired);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        KnockBack(playerObject);
    }

    private void KnockBack(GameObject attackObject)
    {
        Vector2 direction = (transform.position - attackObject.transform.position).normalized;
        Vector2 strictDirection = new Vector2(Mathf.Round(direction.x * Convert.ToInt32(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))), Mathf.Round(direction.y * Convert.ToInt32(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))));
        _rigidbody.velocity = strictDirection * _knockbackForce;
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        _rigidbody.velocity = Vector2.zero;
    }

    private void OnEnable()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return null;
        _rigidbody.velocity = Vector2.zero;
        _isFrozen = true;
        _spriteRenderer.enabled = false;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        _isFrozen = false;
        _spriteRenderer.enabled = true;
    }
}
