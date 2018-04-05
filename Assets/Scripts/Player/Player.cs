using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    static Player _instance;

    public static Player instance {
        get {
            return _instance; 
        }
    }

    public int score;

    void Awake() {

        Debug.Assert(_instance == null);
        _instance = this;

        GlobalEvents.OnGameStarted.AddListener(() => gameObject.SetActive(true));
        GlobalEvents.OnGamePaused.AddListener(() => gameObject.SetActive(false));

        GlobalEvents.OnEnemyDead.AddListener(OnEnemyDeadHandler);
    }

    private void OnEnemyDeadHandler(GameObject enemy) {

        score += 100;
    }
}
