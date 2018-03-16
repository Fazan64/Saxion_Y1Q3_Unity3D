using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage = 1;

    Vector3 startPosition;

    void Start() {

        startPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision) {

        if (collision.collider.gameObject.CompareTag("DeflectProjectiles")) {

            var rb = GetComponent<Rigidbody>();

            Vector3 startVelocity = Ballistics.GetStartVelocity(
                start: collision.contacts[0].point,
                target: startPosition,
                muzzleSpeed: collision.relativeVelocity.magnitude
            );
            rb.velocity = startVelocity;
            return;
        }

        Explode();

        var health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;

        health.health -= damage;
    }

    private void Explode() {

        Debug.Log("Explosion goes here.");
        Destroy(gameObject);
    }
}
