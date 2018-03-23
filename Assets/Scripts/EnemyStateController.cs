using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class EnemyStateController : MonoBehaviour {

    Health health;

	// Use this for initialization
	void Start () {
        
        health = GetComponent<Health>();
        health.OnDeath += (sender) => SwitchToDead();
	}

    private void SwitchToDead() {

        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null) navMeshAgent.enabled = false;

        var ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
    }
}
