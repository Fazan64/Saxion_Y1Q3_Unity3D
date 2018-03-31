using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
public class EnemyDeath : MonoBehaviour {

    [SerializeField] float bodyFadeoutDelay = 1f;
    [SerializeField] float bodyFadeoutTime  = 1f;

    Health health;
    bool isDead;

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

        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null) navMeshAgent.enabled = false;

        var ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        isDead = true;

        Invoke("StartFadeout", bodyFadeoutDelay);
    }

    private void StartFadeout() {

        var fadeout =
                GetComponent<BodyFadeout>() ??
                gameObject.AddComponent<BodyFadeout>();

        fadeout.SetFadeoutTime(bodyFadeoutTime);
        fadeout.enabled = true;
    }
}
