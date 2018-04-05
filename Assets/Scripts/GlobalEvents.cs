using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents {

    public class EnemyEvent : UnityEvent<GameObject> {}

    public static readonly EnemyEvent OnEnemyDead    = new EnemyEvent();
    public static readonly EnemyEvent OnEnemyCreated = new EnemyEvent();

    public static readonly UnityEvent OnGameStarted = new UnityEvent();
    public static readonly UnityEvent OnGamePaused  = new UnityEvent();
}
