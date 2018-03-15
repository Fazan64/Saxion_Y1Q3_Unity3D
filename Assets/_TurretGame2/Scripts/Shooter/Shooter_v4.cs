using UnityEngine;

public class Shooter_v4 : MonoBehaviour {

	public Transform spawnPoint;
	public AudioClip shootSound;

	//private AudioSource source;

	private void Awake()
	{
		//source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Shoot! (Left mouse button pressed)");

			//GameObject bullet = new GameObject();
			GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			bullet.transform.position = spawnPoint.position;
			bullet.transform.rotation = spawnPoint.rotation;
			bullet.transform.localScale = Vector3.one * 0.3f;

			bullet.AddComponent<Bullet>();

			//source.PlayOneShot(shootSound);
		}

	}
}
