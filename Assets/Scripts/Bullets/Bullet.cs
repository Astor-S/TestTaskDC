using UnityEngine;
using Characters.EnemiesComponents;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private float _damage = 1f;
        
        private Vector3 _movementDirection;

        public void Initialize(Vector3 direction)
        {
            _movementDirection = direction.normalized;
            _rigidbody.linearVelocity = _movementDirection * _speed;

            Destroy(gameObject, _lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}