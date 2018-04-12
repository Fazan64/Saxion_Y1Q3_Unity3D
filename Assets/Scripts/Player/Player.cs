using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : Singleton<Player> {

    public event Action<Player> OnDeath = delegate {};

    public int score;

    void Awake() {
        
        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDeadHandler);
    }

    void Start() {

        GetComponent<Health>().OnDeath += (sender) => OnDeath.Invoke(this);
    }

    private void OnEnemyDeadHandler(GameObject enemy) {

        score += 100;
    }
}
