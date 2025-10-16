using System;
using UnityEngine;
using Characters.CharacterComponents;

namespace Characters.EnemiesComponents
{
    public class Enemy : DamagableCharacter
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private float _moveSpeed = 2.0f;

        private Transform _playerTransform;

        public event Action<Enemy> Died;

        private void OnEnable()
        {
            _health.Died += () => Died?.Invoke(this);
        }

        private void OnDisable()
        {
            _health.Died -= () => Died?.Invoke(this);
        }

        private void Update()
        {
            MoveTowardsPlayer();
        }

        public void Initialize(Transform playerTransform) =>
            _playerTransform = playerTransform;

        private void MoveTowardsPlayer()
        {
            if (_playerTransform != null)
            {
                Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;
                _rigidbody.linearVelocity = directionToPlayer * _moveSpeed;
            }
        }
    }
}