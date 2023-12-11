using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BeatEmUp
{
    [CreateAssetMenu(menuName = "SpriteSave")]
    public class SaveGameSO : ScriptableObject
    {
        [Header("Sauvegardes")]
        [SerializeField] private bool _raccoonFinished;
        [SerializeField] private bool _raiponceFinished;
        
        [Header("Sauvegarde Raton")]
        [SerializeField] private bool _cape;
        [SerializeField] private bool _weapon;
        [SerializeField] private bool _shield;
        [SerializeField] private bool _banana;

        public void SetCape(bool status) => _cape = status;
        public void SetWeapon(bool status) => _weapon = status;
        public void SetShield(bool status) => _shield = status;
        public void SetBanana(bool status) => _banana = status;

        public bool GetCape() => _cape;
        public bool GetWeapon() => _weapon;
        public bool GetShield() => _shield;
        public bool GetBanana() => _banana;

        public void FinishRaccoonGame() => _raccoonFinished = true;
        public void FinishRaiponceGame() => _raiponceFinished = true;

        public void Reset()
        {
            SetCape(false);
            SetWeapon(false);
            SetShield(false);
            SetBanana(false);
        }

        public void ResetGame()
        {
            _raiponceFinished = false;
            _raccoonFinished = false;
            Reset();
        }

        public bool IsGameFinished()
        {
            return _raccoonFinished && _raiponceFinished;
        }
    }
}