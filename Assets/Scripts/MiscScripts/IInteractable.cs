public enum InteractableType { Door, Bonfire }

public interface IInteractable
{
    void Interact();
    InteractableType GetInteractableType();
}
