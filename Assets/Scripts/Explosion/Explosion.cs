using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int damage = 1;

    public float explosionForce  = 100f;
    public float radius = 2f;
    public float upwardsModifier = 0.5f;

    private IEnumerator Start() {

        // wait one frame because some explosions instantiate debris which should then
        // be pushed by physics force
        yield return null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        var rigidbodies = new List<Rigidbody>();

        foreach (Collider col in colliders) {
            
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody)) {

                rigidbodies.Add(col.attachedRigidbody);
            }
        }

        foreach (Rigidbody rb in rigidbodies) {

            var health = rb.gameObject.GetComponent<Health>();
            if (health != null) health.DealDamage(damage);

            rb.AddExplosionForce(
                explosionForce, 
                transform.position, 
                radius,
                upwardsModifier
            );
        }
    }
}
