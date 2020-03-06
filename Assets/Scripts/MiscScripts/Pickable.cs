using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private GameObject _itemExplosionPrefab;
    private Player _player;


    void Update()
    {
        if (_player != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                UIManager.Instance.HideUIPrompt();
                Instantiate(_itemExplosionPrefab, transform.position, Quaternion.identity);
                _player.PickUp();
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<Player>();
        if (_player != null)
        {
            UIManager.Instance.ShowUIPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _player = collision.GetComponent<Player>();
        if (_player != null)
        {
            UIManager.Instance.HideUIPrompt();
        }
    }
}
