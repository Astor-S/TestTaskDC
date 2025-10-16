using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float fireRate;

        protected float lastFireTime = 0f;

        public abstract void Fire(Vector3 aimDirection);

        protected bool CanFire() =>
            Time.time >= lastFireTime + fireRate;
    }
}