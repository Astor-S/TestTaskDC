using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.EnemiesComponents;

namespace Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy[] _enemyPrefabs;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Camera _mainCamera;

        [SerializeField] private float _spawnRadiusAroundPlayer;
        [SerializeField] private float _spawnBufferOutsideView;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private int _maxEnemiesOnScreen;

        private List<Enemy> _spawnedEnemies = new();

        public event Action<Enemy> OnEnemySpawned;

        private void Start()
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }

        public void RemoveEnemy(Enemy enemy)
        {
            if (_spawnedEnemies.Contains(enemy))
                _spawnedEnemies.Remove(enemy);
        }

        private IEnumerator SpawnEnemiesRoutine()
        {
            while (enabled)
            {
                yield return new WaitUntil(() => _spawnedEnemies.Count < _maxEnemiesOnScreen);
                yield return new WaitForSeconds(_spawnInterval);
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            if (CanSpawnEnemy() == false)
                return;

            Enemy randomEnemyPrefab = GetRandomEnemyPrefab();
            Vector3 spawnPosition = FindValidSpawnPosition();
            Enemy spawnedEnemy = CreateEnemyInstance(randomEnemyPrefab, spawnPosition);
            OnEnemySpawned?.Invoke(spawnedEnemy);
            _spawnedEnemies.Add(spawnedEnemy);
        }

        private bool CanSpawnEnemy()
        {
            return _enemyPrefabs != null && _enemyPrefabs.Length > 0 &&
                   _playerTransform != null && _mainCamera != null;
        }

        private Enemy GetRandomEnemyPrefab()
        {
            if (_enemyPrefabs.Length == 0)
                return null;

            return _enemyPrefabs[UnityEngine.Random.Range(0, _enemyPrefabs.Length)];
        }

        private Vector3 FindValidSpawnPosition()
        {
            Vector3 potentialSpawnPosition = Vector3.zero;
            bool foundValidSpawnPosition = false;
            int attempts = 0;
            int maxAttempts = 100;

            while (foundValidSpawnPosition == false && attempts < maxAttempts)
            {
                attempts++;

                Vector2 randomCircleDirection = UnityEngine.Random.insideUnitCircle;
                Vector3 directionFromPlayer = new(randomCircleDirection.x, 0, randomCircleDirection.y);

                float spawnDistance = UnityEngine.Random.Range(_spawnBufferOutsideView, _spawnRadiusAroundPlayer);
                potentialSpawnPosition = _playerTransform.position + directionFromPlayer * spawnDistance;

                if (IsPositionOutsideCameraView(potentialSpawnPosition))
                    foundValidSpawnPosition = true;
            }

            if (foundValidSpawnPosition == false)
            {
                Vector2 fallbackRandomDirection = UnityEngine.Random.insideUnitCircle;
                potentialSpawnPosition = _playerTransform.position + new Vector3(fallbackRandomDirection.x, 0, fallbackRandomDirection.y) * _spawnRadiusAroundPlayer;
            }

            return potentialSpawnPosition;
        }

        private Enemy CreateEnemyInstance(Enemy prefab, Vector3 position)
        {
            Enemy enemy = Instantiate(prefab, position, Quaternion.identity);
            enemy.Initialize(_playerTransform);

            return enemy;
        }

        private bool IsPositionOutsideCameraView(Vector3 worldPosition)
        {
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);

            if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
                screenPosition.y < 0 || screenPosition.y > Screen.height ||
                screenPosition.z < 0)
            {
                float distanceToPlayer = Vector3.Distance(worldPosition, _playerTransform.position);

                return distanceToPlayer > _spawnBufferOutsideView;
            }

            return false;
        }
    }
}