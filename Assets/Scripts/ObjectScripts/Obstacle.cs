using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleExplosionPrefab;
    [SerializeField] private bool _hasItem;


    public void Destroy()
    {
        AudioManager.Instance.Play("ObstacleBreak");
        Instantiate(_obstacleExplosionPrefab, transform.position, Quaternion.identity);
        if (_hasItem)
        {
            Transform item = transform.GetChild(0);
            item.SetParent(null);
            item.gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
}
