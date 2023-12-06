using System.Collections;
using System.Collections.Generic;
using BossBattle;
using UnityEngine;

namespace BossBattle
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        
        private PlayerController controller;

        private void Start() => _parent.TryGetComponent(out controller);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                controller.AddEnemyInRange(other.GetComponent<HealthSystem>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                controller.AddEnemyInRange(other.GetComponent<HealthSystem>());
        }
    }
}