using UnityEngine;

namespace Characters.PlayerComponents
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _rotationSpeed = 5f;

        private Vector3 _rotationDirection = Vector3.zero;

        private void Awake()
        {
            if( _playerInput != null )
            _playerInput.PlayerRotationDirectionChanged += HandleRotationInput;
        }

        private void Update()
        {
            Rotate();
        }

        private void OnDestroy()
        {
            if (_playerInput != null)
                _playerInput.PlayerRotationDirectionChanged -= HandleRotationInput;
        }

        private void Rotate()
        {
            if (_rotationDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_rotationDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            }
        }

        private void HandleRotationInput(Vector3 rotationDirection) =>
            _rotationDirection = rotationDirection;
    }
}