using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

public class PlayerGun : MonoBehaviour {

    [SerializeField] float muzzleSpeed = 100f;
    [SerializeField] float numSecondsTillFullWeaponCharge = 2f;
    [SerializeField] Collider ownCollider;
    [SerializeField] Camera playerCamera;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject bulletPrefab;
    [Space]
    [SerializeField] Transform barrelEnd;
    [SerializeField] Transform gunTransform;
    [SerializeField] Vector3 gunOffset = Vector3.back * 0.5f;
    [SerializeField] LayerMask shootingRaycastLayerMask;

    [SerializeField] AnimationCurve muzzleSpeedModifier;
    [SerializeField] AnimationCurve explosionSizeModifier;

    private float weaponChargeup;
    private bool isChargingUp;
    private Vector3 defaultGunLocalPosition;

    public float currentChargeup { get { return weaponChargeup; } }

    private float weaponChargeupPerSecond {
        get {
            return 1f / numSecondsTillFullWeaponCharge;
        }
    }

    void Start() {
        
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(barrelEnd);
        Assert.IsNotNull(gunTransform);
        Assert.IsNotNull(playerCamera);
        Assert.IsNotNull(characterController);

        defaultGunLocalPosition = gunTransform.localPosition;
    }

    void FixedUpdate() {

        if (isChargingUp) {

            weaponChargeup += weaponChargeupPerSecond * Time.fixedDeltaTime;
            weaponChargeup = Mathf.Clamp01(weaponChargeup);
        }

        Vector3 targetGunLocalPosition = defaultGunLocalPosition + gunOffset * weaponChargeup;
        targetGunLocalPosition += Random.onUnitSphere * 0.02f * weaponChargeup;
        gunTransform.localPosition = Vector3.MoveTowards(gunTransform.localPosition, targetGunLocalPosition, 2f * Time.fixedDeltaTime);
    }

    public void Hold() {

        isChargingUp = true;
    }

    public void Release() {

        if (isChargingUp) {
            Fire();
        }

        isChargingUp = false;
        weaponChargeup = 0f;
    }

    private void Fire() {

        GameObject bullet = Instantiate(
            bulletPrefab,
            barrelEnd.position,
            barrelEnd.rotation
        );

        if (ownCollider != null) {
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), ownCollider);
        }

        bullet.GetComponent<Projectile>().explosionModifier = explosionSizeModifier.Evaluate(weaponChargeup);

        var rb = bullet.GetComponentInChildren<Rigidbody>();
        Assert.IsNotNull(rb);

        Vector3 direction;
        Transform cameraTransform = playerCamera.transform;
        var ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootingRaycastLayerMask)) {    
            direction = (hit.point - barrelEnd.position).normalized;
        } else {
            direction = cameraTransform.forward;
        }

        float speedModifier = muzzleSpeedModifier.Evaluate(weaponChargeup);

        Vector3 ownVelocity = characterController.velocity;

        rb.AddForce(
            direction * muzzleSpeed * speedModifier + ownVelocity,
            ForceMode.VelocityChange
        );
    }
}
