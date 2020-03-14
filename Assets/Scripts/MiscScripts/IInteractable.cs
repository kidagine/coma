public enum InteractableType { Door }

public interface IInteractable
{
    void Interact();
    InteractableType GetInteractableType();
}
