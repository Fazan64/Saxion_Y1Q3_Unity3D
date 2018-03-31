using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class Parachute : MonoBehaviour {

    [SerializeField] float dragCoeficient = 0.05f;
    [SerializeField] Rigidbody targetRigidbody;

    bool isDetached;

	// Use this for initialization
	void Start () {

        targetRigidbody = targetRigidbody ?? GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if (isDetached) return;

        float verticalSpeed = Vector3.Dot(targetRigidbody.velocity, transform.up);
        if (verticalSpeed >= 0f) return;

        Vector3 force = transform.up * verticalSpeed * verticalSpeed * dragCoeficient;
        targetRigidbody.AddForce(force);
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

    private void FlyOff() {
        
        var rb = gameObject.AddComponent<Rigidbody>();
        Vector3 direction = (Vector3.up * 4f + Random.onUnitSphere).normalized;
        rb.AddForce(direction * Random.Range(70f, 100f));
        rb.AddTorque(Random.onUnitSphere * 100f);
    }
}
