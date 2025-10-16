using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Characters.CharacterComponents;

namespace UI.Bars
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _fillingSpeed;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            _health.ValueChanged += UpdateHealth;
        }

        private void OnDisable()
        {
            _health.ValueChanged -= UpdateHealth;

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private void UpdateHealth(float health, float maxHealth)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (health == 0)
            {
                _slider.fillRect.gameObject.SetActive(false);
            }
            else
            {
                _slider.fillRect.gameObject.SetActive(true);
                _coroutine = StartCoroutine(SmoothFill(health, maxHealth));
            }
        }

        private IEnumerator SmoothFill(float health, float maxHealth)
        {
            float value = health / maxHealth;

            while (_slider.value != value)
            {
                _slider.value = Mathf.MoveTowards(_slider.value, value, _fillingSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}