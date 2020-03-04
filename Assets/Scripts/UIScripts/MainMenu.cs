using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    public void StartGame()
    {
        AudioManager.Instance.Play("Click");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
