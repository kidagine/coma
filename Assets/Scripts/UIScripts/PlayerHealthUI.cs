using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image[] _heartsImage;
    [SerializeField] private Sprite _heartSprite;
    [SerializeField] private Sprite _blackSprite;


    public void SetHearts (int currentHearts)
    {
        for (int i = 0; i < _heartsImage.Length; i++)
        {
            _heartsImage[i].sprite = _blackSprite;
        }

        for (int i = 0; i < currentHearts; i++)
        {
            int heartIndex = i;
            _heartsImage[heartIndex].sprite = _heartSprite;
        }
    }
}
