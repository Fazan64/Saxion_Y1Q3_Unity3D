using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class CameraRotator : MonoBehaviour {

    [SerializeField] float rotationSpeed;

	void Update () {

        Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
        transform.rotation *= rotation;
	}
}
