using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

public class PlayerController : MonoBehaviour {

    public float muzzleSpeed = 100f;
    public float numSecondsTillFullWeaponCharge = 2f;
    public Vector3 gunOffset = Vector3.back * 0.5f;
    public GameObject bulletPrefab;

    [SerializeField] Transform barrelEnd;
    [SerializeField] Transform gun;
    [SerializeField] Shield shield;

    [SerializeField] AnimationCurve muzzleSpeedModifier;
    [SerializeField] AnimationCurve explosionSizeModifier;

    private float weaponChargeup;
    private bool isChargingUp;
    private Vector3 defaultGunLocalPosition;

    private float weaponChargeupPerSecond {
        get {
            return 1f / numSecondsTillFullWeaponCharge;
        }
    }

    // Use this for initialization
    void Start() {

        Debug.Assert(barrelEnd != null);
        Debug.Assert(bulletPrefab != null);
        Debug.Assert(shield != null);
        Debug.Assert(gun != null);

        defaultGunLocalPosition = gun.localPosition;
    }

    // Update is called once per frame
    void Update() {

        if (ShieldButtonPressed()) {

            shield.SetIsOn(!shield.isOn);

            if (shield.isOn) {

                weaponChargeup = 0f;
                isChargingUp = false;
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            
           isChargingUp = true;
        }

        if (isChargingUp) {

            weaponChargeup += weaponChargeupPerSecond * Time.deltaTime;
            weaponChargeup = Mathf.Clamp01(weaponChargeup);
        }

        if (Input.GetButtonUp("Fire1")) {

            if (shield.isOn) shield.TurnOff();

            Fire();

            isChargingUp = false;
            weaponChargeup = 0f;
        }

        Vector3 targetGunLocalPosition = defaultGunLocalPosition + gunOffset * weaponChargeup;
        targetGunLocalPosition += UnityEngine.Random.onUnitSphere * 0.02f * weaponChargeup;
        gun.localPosition = Vector3.MoveTowards(gun.localPosition, targetGunLocalPosition, 2f * Time.deltaTime);
    }

    private bool ShieldButtonPressed() {

        return Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E);
    }

    private void Fire() {

        GameObject bullet = Instantiate(
            bulletPrefab,
            barrelEnd.position,
            barrelEnd.rotation
        );

        bullet.GetComponent<Projectile>().explosionModifier = explosionSizeModifier.Evaluate(weaponChargeup);

        var rb = bullet.GetComponentInChildren<Rigidbody>();
        Debug.Assert(rb != null);

        float speedModifier = muzzleSpeedModifier.Evaluate(weaponChargeup);
        Vector3 ownVelocity = GetComponent<CharacterController>().velocity;

        rb.AddForce(
            rb.transform.forward * muzzleSpeed * speedModifier + ownVelocity,
            ForceMode.VelocityChange
        );
    }
}
