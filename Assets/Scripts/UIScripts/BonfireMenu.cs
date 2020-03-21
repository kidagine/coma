using UnityEngine;
using UnityEngine.UI;

public class BonfireMenu : MonoBehaviour
{
    [SerializeField] private GameObject _fadeToDarkCanvas;
    [SerializeField] private GameObject _bonfireMainMenu;
    [SerializeField] private GameObject _bonfireSpendLevelMenu;
    [SerializeField] private Button _firstIncrementLevelButton;
    [SerializeField] private Button _restButton;
    [SerializeField] private MenuButton _spendLevelMenuButton;
    [SerializeField] private Text _levelPointsCountText;
    [SerializeField] private Text _healthPointsCountText;
    [SerializeField] private Text _attackPointsCountText;
    [SerializeField] private Text _speedPointsCountText;


    public void CinematicFade()
    {
        AudioManager.Instance.Play("Click");
        _bonfireMainMenu.SetActive(false);
        _fadeToDarkCanvas.SetActive(true);
    }

    public void SpendLevel()
    {
        _spendLevelMenuButton.ButtonReset();
        AudioManager.Instance.Play("Click");
        _bonfireMainMenu.SetActive(false);
        _bonfireSpendLevelMenu.SetActive(true);
        _firstIncrementLevelButton.Select();
    }

    public void CancelSpendLevel()
    {
        AudioManager.Instance.Play("Click");
        _bonfireMainMenu.SetActive(true);
        _bonfireSpendLevelMenu.SetActive(false);
        _restButton.Select();
    }

    public void ConfirmSpendLevel()
    {
        AudioManager.Instance.Play("Click");
        _bonfireMainMenu.SetActive(true);
        _bonfireSpendLevelMenu.SetActive(false);
        _restButton.Select();
    }

    public void IncrementHealth()
    {
        AudioManager.Instance.Play("Click");
        _levelPointsCountText.text = "0";
        _healthPointsCountText.text = "4";
    }

    public void IncrementAttack()
    {
        AudioManager.Instance.Play("Click");
        _levelPointsCountText.text = "0";
        _attackPointsCountText.text = "2";

    }

    public void IncrementSpeed()
    {
        AudioManager.Instance.Play("Click");
        _levelPointsCountText.text = "0";
        _speedPointsCountText.text = "2";
    }

    public void Hover()
    {
        AudioManager.Instance.Play("Hover");
    }
}
