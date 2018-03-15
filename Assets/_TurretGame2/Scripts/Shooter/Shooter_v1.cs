using UnityEngine;

public class Shooter_v1 : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("Shoot! (Spacebar pressed)");
		if (Input.GetMouseButtonDown(0)) Debug.Log("Shoot! (Left mouse button pressed)");
		if (Input.GetButtonDown("Fire1")) Debug.Log("Shoot! (Fire1 button pressed)");
	}
}
