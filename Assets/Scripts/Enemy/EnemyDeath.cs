using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(Enemy))]
public class EnemyDeath : MonoBehaviour {

    private Enemy enemy;

    void Start () {

        enemy = GetComponent<Enemy>();
        GetComponent<Health>().OnDeath += (sender) => {

            GlobalEvents.OnEnemyDead.Invoke(gameObject);
            SwitchToDead();
        };
	}

    private void SwitchToDead() {
        
        enemy.fsm.ChangeState<EnemyDeadState>();
        StartFadeout();
    }

    private void StartFadeout() {

        var fadeout =
                GetComponent<BodyFadeout>() ??
                gameObject.AddComponent<BodyFadeout>();

        fadeout.enabled = true;
    }
}
