using UnityEngine;

public class Shooter_v6 : MonoBehaviour {

	public Transform spawnPoint;
	public AudioClip shootSound;

	[SerializeField]
	private AudioSource source;

	//recoil settings
	public Transform barrel;
	private Vector3 barrelStart;
	public Vector3 localRecoilOffset;
	public float recoilSpeed;

	//reload time
	//private float nextShotTime;
	//public float reloadTime = 1;

	private void Awake()
	{
		barrelStart = barrel.transform.localPosition;
		//nextShotTime = Time.realtimeSinceStartup + reloadTime;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
		//if (Time.realtimeSinceStartup > nextShotTime && Input.GetMouseButton(0)) {
			Debug.Log("Shoot! (Left mouse button pressed)");

			//GameObject bullet = new GameObject();
			GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			bullet.transform.position = spawnPoint.position;
			bullet.transform.rotation = spawnPoint.rotation;
			bullet.transform.localScale = Vector3.one * 0.3f;

			bullet.AddComponent<Bullet>();

			source.PlayOneShot(shootSound);

			barrel.transform.localPosition = barrelStart + localRecoilOffset;

			//nextShotTime = Time.realtimeSinceStartup + reloadTime;
		}

		barrel.transform.localPosition = Vector3.Lerp(barrel.transform.localPosition, barrelStart, Time.deltaTime * recoilSpeed);

	}
}
