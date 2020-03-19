using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Animator _activatorAnimator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openSwitchSprite;
    [SerializeField] private Sprite _closedSwitchSprite;
    [SerializeField] private bool _isClosed;


    void Start()
    {
        if (!_isClosed)
        {
            OpenSwitch(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isClosed)
        {
            OpenSwitch(true);
        }
    }

    public void OpenSwitch(bool isSwitchOpen)
    {
        _activatorAnimator.SetBool("IsShowingUp", isSwitchOpen);
        if (isSwitchOpen)
        {
            AudioManager.Instance.Play("BridgeOpen");
            _spriteRenderer.sprite = _openSwitchSprite;
        }
        else
        {
            _spriteRenderer.sprite = _closedSwitchSprite;
        }
        _isClosed = !isSwitchOpen;
    }
}
