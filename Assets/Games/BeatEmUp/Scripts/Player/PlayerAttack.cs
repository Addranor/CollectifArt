using UnityEngine;

namespace BeatEmUp
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                controller.AddEnemyInRange(other.GetComponent<EnemyPresence>().GetEnemyHealthSystem());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
                controller.RemEnemyInRange(other.GetComponent<EnemyPresence>().GetEnemyHealthSystem());
        }
    }
}