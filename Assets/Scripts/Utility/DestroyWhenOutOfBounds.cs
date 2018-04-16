using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Destroys its gameobject when it gets too far from origin 
public class DestroyWhenOutOfBounds : MonoBehaviour {

    [SerializeField] float maxDistance = 100f;
    
	void FixedUpdate() {

        Vector3 position = transform.position;
        if (
            Mathf.Abs(position.x) > maxDistance ||
            Mathf.Abs(position.y) > maxDistance ||
            Mathf.Abs(position.z) > maxDistance
        ) {
            Destroy(gameObject);
        }
	}
}
