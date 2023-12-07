using UnityEngine;

namespace BeatEmUp
{
    public class PlayerTriggers : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;

        private PlayerController controller;

        private void Start() => _parent.TryGetComponent(out controller);
        private void EnableMovement() => controller.CanPlayerMove(true);
        private void DisableMovement() => controller.CanPlayerMove(false);

        public void Attack()
        {
            DisableMovement();
            InflictDamages();
        }
        
        private void InflictDamages()
        {
            foreach (HealthSystem enemy in controller.GetEnemiesInRange())
                enemy.TakeDamage(controller.GetDamageDealt());
        }
    }
}