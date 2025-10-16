using UnityEngine;
using UnityEngine.SceneManagement;
using Characters.CharacterComponents;

namespace GameHandlers
{
    public class GameOverHandler : MonoBehaviour
    {
        [SerializeField] private Health _playerHealth;

        private void Awake()
        {
            _playerHealth.Died += GameOver;
        }

        private void OnDestroy()
        {
            _playerHealth.Died -= GameOver;
        }

        private void GameOver() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}