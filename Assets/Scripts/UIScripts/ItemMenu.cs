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
                _isGameStateFrozen = false;
                _animator.SetTrigger("PopOut");
            }
        }
    }

    private void OnEnable()
    {
        _isGameStateFrozen = true;
        GameManager.Instance.FreezeGameState();
    }

    public void HideUIItemAnimationEvent()
    {
        UIManager.Instance.HideUIItem();
        GameManager.Instance.UnFreezeGameState();
    }
}
