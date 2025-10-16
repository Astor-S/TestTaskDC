using System.Collections;
using UnityEngine;
using Characters.PlayerComponents;

namespace Characters.EnemiesComponents
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _attackInterval = 2f;

        private Coroutine _coroutine;
        private WaitForSeconds _wait;
        private bool _isCooldown = false;

        private void Awake()
        {
            _wait = new WaitForSeconds(_attackInterval);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
                if (_isCooldown == false)
                    _coroutine = StartCoroutine(AttackCooldown(player));
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player _))
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _isCooldown = false;
                }
            }
        }

        private IEnumerator AttackCooldown(Player target)
        {
            _isCooldown = true;

            while (_isCooldown)
            {
                target.TakeDamage(_damage);

                yield return _wait;
            }
        }
    }
}