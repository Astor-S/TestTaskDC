using UnityEngine;

namespace Characters.CharacterComponents
{
    public class DamagableCharacter : MonoBehaviour
    {
        [SerializeField] protected Health _health;

        public void TakeDamage(float damage) =>
            _health.TakeDamage(damage);
    }
}