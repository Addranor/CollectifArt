using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    [CreateAssetMenu(menuName = "New Enemy Pack", fileName = "Enemy Pack")]
    public class EnemyPack : ScriptableObject
    {
        [SerializeField] private List<EnemyDataSO> _enemiesToSpawn;
        public List<EnemyDataSO> GetEnemyPack() => _enemiesToSpawn;
        public int GetEnemyAmount() => _enemiesToSpawn.Count;
    }
}