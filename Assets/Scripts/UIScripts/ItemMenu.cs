using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _canClose;


    void Update()
    {
        if (GameManager.Instance.IsGameStateFrozen)
        {
            if (Input.GetKeyDown(KeyCode.X) && _canClose)
            {
                AudioManager.Instance.Play("HidePickup");
                _animator.SetTrigger("PopOut");
            }
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.FreezeGameState();
    }

    public void CanCloseUIItemAnimationEvent()
    {
        _canClose = true;
    }

    public void HideUIItemAnimationEvent()
    {
        UIManager.Instance.HideUIItem();
        GameManager.Instance.UnFreezeGameState();
    }
}