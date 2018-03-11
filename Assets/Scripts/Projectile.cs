using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public int damage = 1;

    private void OnCollisionEnter(Collision collision) {

        Explode();

        var health = collision.gameObject.GetComponent<Health>();
        if (health == null) return;

        health.health -= damage;
    }

    void Explode() {

        Debug.Log("Explosion goes here.");
        Destroy(gameObject);
    }
}
