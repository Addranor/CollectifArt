using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public class PlayerPickup : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Pickup")) return;
            _player.SetPickup(other.GetComponent<Pickable>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Pickup")) return;
            _player.SetPickup(null);
        }
    }
}