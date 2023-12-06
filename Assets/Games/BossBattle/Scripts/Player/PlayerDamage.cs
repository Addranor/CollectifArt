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
        private BossDamage enemy;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                enemy = other.GetComponent<BossDamage>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && enemy != null)
                health.TakeDamage(enemy.GetDamages());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                enemy = null;
        }
    }
}