using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BossBattle
{
    [RequireComponent(typeof(GameManager))]
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pauseMenu;
        
        [Header("Boss")]
        [SerializeField] private Slider _slider;
        
        [Header("Player")]
        [SerializeField] private GameObject _heartsGameObject;
        [SerializeField] private GameObject _heartsBackGameObject;
        [SerializeField] private Transform heartsParent;
        [SerializeField] private Transform heartsBackParent;
        
        private GameManager _gameManager;
        private bool isPausePressed;
        private bool isPaused;
        private int heartIndex;

        private HealthSystem _player;
        private HealthSystem _boss;

        private List<GameObject> hearts;

        private void Start()
        {
            TryGetComponent(out _gameManager);
            
            _player = _gameManager.GetPlayerHealthSystem();
            _boss = _gameManager.GetBossHealthSystem();
            
            PlayerController.PausePressed += OnPausePressed;
            
            _gameManager.GetPlayerHealthSystem().OnDamage += OnPlayerHealthChange;
            _gameManager.GetBossHealthSystem().OnDamage += OnBossHealthChange;
            
            _gameManager.GetPlayerHealthSystem().OnHeal += OnPlayerHealthChange;
            _gameManager.GetBossHealthSystem().OnHeal += OnBossHealthChange;
            
            _gameManager.GetPlayerHealthSystem().OnDeath += OnPlayerDeath;
            _gameManager.GetBossHealthSystem().OnDeath += OnBossDeath;
            
            _slider.maxValue = _boss.GetMaxHp();
            _slider.value = _boss.GetMaxHp();
            
            GeneratePlayerHealthDisplay(_player.GetMaxHp());
        }
        
        private void OnDisable()
        {
            // Disable Subs
            PlayerController.PausePressed -= OnPausePressed;
            
            _gameManager.GetPlayerHealthSystem().OnDamage -= OnPlayerHealthChange;
            _gameManager.GetBossHealthSystem().OnDamage -= OnBossHealthChange;
            
            _gameManager.GetPlayerHealthSystem().OnHeal -= OnPlayerHealthChange;
            _gameManager.GetBossHealthSystem().OnHeal -= OnBossHealthChange;
            
            _gameManager.GetPlayerHealthSystem().OnDeath -= OnPlayerDeath;
            _gameManager.GetBossHealthSystem().OnDeath -= OnBossDeath;
        }

        #region Delegates

        private void OnPausePressed()
        {
            TogglePause();
        }
        
        private void OnPlayerHealthChange(int currentHp, int amount)
        {
            // Update Player Healthbar
            UpdatePlayerHealthDisplay(amount);
        }
        
        private void OnBossHealthChange(int currentHp, int amount)
        {
            // Update Enemy Health Bar
            _slider.value = currentHp;
        }

        private void OnPlayerDeath()
        {
            // Display Gamee Over screen
        }

        private void OnBossDeath()
        {
            // Display Victory screen
        }
        
        #endregion

        private void GeneratePlayerHealthDisplay(int amount)
        {
            hearts = new List<GameObject>();
            
            for (int i = 0; i < amount; i++)
                hearts.Add(Instantiate(_heartsGameObject, heartsParent));
            
            for (int i = 0; i < amount; i++)
                Instantiate(_heartsBackGameObject, heartsBackParent);
        }

        private void UpdatePlayerHealthDisplay(int amount = 0)
        {
            if (amount == 0)
            {
                heartIndex = 0;
                foreach (GameObject heart in hearts)
                    heart.SetActive(true);
            }
            else
                for (int i = 0; i < amount; i++)
                    hearts[heartIndex++].SetActive(false);
        }

        public void TogglePause()
        {
            _gameManager.PauseGame(!isPaused);
            _pauseMenu.SetActive(!isPaused);
            
            isPaused = !isPaused;
        }

        public void BackToMenu()
        {
            Bootstrap.instance.LoadScene("MainMenu");
        }
    }
}