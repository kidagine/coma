using UnityEngine;

public class EnemyEventNotifier : MonoBehaviour
{
    [SerializeField] private SpawnEnemiesTrigger _spawnEnemiesTrigger;


    private void OnDestroy()
    {
        _spawnEnemiesTrigger.DecreaseEnemies();
    }
}
