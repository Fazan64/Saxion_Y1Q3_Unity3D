using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(ShootingController))]
public class EnemyAI : MonoBehaviour {
    
    public float startShootDistance = 10f;
    public float endShootDistance = 15f;
    public float aimingTime = 1f;

    GameObject player;

    Health health;
    NavMeshAgent agent;
    ShootingController shooter;

    FiniteStateMachine finiteStateMachine;

    float timeTillCanShoot;
    Material material;

    void Start() {

        health = GetComponent<Health>();
        agent  = GetComponent<NavMeshAgent>();
        shooter  = GetComponent<ShootingController>();
        material = GetComponent<Renderer>().material;

        player = GameObject.FindWithTag("Player");
        Debug.Assert(player != null, "Player not found by tag!");

        finiteStateMachine = new FiniteStateMachine(StateFollowPlayer);
    }

    void Update() {

        finiteStateMachine.Update();
    }

    private void StateFollowPlayer() {

        if (GetDistanceToPlayer() <= startShootDistance && shooter.CanShootAt(player)) {

            timeTillCanShoot = aimingTime;
            finiteStateMachine.ReplaceState(StateShooting);
            return;
        }

        material.color = Color.white;
        agent.isStopped = false;
        agent.destination = player.transform.position;
    }

    private void StateShooting() {
        
        if (GetDistanceToPlayer() > endShootDistance || !shooter.CanShootAt(player)) {

            finiteStateMachine.ReplaceState(StateFollowPlayer);
            return;
        }

        agent.isStopped = true;

        if (timeTillCanShoot > 0f) {

            timeTillCanShoot -= Time.deltaTime;
            if (timeTillCanShoot <= 0f) {

                timeTillCanShoot = aimingTime;
                shooter.ShootAt(player);
            }
        }

        material.color = Color.Lerp(Color.white, Color.yellow, (1f - timeTillCanShoot) * (1f - timeTillCanShoot));
    }

    private float GetDistanceToPlayer() {

        return (player.transform.position - transform.position).magnitude;
    }
}
