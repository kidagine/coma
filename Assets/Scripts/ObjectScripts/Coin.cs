using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            UIManager.Instance.IncrementCoins();
            AudioManager.Instance.Play("Coin");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Pit"))
        {
            Destroy(gameObject);
        }
    }
}
