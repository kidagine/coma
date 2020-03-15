using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleExplosionPrefab;


    public void Destroy()
    {
        AudioManager.Instance.Play("ObstacleBreak");
        Instantiate(_obstacleExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
