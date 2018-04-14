using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IAgent {
    
    public float startShootDistance = 10f;
    public float endShootDistance = 15f;
    public float aimingTime = 1f;

    public FSM<Enemy> fsm { get; private set; }

    public NavMeshAgent navMeshAgent             { get; private set; } 
    public Health health                         { get; private set; }
    public new Rigidbody rigidbody               { get; private set; }
    public ShootingController shootingController { get; private set; }
    public Material material                     { get; private set; }

    void Start() {

        GlobalEvents.OnEnemyCreated.Invoke(gameObject);

        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        rigidbody = GetComponent<Rigidbody>();

        shootingController = GetComponentInChildren<ShootingController>();
        material = GetComponent<Renderer>().material;

        Assert.IsNotNull(shootingController);
        Assert.IsNotNull(material);

        fsm = new FSM<Enemy>(this);
        if (fsm.GetCurrentState() == null) {
            fsm.ChangeState<EnemyParachuteDeployingState>();
        }

        health.OnDeath += (sender) => GlobalEvents.OnEnemyDead.Invoke(gameObject);
    }

    public void Print(string message) {
        
        Debug.Log(typeof(Enemy).Name + ": " + message);
    }

    public float GetDistanceToPlayer() {
        
        GlobalEvents.OnEnemyCreated.Invoke(gameObject);
        Vector3 playerPosition = Player.instance.transform.position;
        return (playerPosition - transform.position).magnitude;
    }

    public bool CanShootAtPlayer() {

        return shootingController.CanShootAt(Player.instance.gameObject);
    }
}
