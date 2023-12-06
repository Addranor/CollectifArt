using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossBattle
{
    [CreateAssetMenu(fileName = "Boss Attack", menuName = "Boss Attack")]
    public class BossAttack : ScriptableObject
    {
        [SerializeField] private GameObject _gameObject;
    }
}