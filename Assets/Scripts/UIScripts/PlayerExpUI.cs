using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : MonoBehaviour
{
    [SerializeField] private Slider _expSlider;

    public void SetExp(int currentExp)
    {
        _expSlider.value = currentExp;
    }
    
    public void SetExpCap(int expCap)
    {
        _expSlider.maxValue = expCap;
    }
}
