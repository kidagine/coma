using UnityEngine;

public class IntroMenu : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;


    public void EnablePlayerAnimationEvent()
    {
        _playerInput.enabled = true;
    }
}
