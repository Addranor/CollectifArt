using System;
using System.Collections;
using System.Collections.Generic;
using BossBattle;
using UnityEngine;

namespace BossBattle
{
    public class PlayerDamage : MonoBehaviour
    {
        [SerializeField] private HealthSystem health;
        private DamageSystem enemy;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                enemy = other.GetComponent<DamageSystem>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && enemy != null)
                health.TakeDamage(enemy.GetDamageDealt());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                enemy = null;
        }
    }
}