using UnityEngine;

public class IntroMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerInput _playerInput;


    void Start()
    {
        if (!GlobalSettings.isIntroOpeningPlayed)
        {
            _animator.SetTrigger("FadeOut");
            GlobalSettings.isIntroOpeningPlayed = true;
        }
    }

    public void EnablePlayerAnimationEvent()
    {
        _playerInput.enabled = true;
    }
}
