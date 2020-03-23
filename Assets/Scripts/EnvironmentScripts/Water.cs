using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeUntilActivate;
  
    void Start()
    {
        Invoke("EnableAnimator", _timeUntilActivate);    
    }

    private void EnableAnimator()
    {
        _animator.enabled = true;
    }
}
