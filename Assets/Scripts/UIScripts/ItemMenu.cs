using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _isGameStateFrozen;


    void Update()
    {
        if (_isGameStateFrozen)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                AudioManager.Instance.Play("HidePickup");
                _animator.SetTrigger("PopOut");
                _isGameStateFrozen = false;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.FreezeGameState();
        _isGameStateFrozen = true;
    }

    public void HideUIItemAnimationEvent()
    {
        UIManager.Instance.HideUIItem();
        GameManager.Instance.UnFreezeGameState();
    }
}
