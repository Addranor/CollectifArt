using System.Diagnostics.Contracts;
using UnityEngine;

namespace BeatEmUp
{
    public class PlayerTriggers : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private AudioSource _audioSource;

        private PlayerController controller;

        private void Start() => _parent.TryGetComponent(out controller);
        private void EnableMovement() => controller.CanPlayerMove(true);
        private void DisableMovement() => controller.CanPlayerMove(false);

        public void Attack()
        {
            DisableMovement();
            InflictDamages();
        }

        public void PickupItem()
        {
            controller.Anim_PickupItem();
            controller.DisablePickable();
        }
        
        private void InflictDamages()
        {
            var enemies = controller.GetEnemiesInRange();
            _audioSource.PlayOneShot(enemies.Count == 0 ? controller.GetMissedSfx() : controller.GetLandedSfx());
            foreach (HealthSystem enemy in enemies)
                enemy.TakeDamage(controller.GetDamageDealt());
        }
    }
}