using UnityEngine;

public enum PickableType { Weapon, Key }

public interface IPickable
{
    void Picked(GameObject player);
    void Throw(Vector2 throwDirection);
    PickableType GetPickableType();
}
