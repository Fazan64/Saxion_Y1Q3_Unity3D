using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649

public class EnemyStateController : MonoBehaviour {

    [SerializeField] new Rigidbody rigidbody;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] EnemyAI ai;

    void Awake() {

        DetectComponents();
    }

    [ContextMenu("SetStateRagdoll")]
    public void SetStateRagdoll() {

        rigidbody.isKinematic = false;
        navMeshAgent.enabled  = false;
        ai.enabled            = false;
    }

    [ContextMenu("SetStateActive")]
    public void SetStateActive() {

        rigidbody.isKinematic = true;
        navMeshAgent.enabled  = true;
        ai.enabled            = true;
    }

    [ContextMenu("DetectComponents")]
    private void DetectComponents() {

        rigidbody = rigidbody ?? GetComponent<Rigidbody>();
        navMeshAgent = navMeshAgent ?? GetComponent<NavMeshAgent>();
        ai = ai ?? GetComponent<EnemyAI>();
    }
}
