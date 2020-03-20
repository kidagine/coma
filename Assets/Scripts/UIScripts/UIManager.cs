using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject _promptCanvas;
	[SerializeField] private GameObject _itemCanvas;
	[SerializeField] private Text _coinsText;
	[SerializeField] private Text _promptText;
	private int coinsAmount;

	public static UIManager Instance { get; private set; }


	void Awake()
	{
		CheckInstance();
	}

	private void CheckInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void ShowUIPrompt(Transform target, string text)
	{
		_promptText.text = text;
		_promptCanvas.SetActive(true);
		Vector3 promptPosition = Camera.main.WorldToScreenPoint(new Vector2(target.transform.position.x, target.transform.position.y + 1.5f));
		Transform prompt = _promptCanvas.transform.GetChild(0);
		prompt.transform.position = promptPosition;
	}

	public void HideUIPrompt()
	{
		_promptCanvas.SetActive(false);
	}

	public void ShowUIItem()
	{
		_itemCanvas.SetActive(true);
	}

	public void HideUIItem()
	{
		_itemCanvas.SetActive(false);
	}

	public void IncrementCoins()
	{
		coinsAmount++;
		_coinsText.text = coinsAmount.ToString();
	}
}
