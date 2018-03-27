using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void Update () {
		//What should we use??
		transform.Translate(transform.forward, Space.World);		
		//transform.Translate(Vector3.forward, Space.Self);		
	}
}
