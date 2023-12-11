using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            _uiManager.GoScreen();
            _confiner.m_BoundingShape2D = _defaultConfiner;
        }

        public void PauseGame(bool status)
        {
            _player.CanPlayerMove(!status);
            Time.timeScale = status ? 0 : 1;
        }
        
        public void RestartLevel1() => Bootstrap.instance.LoadScene("BeatEmUp_Level1");
        public void RestartLevel2() => Bootstrap.instance.LoadScene("BeatEmUp_Level2");
        public void RestartLevel3() => Bootstrap.instance.LoadScene("BeatEmUp_Level3");
        public void RestartLevel4() => Bootstrap.instance.LoadScene("BeatEmUp_Level4");
        
        public void ToLevel2() => Bootstrap.instance.LoadScene("BeatEmUp_Level2");
        public void ToLevel3() => Bootstrap.instance.LoadScene("BeatEmUp_Level3");
        public void ToLevel4() => Bootstrap.instance.LoadScene("BeatEmUp_Level4");

        public void RestartLevel1WithDelay() => StartCoroutine(RestartWithDelay("BeatEmUp_Level1"));
        public void RestartLevel2WithDelay() => StartCoroutine(RestartWithDelay("BeatEmUp_Level2"));
        public void RestartLevel3WithDelay() => StartCoroutine(RestartWithDelay("BeatEmUp_Level3"));
        public void RestartLevel4WithDelay() => StartCoroutine(RestartWithDelay("BeatEmUp_Level4"));
        private IEnumerator RestartWithDelay(string level)
        {
            // Wait
            yield return new WaitForSeconds(1);
            
            // Display Game Over Screen
            _uiManager.GameOverScreen();
            
            // Wait
            yield return new WaitForSeconds(5);
            
            // Back to Main Menu
            Bootstrap.instance.LoadScene(level);
            yield return null;
        }
        
        public void ToMainMenu() => Bootstrap.instance.LoadScene("Main_Menu");
        public void ToMainMenuVictory() => StartCoroutine(BackToMainMenuVictory());
        private IEnumerator BackToMainMenuVictory()
        {
            // Wait
            yield return new WaitForSeconds(1);
            
            // Display Victory Screen
            _uiManager.VictoireScreen();
            
            // Wait
            yield return new WaitForSeconds(5);
            
            // Back to Main Menu
            Bootstrap.instance.LoadScene("Main_Menu");
            yield return null;
        }
    }
}