using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private int _damageDealt = 1;
    public int GetDamages() => _damageDealt;
}
