using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Animator _activatorAnimator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openSwitchSprite;
    [SerializeField] private Sprite _closedSwitchSprite;
    [SerializeField] private bool _isClosed;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isClosed)
        {
            _activatorAnimator.SetTrigger("ShowUp");
            _spriteRenderer.sprite = _openSwitchSprite;
        }
    }
}
