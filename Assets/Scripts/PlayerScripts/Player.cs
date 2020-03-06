using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _weaponObject;

    void Awake()
    {
        if (GlobalSettings.hasWeaponEquiped)
        {
            _weaponObject.SetActive(true);
        }
    }

    public void PickUp()
    {
        GlobalSettings.hasWeaponEquiped = true;
        _weaponObject.SetActive(true);
    }
}
