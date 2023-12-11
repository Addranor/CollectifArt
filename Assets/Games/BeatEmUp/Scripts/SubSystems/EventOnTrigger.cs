using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    [SerializeField] private bool _canAccess;
    [SerializeField][Space()] private UnityEvent _onTrigger;

    public void CanAccess(bool status) => _canAccess = status;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_canAccess && col.CompareTag("Player"))
            _onTrigger.Invoke();
    }
}
