using UnityEngine;

public class Archer : MonoBehaviour, IEnemy
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowPoint;
    private readonly float _fireArrowCooldown = 3.0f;
    private readonly int _fireArrowForce = 200;
    private readonly int _expAcquired = 15;
    private Transform _player;
    private float _currentFireArrowCooldown;
    private int _health = 1;
    private bool _hasDetectedPlayer;


    void Start()
    {
        _currentFireArrowCooldown = _fireArrowCooldown;
    }

    void Update()
    {
        if (_hasDetectedPlayer)
        {
            _currentFireArrowCooldown -= Time.deltaTime;
            if (_currentFireArrowCooldown <= 0)
            {
                AudioManager.Instance.Play("EnemyArrow");
                _animator.SetTrigger("Fire");
                GameObject arrow = Instantiate(_arrowPrefab, _arrowPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().FireArrow(_player, _fireArrowForce);
                _currentFireArrowCooldown = _fireArrowCooldown;
            }
        }
    }

    public void DetectedPlayer(Transform player)
    {
        _hasDetectedPlayer = true;
        _player = player;
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
    }
}
