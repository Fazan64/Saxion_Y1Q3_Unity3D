using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : Singleton<Player> {

    public event Action<Player> OnDeath = delegate {};

    public int score;

    public Health health { get; private set; }

    void Awake() {
        
        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDeadHandler);
        OnDeath += (sender) => {
            GlobalEvents.OnEnemyDead.RemoveListener(OnEnemyDeadHandler);
        };
    }

    void OnDestroy() {

        GlobalEvents.OnEnemyDead.RemoveListener(OnEnemyDeadHandler);
    }

    void Start() {

        health = GetComponent<Health>();
        health.OnDeath += (sender) => OnDeath.Invoke(this);
    }

    private void OnEnemyDeadHandler(GameObject enemy) {
        
        score += 100;
    }
}
