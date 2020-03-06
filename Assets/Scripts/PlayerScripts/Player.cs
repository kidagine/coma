using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _weaponObject;


    public void PickUp()
    {
        _weaponObject.SetActive(true);
    }
}
