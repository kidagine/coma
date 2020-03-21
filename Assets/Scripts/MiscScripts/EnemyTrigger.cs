using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private Archer[] _archers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            foreach (Archer archer in _archers)
            {
                archer.DetectedPlayer(collision.transform);
            }
        }
    }
}
