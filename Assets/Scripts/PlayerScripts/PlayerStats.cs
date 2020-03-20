using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health { get; set; } = 3;
    public int Level { get; set; } = 1;
    public int CurrentExp { get; set; } = 0;
    public int ExpCap { get; set; } = 100;
}
