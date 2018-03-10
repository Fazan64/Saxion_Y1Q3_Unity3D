using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovementController : MonoBehaviour {

    GameObject player;
    NavMeshAgent agent;

    void Start() {

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player");
        Debug.Assert(player != null, "Player not found by tag!");
    }

    void Update() {

        agent.destination = player.transform.position;
    }
}
