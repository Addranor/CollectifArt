using System;
using System.Collections;
using System.Collections.Generic;
using BossBattle;
using UnityEngine;

namespace BossBattle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static bool isPlayerDead = false;
        public static bool isBossDead = false;
        public static bool canHurtBoss = false;

        [Header("References")] [SerializeField]
        private GameObject _playerGameObject;

        [SerializeField] private GameObject _bossGameObject;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _bossSpawnPoint;

        private UIManager _uiManager;
        private PlayerController _player;
        private BossAI _boss;
        
        private HealthSystem _playerHealth;
        private HealthSystem _bossHealth;

        public HealthSystem GetPlayerHealthSystem() => _playerHealth;
        public HealthSystem GetBossHealthSystem() => _bossHealth;

        private void Start()
        {
            TryGetComponent(out _uiManager);
            
            _player = FindAnyObjectByType<PlayerController>();
            _boss = FindAnyObjectByType<BossAI>();

            _player.transform.position = _playerSpawnPoint.position;
            _boss.transform.position = _bossSpawnPoint.position;
            
            _playerHealth = _player.GetComponent<HealthSystem>();
            _bossHealth = _boss.GetComponent<HealthSystem>();
            
            _playerHealth.OnDeath += OnPlayerDeath;
            _bossHealth.OnDeath += OnBossDeath;

            // Cutscene ?
            StartCoroutine(StartEncounter());
        }

        private void OnDisable()
        {
            _playerHealth.OnDeath -= OnPlayerDeath;
            _bossHealth.OnDeath -= OnBossDeath;
        }

        private IEnumerator StartEncounter()
        {
            // TODO: Cutscene ?
            
            _player.SetControlsActive(true);
            _boss.SetAIActive(true);
            yield return null;
        }

        public void PauseGame(bool status)
        {
            _player.SetControlsActive(!status);
            _boss.SetAIActive(!status);
            Time.timeScale = status ? 0 : 1;
        }
        
        private void OnBossDeath()
        {
            StartCoroutine(BackToMainMenu());
        }

        private void OnPlayerDeath()
        {
            _player.SetControlsActive(false);
            _boss.SetAIActive(false);
            StartCoroutine(RestartOnCheckpoint());
        }
        
        private IEnumerator BackToMainMenu()
        {
            // Display Victory Screen
            // DisplayScreen
            
            // Wait
            yield return new WaitForSeconds(1);
            
            // Back to Main Menu
            Bootstrap.instance.LoadScene("Main_Menu");
            yield return null;
        }

        private IEnumerator RestartOnCheckpoint()
        {
            Bootstrap.instance.LoadLocal(true);
            
            yield return new WaitForSeconds(1);

            // Reset Player
            _player.transform.position = _playerSpawnPoint.position;
            _playerHealth.FillHealth();

            // Reset Boss
            _boss.transform.position = _bossSpawnPoint.position;
            _bossHealth.FillHealth(_bossHealth.GetPerHp(_boss.GetCurPhasePercentage()));

            Bootstrap.instance.LoadLocal(false);

            yield return new WaitForSeconds(2);

            _player.SetControlsActive(true);
            _boss.SetAIActive(true);
            //yield return null;
        }
    }
}