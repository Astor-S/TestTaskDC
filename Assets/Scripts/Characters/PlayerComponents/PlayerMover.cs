using UnityEngine;

namespace Characters.PlayerComponents
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _moveSpeed = 5f;

        private void Awake()
        {
            _playerInput.Moving += Move;
        }

        private void OnDestroy()
        {
            _playerInput.Moving -= Move;
        }

        private void Move(float horizontalInput, float verticalInput)
        {
            Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
            _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);
        }
    }
}