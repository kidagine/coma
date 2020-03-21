using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Animator _animator;
    [SerializeField] bool _notAnimated;

    public void OnEnterSelect()
    {
        if (!_notAnimated)
        {
            _animator.SetBool("IsSelected", true);
        }
        _text.color = Color.black;
    }

    public void OnExitSelect()
    {
        if (!_notAnimated)
        {
            _animator.SetBool("IsSelected", false);
        }
        _text.color = Color.white;
    }

    public void ButtonReset()
    {
        transform.position = new Vector2(transform.position.x - 60, transform.position.y);
        _animator.SetBool("IsSelected", false);
        _text.color = Color.white;
    }
}
