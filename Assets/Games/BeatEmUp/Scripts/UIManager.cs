using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    [RequireComponent(typeof(GameManager))]
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _lifeGaugeGameObject;
        [SerializeField] private Transform _lifeGaugeParent;
        [SerializeField] private Animator _uiAnimator;

        private List<GameObject> _lifeGaugeList;
        private HealthSystem _playerHealth;
        private GameManager _gameManager;
        private bool isPaused;
        private static readonly int Victoire = Animator.StringToHash("Victoire");
        private static readonly int Défaite = Animator.StringToHash("Défaite");
        private static readonly int Go = Animator.StringToHash("Go");
        
        public void GameOverScreen() => _uiAnimator.SetTrigger(Défaite);
        public void VictoireScreen() => _uiAnimator.SetTrigger(Victoire);
        public void GoScreen() => _uiAnimator.SetTrigger(Go);

        private void Start()
        {
            TryGetComponent(out _gameManager);
            _playerHealth = _gameManager.GetPlayerController().GetComponent<HealthSystem>();
            _lifeGaugeList = new List<GameObject>();
            
            _playerHealth.OnDamage += OnPlayerHealthChange;
            
            PlayerController.OnPausePressed += TogglePause;

            InitPlayerHealth();
        }

        private void InitPlayerHealth()
        {
            for (int i = 0; i < _playerHealth.GetMaxHp(); i++)
                _lifeGaugeList.Add(Instantiate(_lifeGaugeGameObject, _lifeGaugeParent));
        }

        private void OnPlayerHealthChange(int amount)
        {
            for (int i = 0; i < _playerHealth.GetMaxHp(); i++)
                _lifeGaugeList[i].SetActive(false);
            
            for (int i = 0; i < _playerHealth.GetCurHp(); i++)
                _lifeGaugeList[i].SetActive(true);
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
            Bootstrap.instance.LoadScene("Main_Menu", 0, "Raccoon_Menu");
        }
    }
}