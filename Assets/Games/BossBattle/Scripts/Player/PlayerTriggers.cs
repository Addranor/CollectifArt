using System;
using System.Collections;
using System.Collections.Generic;
using BossBattle;
using UnityEngine;

namespace BossBattle
{
    public class PlayerTriggers : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        
        private PlayerController controller;
        private HealthSystem health;

        private List<HealthSystem> enemies;

        private void Start()
        {
            _parent.TryGetComponent(out controller);
            _parent.TryGetComponent(out health);
        }

        public void AttackTrigger()
        {
            if (health.IsEntityDead()) return;
            
            enemies = controller.GetEnemiesInRange();
            foreach (HealthSystem enemy in enemies)
                enemy.TakeDamage(controller.GetDamages());
        }
    }
}