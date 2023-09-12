using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform _Respawn;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_Respawn == null) return;
        if (other.CompareTag("Player"))
        {
            // Do the death scene
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.position = _Respawn.position;
            return;
        }
        
        // Otherwise, destroy the enemy
        Destroy(other.gameObject);
    }
}
