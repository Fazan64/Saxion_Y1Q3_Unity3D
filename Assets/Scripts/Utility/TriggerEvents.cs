﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// Exposes OnTriggerEnter as a UnityEvent.
public class TriggerEvents : MonoBehaviour {

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> {}

    [SerializeField] TriggerEvent _onTriggerEnter = new TriggerEvent();
    public TriggerEvent onTriggerEnter { get { return _onTriggerEnter; }}

    [SerializeField] UnityEvent _onPlayerTriggerEnter = new UnityEvent();
    public UnityEvent onPlayerTriggerEnter { get { return _onPlayerTriggerEnter; } }

    void OnTriggerEnter(Collider other) {

        onTriggerEnter.Invoke(other);

        if (
            other.attachedRigidbody != null && 
            other.attachedRigidbody.gameObject == Player.instance.gameObject
        ) {
            onPlayerTriggerEnter.Invoke();
        }
    }
}
