using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {

    public int score;

    void Awake() {
        
        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDeadHandler);
    }

    private void OnEnemyDeadHandler(GameObject enemy) {

        score += 100;
    }
}
