using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public class EnemyTriggers : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;

        private EnemyAI enemy;

        private void Start() => _parent.TryGetComponent(out enemy);
        private void EnableMovement() => enemy.SetAiActive(true);
        private void DisableMovement() => enemy.SetAiActive(false);

        public void Attack()
        {
            DisableMovement();
            InflictDamages();
        }

        private void InflictDamages()
        {
            // Inflict Damages
        }
    }
}