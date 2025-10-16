using System.Collections;
using UnityEngine;
using Bullets;

namespace Weapons
{
    public class BurstFireGun : Weapon
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletSpawnOffset = 2f;
        [SerializeField] private int _shotsInBurst = 3;
        [SerializeField] private float _timeBetweenShotsInBurst = 0.1f;

        private bool _isFiringBurst = false;
        private float _lastShotTimeInBurst = 0f;
        private int _shotsFiredInBurst = 0;

        public override void Fire(Vector3 aimDirection)
        {
            if (_isFiringBurst)
                return;

            if (CanFire() == false)
                return;

            _isFiringBurst = true;
            _shotsFiredInBurst = 0;
            _lastShotTimeInBurst = Time.time;

            lastFireTime = Time.time;

            StartCoroutine(FireBurstSequence(aimDirection));
        }

        private IEnumerator FireBurstSequence(Vector3 aimDirection)
        {
            while (_shotsFiredInBurst < _shotsInBurst)
            {
                if (Time.time >= _lastShotTimeInBurst + _timeBetweenShotsInBurst)
                {
                    ShootSingleBullet(aimDirection);
                    _lastShotTimeInBurst = Time.time;
                    _shotsFiredInBurst++;
                }

                yield return null;
            }

            _isFiringBurst = false;
        }

        private void ShootSingleBullet(Vector3 aimDirection)
        {
            Vector3 bulletDirection = aimDirection.normalized;
            Vector3 firePosition = transform.position + transform.forward * _bulletSpawnOffset;

            Bullet bullet = Instantiate(_bulletPrefab, firePosition, Quaternion.LookRotation(bulletDirection));
            bullet.Initialize(bulletDirection);
        }
    }
}