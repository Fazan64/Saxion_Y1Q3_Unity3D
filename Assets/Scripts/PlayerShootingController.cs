using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour {

    public float muzzleSpeed = 100f;
    public GameObject bulletPrefab;

    [SerializeField] Transform barrelEnd;

    // Use this for initialization
    void Start() {

        Debug.Assert(barrelEnd != null);
        Debug.Assert(bulletPrefab != null);
    }

    // Update is called once per frame
    void Update() {

        if (!Input.GetButtonDown("Fire1")) return;

        GameObject bullet = Instantiate(
            bulletPrefab, 
            barrelEnd.position,
            barrelEnd.rotation
        );

        var rigidbody = bullet.GetComponentInChildren<Rigidbody>();
        Debug.Assert(rigidbody != null);
        rigidbody.AddRelativeForce(
            Vector3.forward * muzzleSpeed,
            ForceMode.VelocityChange
        );
    }
}
