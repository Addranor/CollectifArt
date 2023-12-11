using System.Collections;
using System.Collections.Generic;
using BeatEmUp;
using UnityEngine;

namespace BeatEmUp
{
    public class EnemyPresence : MonoBehaviour
    {
        [SerializeField] private HealthSystem _enemyHealth;
        public HealthSystem GetEnemyHealthSystem() => _enemyHealth;
    }
}