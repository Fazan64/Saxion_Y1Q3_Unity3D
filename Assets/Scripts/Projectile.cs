using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour {

    public float bounceBackSpeedMultiplier = 3f;
    public GameObject explosionPrefab;

    Vector3 startPosition;

    void Start() {
        startPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision) {

        if (ShouldBounceBack(collision)) {

            BounceToStartingPosition(
                startingSpeed: collision.relativeVelocity.magnitude * bounceBackSpeedMultiplier
            );
            return;
        }

        CreateExplosion();
        Destroy(gameObject);
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

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Debug.Log("Explosion goes here.");
    }
}
