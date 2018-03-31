using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents {

    public static readonly UnityEvent OnEnemyDead    = new UnityEvent();
    public static readonly UnityEvent OnEnemyCreated = new UnityEvent();

    static GlobalEvents() {


    }
}
