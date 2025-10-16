using System;
using UnityEngine;

namespace Characters.PlayerComponents
{
    public class PlayerInput : MonoBehaviour
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string MouseScrollWheelAxis = "Mouse ScrollWheel";

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private int _shootMouseButton = 0;
        [SerializeField] private float _minAimDistance = 2f;

        private bool _isShooting = false;

        public event Action<float, float> Moving;
        public event Action<Vector3> PlayerRotationDirectionChanged;

        public event Action ShootPerformed;
        public event Action ShootCancelled;

        public event Action SwitchWeaponNext;
        public event Action SwitchWeaponPrevious;

        private void Update()
        {
            HandleMovementInput();
            HandleAimingInput();
            HandleShootingInput();
            HandleWeaponSwitchingInput();
        }

        public bool IsShooting() =>
            _isShooting;

        private void HandleMovementInput()
        {
            float horizontalInput = Input.GetAxis(HorizontalAxis);
            float verticalInput = Input.GetAxis(VerticalAxis);

            if (horizontalInput != 0 || verticalInput != 0)
                Moving?.Invoke(horizontalInput, verticalInput);
        }

        private void HandleAimingInput()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new(Vector3.up, Vector3.zero);
            Vector3 aimPoint;

            if (groundPlane.Raycast(ray, out float rayDistance))
                aimPoint = ray.GetPoint(rayDistance);
            else
                aimPoint = ray.GetPoint(_mainCamera.farClipPlane);

            float distanceToAimPoint = Vector3.Distance(aimPoint, _playerTransform.position);

            Vector3 playerRotationDirection;

            if (distanceToAimPoint < _minAimDistance)
            {
                playerRotationDirection = _playerTransform.forward;
            }
            else
            {
                Vector3 directionToAim = (aimPoint - _playerTransform.position);
                directionToAim.y = 0;
                playerRotationDirection = directionToAim.normalized;
            }

            PlayerRotationDirectionChanged?.Invoke(playerRotationDirection);
        }

        private void HandleShootingInput()
        {
            if (Input.GetMouseButtonDown(_shootMouseButton))
            {
                _isShooting = true;
                ShootPerformed?.Invoke();
            }
            else if (Input.GetMouseButtonUp(_shootMouseButton))
            {
                _isShooting = false;
                ShootCancelled?.Invoke();
            }
        }

        private void HandleWeaponSwitchingInput()
        {
            float scrollInput = Input.GetAxis(MouseScrollWheelAxis);

            if (scrollInput > 0f)
                SwitchWeaponNext?.Invoke();
            else if (scrollInput < 0f)
                SwitchWeaponPrevious?.Invoke();
        }
    }
}