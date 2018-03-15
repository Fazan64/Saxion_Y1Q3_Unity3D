using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_v8 : MonoBehaviour {

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
	private float nextShotTime;
	public float reloadTime = 1;

	//muzzle flash
	public GameObject muzzleFlash;

	//barrel smoke
	//public ParticleSystem smoke;

	private void Awake()
	{
		barrelStart = barrel.transform.localPosition;
		nextShotTime = Time.realtimeSinceStartup + reloadTime;
		muzzleFlash.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetMouseButton(0)) {
		if (Time.realtimeSinceStartup > nextShotTime && Input.GetMouseButton(0))
		{
			Debug.Log("Shoot! (Left mouse button pressed)");

			//GameObject bullet = new GameObject();
			GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			bullet.transform.position = spawnPoint.position;
			bullet.transform.rotation = spawnPoint.rotation;
			bullet.transform.localScale = Vector3.one * 0.3f;

			bullet.AddComponent<Bullet>();

			source.PlayOneShot(shootSound);

			barrel.transform.localPosition = barrelStart + localRecoilOffset;

			nextShotTime = Time.realtimeSinceStartup + reloadTime;

			ShowMuzzleFlash();
			//ShowSmoke();
		}

		barrel.transform.localPosition = Vector3.Lerp(barrel.transform.localPosition, barrelStart, Time.deltaTime * recoilSpeed);

	}

	void ShowMuzzleFlash()
	{
		muzzleFlash.SetActive(true);
		muzzleFlash.transform.localEulerAngles =
			new Vector3(0, 0, Random.Range(0, 360));
		Invoke("HideMuzzleFlash", 0.05f);
	}

	void HideMuzzleFlash()
	{
		muzzleFlash.SetActive(false);
	}

	/*
	private void ShowSmoke()
	{
		smoke.Play();
		Invoke("HideSmoke", 1f);
	}

	private void HideSmoke()
	{
		smoke.Stop();
	}
	*/


}
