using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    [CreateAssetMenu(menuName = "New Enemy Pack", fileName = "EnemyPack")]
    public class EnemyPack : ScriptableObject
    {
        [SerializeField] private List<GameObject> _enemiesToSpawn;
        public List<GameObject> GetEnemyPack() => _enemiesToSpawn;
    }
}