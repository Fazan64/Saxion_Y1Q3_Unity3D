using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health), typeof(EnemyStateController))]
public class EnemyDeath : MonoBehaviour {

    Health health;

    void Awake() {

        GlobalEvents.OnEnemyCreated.Invoke(gameObject);
    }

    void Start () {
        
        health = GetComponent<Health>();
        health.OnDeath += (sender) => {

            GlobalEvents.OnEnemyDead.Invoke(gameObject);
            SwitchToDead();
        };
	}

    private void SwitchToDead() {

        GetComponent<EnemyStateController>().SetStateRagdoll();
        DetachParachuteIfAttached();
        StartFadeout();
    }

    private void StartFadeout() {

        var fadeout =
                GetComponent<BodyFadeout>() ??
                gameObject.AddComponent<BodyFadeout>();

        fadeout.enabled = true;
    }

    private void DetachParachuteIfAttached() {

        var parachute = GetComponentInChildren<Parachute>();
        if (parachute == null) return;

        parachute.Detach();
    }
}
