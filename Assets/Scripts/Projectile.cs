using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour {

    public float bounceBackSpeedMultiplier = 3f;
    public GameObject explosionPrefab;

    public int impactDamage = 10;
    public float explosionModifier = 1f;

    Vector3 startPosition;

    void Start() {
        
        startPosition = transform.position;
    }

    public void Shoot(Vector3 velocity) {

        var rb = GetComponentInChildren<Rigidbody>();
        Debug.Assert(rb != null);
        rb.AddForce(
            velocity,
            ForceMode.VelocityChange
        ); 
    }

    void OnCollisionEnter(Collision collision) {

        if (ShouldBounceBack(collision)) {

            BounceToStartingPosition(
                startingSpeed: collision.relativeVelocity.magnitude * bounceBackSpeedMultiplier
            );
            return;
        }

        DealDamage(collision);
        if (explosionModifier >= 0.3f) CreateExplosion();
        Destroy(gameObject);
    }

    private void DealDamage(Collision collision) {
        
        var health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;

        bool wasAlive = health.isAlive;
        health.DealDamage(impactDamage);
        bool didDie = wasAlive && health.isDead;

        if (didDie) {

            var rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddForce(-collision.impulse, ForceMode.Impulse);
            }
        }
    }

    private bool ShouldBounceBack(Collision collision) {

        return collision.collider.gameObject.CompareTag("DeflectProjectiles");
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

    private void CreateExplosion() {

        if (explosionPrefab == null) return;
        GameObject explosion = Instantiate(
            explosionPrefab, transform.position, Quaternion.identity
        );

        explosion.GetComponent<Explosion>().modifier = explosionModifier;
    }
}
