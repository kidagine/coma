using UnityEngine;

public class Pickable : MonoBehaviour
{
    private bool _isPlayerClose;


    void Update()
    {
        if (_isPlayerClose)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                UIManager.Instance.HideUIPrompt();
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            _isPlayerClose = true;
            UIManager.Instance.ShowUIPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            _isPlayerClose = false;
            UIManager.Instance.HideUIPrompt();
        }
    }
}
