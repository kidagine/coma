using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;
	[SerializeField] private SceneName _sceneName;


	public void StartGame()
    {
        AudioManager.Instance.Play("Click");
        AudioManager.Instance.FadeOut("EarlyBus");
        _animator.SetTrigger("FadeIn");
    }

    public void ExitGame()
    {
        AudioManager.Instance.Play("Click");
        Application.Quit();
    }

    public void Hover()
    {
		AudioManager.Instance.Play("Hover");
    }

    public void NextSceneAnimationEvent()
    {
		GameManager.Instance.LoadScene(_sceneName);
    }
}
