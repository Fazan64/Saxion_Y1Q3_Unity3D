using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Parachute : MonoBehaviour {

    [SerializeField] float dragCoeficient = 0.05f;
    [SerializeField] Rigidbody targetRigidbody;
    [SerializeField] bool autoDetach = true;
    [SerializeField] float autoDetachMinSpeed = 0.1f;

    bool isDetached;

	void Start () {

        targetRigidbody = targetRigidbody ?? GetComponentInParent<Rigidbody>();
	}
	
	void FixedUpdate () {

        if (!isDetached && autoDetach && targetRigidbody.velocity.magnitude <= autoDetachMinSpeed) {
            
            Detach();
        }

        ApplyForces();
	}

    public void Detach() {

        var fadeout = 
            GetComponent<BodyFadeout>() ?? 
            gameObject.AddComponent<BodyFadeout>();
        fadeout.enabled = true;

        if (transform.parent != null) {
            transform.SetParent(transform.parent.parent, worldPositionStays: true);
        }

        FlyOff();

        isDetached = true;
    }

    private void ApplyForces() {
        
        float verticalSpeed = Vector3.Dot(targetRigidbody.velocity, transform.up);
        if (verticalSpeed < 0f) {

            Vector3 force = transform.up * verticalSpeed * verticalSpeed * dragCoeficient * Time.fixedDeltaTime;
            targetRigidbody.AddForce(force);
        }

        float angle = 0f;
        Vector3 axis = Vector3.zero;
        Quaternion
            .FromToRotation(transform.up, Vector3.up)
            .ToAngleAxis(out angle, out axis);

        targetRigidbody.AddTorque(axis * angle * Time.fixedDeltaTime);
    }

    private void FlyOff() {
        
        var rb = gameObject.AddComponent<Rigidbody>();

        Vector3 direction = (Vector3.up * 4f + Random.onUnitSphere).normalized;
        rb.AddForce(direction * Random.Range(70f, 100f));
        rb.AddTorque(Random.onUnitSphere * 100f);

        targetRigidbody = rb;
    }
}
