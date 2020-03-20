using UnityEngine;

public class WeaponPickable : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _itemExplosionPrefab;


    void Awake()
    {
        if (GlobalSettings.hasPickedBrokenDagger)
        {
            Destroy(gameObject);
        }
    }

    public void Picked(GameObject player)
    {
        Instantiate(_itemExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Throw(Vector2 throwDirection)
    {

    }

    public PickableType GetPickableType()
    {
        return PickableType.Weapon;
    }
}
