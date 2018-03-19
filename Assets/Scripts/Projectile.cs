using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour {

    public int damage = 1;
    public float bounceBackSpeedMultiplier = 3f;
    public float explosionRadius = 1f;

    Vector3 startPosition;

    void Start() {

        startPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision) {

        if (collision.collider.gameObject.CompareTag("DeflectProjectiles")) {

            BounceToStartingPosition(
                startingSpeed: collision.relativeVelocity.magnitude * bounceBackSpeedMultiplier
            );
            return;
        }

        PlayExplosionEffect();
        Destroy(gameObject);

        /*
        var health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;
        health.health -= damage;
        if (health.health <= 0) {
            
            //Push(collision);
            Destroy(collision.gameObject, 5f);
        }*/

        // damage all in radius
        // switch to death
        // apply forces to all in radius

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider otherCollider in colliders) {

            GameObject go = otherCollider.gameObject;

            var health = go.GetComponent<Health>();
            if (health == null) continue;
            health.health -= damage;

            if (health.health <= 0) {

                SwitchToDead(go);
                ApplyExplosiveForce(go);
                Destroy(go, 5f);
            }
        }
    }

    private void BounceToStartingPosition(float startingSpeed) {

        var rb = GetComponent<Rigidbody>();

        Vector3? startVelocity = Ballistics.GetStartVelocity(
            start: rb.position,
            target: startPosition,
            muzzleSpeed: startingSpeed
        );

        if (startVelocity.HasValue) {
            rb.velocity = startVelocity.Value;
        }
    }

    private void PlayExplosionEffect() {

        Debug.Log("Explosion goes here.");
    }

    private void SwitchToDead(GameObject go) {

        var characterController = go.GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var navMeshAgent = go.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null) navMeshAgent.enabled = false;

        var ai = go.GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;
    }

    private void ApplyExplosiveForce(GameObject go) {

        var rb = go.GetComponent<Rigidbody>();
        if (rb != null) {

            rb.isKinematic = false;

            Vector3 delta = rb.position - transform.position;
            rb.AddExplosionForce(2000f, transform.position, 2f, 0.5f);
        }
    }
}
