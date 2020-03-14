using UnityEngine;

public enum PickableType { Weapon, Key }

public interface IPickable
{
    void Picked();
    void Throw(Vector2 throwDirection);
    PickableType GetPickableType();
}
