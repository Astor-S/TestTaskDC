using UnityEngine;

namespace CameraComponents
{
    public class PlayerTracker : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Vector3 _offset = new(0, 0, 0);

        [Header("Follow Settings")]
        [SerializeField] private float _followSpeed = 5.0f;
        [SerializeField] private bool _smoothFollow = true;

        private void LateUpdate()
        {
            if(_playerTransform != null)
            {
                Vector3 desiredPosition = _playerTransform.position + _offset;

                if (_smoothFollow)
                    transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
                else
                    transform.position = desiredPosition;
            }
        }
    }
}