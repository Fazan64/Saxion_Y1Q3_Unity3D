using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage = 1;

    void OnCollisionEnter(Collision collision) {

        if (collision.collider.gameObject.CompareTag("DeflectProjectiles")) {

            //var rb = GetComponent<Rigidbody>();
            //rb.velocity += collision.relativeVelocity;
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
