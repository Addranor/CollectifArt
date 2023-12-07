using System;
using UnityEngine;

namespace BeatEmUp
{
    [RequireComponent(typeof(GameManager))]
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pauseMenu;

        private HealthSystem _playerHealth;
        private GameManager _gameManager;
        private bool isPaused;

        private void Start()
        {
            TryGetComponent(out _gameManager);
            _playerHealth = _gameManager.GetPlayerController().GetComponent<HealthSystem>();
            
            PlayerController.OnPausePressed += TogglePause;
        }

        private void OnDisable()
        {
            PlayerController.OnPausePressed -= TogglePause;
        }

        public void TogglePause()
        {
            _gameManager.PauseGame(!isPaused);
            _pauseMenu.SetActive(!isPaused);

            isPaused = !isPaused;
        }
        
        public void BackToMenu()
        {
            TogglePause();
            Bootstrap.instance.LoadScene("Main_Menu");
        }
    }
}