using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] private GameObject _inputSprite;
        [SerializeField] private PickUps _pickUp;
        public PickUps GetPickup() => _pickUp;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _inputSprite.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _inputSprite.SetActive(false);
        }
        
    }
}