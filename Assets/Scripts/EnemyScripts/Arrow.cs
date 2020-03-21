using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject _arrowExplosionPrefab;
    [SerializeField] private Rigidbody2D _rigidbody;

    public void FireArrow(Transform target, float fireArrowForce)
    {
        transform.right = target.transform.position - transform.position;
        _rigidbody.AddForce(transform.right * fireArrowForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IEnemy enemy))
        {
            if (collision.TryGetComponent(out BoxCollider2D boxCollider))
            {
                if (!boxCollider.isTrigger)
                {
                    AudioManager.Instance.Play("EnemyDies");
                    Instantiate(_arrowExplosionPrefab, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}
