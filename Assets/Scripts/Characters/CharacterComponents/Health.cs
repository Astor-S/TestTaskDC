using System;
using UnityEngine;

namespace Characters.CharacterComponents
{
    public class Health : MonoBehaviour
    {
        private readonly float _minValue = 0;

        [SerializeField] private float _maxValue;

        private float _currentValue;

        public event Action<float, float> ValueChanged;
        public event Action Died;

        private void OnEnable()
        {
            _currentValue = _maxValue;
        }

        public void TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            float newHealth = Mathf.Max(_currentValue - damage, _minValue);
            UpdateValue(newHealth);

            if (_currentValue <= _minValue)
                ApplyDeath();
        }

        private void UpdateValue(float value)
        {
            _currentValue = value;
            ValueChanged?.Invoke(value, _maxValue);
        }

        private void ApplyDeath()
        {
            Died?.Invoke();
            Destroy(gameObject);
        }
    }
}