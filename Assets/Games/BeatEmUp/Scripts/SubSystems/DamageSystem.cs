using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public class DamageSystem : MonoBehaviour
    {
        [SerializeField] private int _damageDealt = 1;
        public int GetDamageDealt() => _damageDealt;

        public void Initialize(EnemyDataSO enemyData)
        {
            _damageDealt = enemyData.GetDamageDealt();
        }
    }
}