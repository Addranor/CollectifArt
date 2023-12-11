using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossBattle
{
    public class DamageSystem : MonoBehaviour
    {
        [SerializeField] private int _damageDealt = 1;
        public int GetDamageDealt() => _damageDealt;
        public int SetDamageDealt(int newValue) => _damageDealt = newValue;
    }
}