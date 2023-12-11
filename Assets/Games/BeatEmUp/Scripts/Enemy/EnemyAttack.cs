using UnityEngine;

namespace BeatEmUp
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAI enemyAI;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            enemyAI.IsEnemyInRange(true);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            enemyAI.IsEnemyInRange(true);
        }
    }
}