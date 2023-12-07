using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace BeatEmUp
{
    public class GameManager : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField] private CinemachineConfiner _confiner;
        [SerializeField] private PolygonCollider2D _defaultConfiner;
        
        private PlayerController _player;
        private UIManager _uiManager;
        public PlayerController GetPlayerController() => _player;

        private void Start()
        {
            _player = FindAnyObjectByType<PlayerController>();

            TryGetComponent(out _uiManager);
            
            EnemySpawner.OnSpawnerStart += OnSpawnerStart;
            EnemySpawner.OnSpawnerClean += OnSpawnerClean;
        }

        private void OnDisable()
        {
            EnemySpawner.OnSpawnerStart -= OnSpawnerStart;
            EnemySpawner.OnSpawnerClean -= OnSpawnerClean;
        }

        private void OnSpawnerStart(PolygonCollider2D confiner)
        {
            if (confiner == null) return;
            _confiner.m_BoundingShape2D = confiner;
        }
        
        private void OnSpawnerClean(PolygonCollider2D confiner)
        {
            _confiner.m_BoundingShape2D = _defaultConfiner;
        }

        public void ToMainMenu()
        {
            StartCoroutine(BackToMainMenu());
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

        public void PauseGame(bool status)
        {
            _player.CanPlayerMove(!status);
            Time.timeScale = status ? 0 : 1;
        }
    }
}