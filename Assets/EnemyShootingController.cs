using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour {

    public float shootCooldown = 1f;
    public GameObject bulletPrefab;

    float timeTillCanShoot = 0f;
    GameObject player;

	// Use this for initialization
	void Start () {
		
        player = GameObject.FindWithTag("Player");
        Debug.Assert(player != null, "Player not found by tag!");
	}
	
	// Update is called once per frame
	void Update () {
		
        if (timeTillCanShoot > 0f) {
            timeTillCanShoot -= Time.deltaTime;
        }

        if (timeTillCanShoot <= 0f && GetCanShootAtPlayer()) {

            ShootAtPlayer();
            timeTillCanShoot = shootCooldown;
        }
	}

    private bool GetCanShootAtPlayer() {

        RaycastHit hit;
        bool didHit = Physics.SphereCast(
            origin: transform.position,
            radius: 10f,
            direction: (player.transform.position - transform.position).normalized,
            hitInfo: out hit,
            maxDistance: 100f,
            layerMask: Physics.DefaultRaycastLayers & ~LayerMask.NameToLayer("Player")
        );

        return !didHit;
    }

    private void ShootAtPlayer() {

        Vector3 delta = player.transform.position - transform.position;
        Vector3 direction = delta.normalized;

        Shoot(position: transform.position + direction * 1.2f, startVelocity: direction * 100f);
    }

    private void Shoot(Vector3 position, Vector3 startVelocity) {

        Instantiate(bulletPrefab, position, Quaternion.identity)
            .GetComponent<Rigidbody>()
            .AddForce(startVelocity, ForceMode.VelocityChange);
    }
}
