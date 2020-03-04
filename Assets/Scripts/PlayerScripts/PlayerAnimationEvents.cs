using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void PlaySoundAnimationEvent(AnimationEvent animationEvent)
    {
        AudioManager.Instance.Play(animationEvent.stringParameter);
    }

}
