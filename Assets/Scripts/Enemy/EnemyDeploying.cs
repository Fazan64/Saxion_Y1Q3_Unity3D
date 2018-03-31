using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649

/// Activates the enemy when it reaches the ground after being airdropped.
[RequireComponent(typeof(EnemyStateController))]
public class EnemyDeploying : MonoBehaviour {

    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckRadius = 1f;

    EnemyStateController stateController;

    void Start() {

        stateController = GetComponent<EnemyStateController>();
        stateController.SetStateRagdoll();
    }

    void Update() {
        
        bool landed = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayerMask);
        if (landed) {
            
            stateController.SetStateActive();
            DetachParachuteIfAttached();
            this.enabled = false;
        }
    }

    private void DetachParachuteIfAttached() {

        var parachute = GetComponentInChildren<Parachute>();
        if (parachute == null) return;

        parachute.Detach();
    }
}
