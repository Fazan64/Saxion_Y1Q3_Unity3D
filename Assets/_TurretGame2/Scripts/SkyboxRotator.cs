using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour {

	public Material material;
	public float rotationSpeed;

	// Update is called once per frame
	void Update () {
		material.SetFloat("_Rotation", Time.realtimeSinceStartup * rotationSpeed);
	}
}
