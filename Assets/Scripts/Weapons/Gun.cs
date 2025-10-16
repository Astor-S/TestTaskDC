using UnityEngine;
using Bullets;

namespace Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletSpawnOffset = 2f;

        public override void Fire(Vector3 aimDirection)
        {
            if (CanFire() == false)
                return;

            lastFireTime = Time.time;

            Vector3 bulletDirection = aimDirection.normalized;
            Vector3 firePosition = transform.position + transform.forward * _bulletSpawnOffset;

            Bullet bullet = Instantiate(_bulletPrefab, firePosition, Quaternion.LookRotation(bulletDirection));
            bullet.Initialize(bulletDirection);
        }
    }
}