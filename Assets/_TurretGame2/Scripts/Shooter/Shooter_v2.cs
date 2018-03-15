using UnityEngine;

public class Shooter_v2 : MonoBehaviour {

	//public Transform spawnPoint;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Shoot! (Left mouse button pressed)");

			GameObject bullet = new GameObject();
			//GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//bullet.transform.position = spawnPoint.position;
			//bullet.transform.rotation = spawnPoint.rotation;
			//bullet.transform.localScale = Vector3.one * 0.3f;
		}

	}
}
