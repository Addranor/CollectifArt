using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "SpriteSave")]
public class PlayerSpriteSO : ScriptableObject
{
    [SerializeField] private bool _cape;
    [SerializeField] private bool _weapon;
    [SerializeField] private bool _shield;
    [SerializeField] private bool _banana;

    public void SetCape(bool status) => _cape = true;
    public void SetWeapon(bool status) => _weapon = true;
    public void SetShield(bool status) => _shield = true;
    public void SetBanana(bool status) => _banana = true;

    public bool GetCape() => _cape;
    public bool GetWeapon() => _weapon;
    public bool GetShield() => _shield;
    public bool GetBanana() => _banana;

    public void Reset()
    {
        SetCape(false);
        SetWeapon(false);
        SetShield(false);
        SetBanana(false);
    }
}
