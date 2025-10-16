using UnityEngine;
using Characters.EnemiesComponents;
using Spawners;

namespace GameHandlers
{
    public class EnemyDeathHandler : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private void Awake()
        {
            _enemySpawner.OnEnemySpawned += EnemySpawnedHandler;
        }

        private void EnemySpawnedHandler(Enemy newEnemy) =>
            newEnemy.Died += HandleEnemyDeath;

        private void HandleEnemyDeath(Enemy deadEnemy)
        {
            deadEnemy.Died -= HandleEnemyDeath;

            if (_enemySpawner != null)
                _enemySpawner.RemoveEnemy(deadEnemy);
        }

        private void OnDestroy()
        {
            if (_enemySpawner != null)
                _enemySpawner.OnEnemySpawned -= EnemySpawnedHandler;
        }
    }
}