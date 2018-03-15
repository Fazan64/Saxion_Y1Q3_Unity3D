using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float muzzleSpeed = 100f;
    public GameObject bulletPrefab;

    [SerializeField] Transform barrelEnd;
    [SerializeField] Shield shield;

    // Use this for initialization
    void Start() {

        Debug.Assert(barrelEnd != null);
        Debug.Assert(bulletPrefab != null);
        Debug.Assert(shield != null);
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E)) {
            shield.SetIsOn(!shield.isOn);
        }

        if (Input.GetButtonDown("Fire1")) {

            if (shield.isOn) shield.TurnOff();
            Fire();
        }
    }

    private void Fire() {

        GameObject bullet = Instantiate(
            bulletPrefab,
            barrelEnd.position,
            barrelEnd.rotation
        );

        var rb = bullet.GetComponentInChildren<Rigidbody>();
        Debug.Assert(rb != null);
        rb.AddRelativeForce(
            Vector3.forward * muzzleSpeed,
            ForceMode.VelocityChange
        );
    }
}
