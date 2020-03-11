public enum PickableType { Weapon, Key }

public interface IPickable
{
    void Picked();
    void Throw();
    PickableType GetPickableType();
}
