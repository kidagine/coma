using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Animator _animator;


    public void OnEnterSelect()
    {
        _animator.SetBool("IsSelected", true);
        _text.color = Color.black;
    }

    public void OnExitSelect()
    {
        _animator.SetBool("IsSelected", false);
        _text.color = Color.white;
    }
}
