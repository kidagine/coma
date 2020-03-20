using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : MonoBehaviour
{
    [SerializeField] private Slider _expSlider;
    [SerializeField] private Text _levelText;
    [SerializeField] private GameObject _levelUp;

    public void SetExp(int currentExp)
    {
        _expSlider.value = currentExp;
    }
    
    public void LevelUp(int expCap)
    {
        AudioManager.Instance.Play("LevelUp");
        _levelUp.SetActive(true);
        _levelText.text = "LV" + "2";
        _expSlider.maxValue = expCap;
    }
}
