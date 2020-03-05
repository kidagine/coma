using UnityEngine;

public class LogoSplash : MonoBehaviour
{
    [SerializeField] private SceneName _sceneName;


    public void NextSceneAnimationEvent()
    {
        GameManager.Instance.LoadScene(_sceneName);
    }
}
