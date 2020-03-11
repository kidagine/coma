using UnityEngine;

public class WeaponPickable : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _itemExplosionPrefab;
    private readonly PickableType _pickableType = PickableType.Weapon;


    void Awake()
    {
        if (GlobalSettings.hasPickedBrokenDagger)
        {
            Destroy(gameObject);
        }
    }

    public void Picked()
    {
        Instantiate(_itemExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Throw()
    {

    }

    public PickableType GetPickableType()
    {
        return _pickableType;
    }
}
