using UnityEngine;

public class Bonfire : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        UIManager.Instance.ShowBonfireUI();
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.Bonfire;
    }
}
