using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour {

    public GameObject bulletPrefab;
    public float muzzleSpeed = 100f;

    public bool CanShootAt(GameObject target) {

        RaycastHit hit;
        bool didHit = Physics.SphereCast(
            origin: transform.position,
            radius: 10f,
            direction: (target.transform.position - transform.position).normalized,
            hitInfo: out hit,
            maxDistance: 100f,
            layerMask: Physics.DefaultRaycastLayers & ~target.layer
        );

        return !didHit || hit.collider.gameObject != gameObject;
    }

    public void ShootAt(GameObject target) {

        /*Vector3 delta = target.transform.position - transform.position;
        Vector3 direction = delta.normalized;

        Shoot(
            position: transform.position + direction * 1.2f, 
            startVelocity: direction * muzzleSpeed
        );*/

        Vector3 delta = target.transform.position - transform.position;
        Vector3 flatDelta = new Vector3(delta.x, 0f, delta.z);
        Vector3 direction = flatDelta.normalized;

        Vector3 shootPosition = transform.position + direction * 1.2f;
        //shootPosition.y += 1f;

        Vector3 startVelocity = Ballistics.GetStartVelocity(
            start: shootPosition,
            target: target.transform.position,
            muzzleSpeed: muzzleSpeed
        );

        Shoot(
            position: shootPosition,
            startVelocity: startVelocity
        );
    }

    private void Shoot(Vector3 position, Vector3 startVelocity) {

        Instantiate(bulletPrefab, position, Quaternion.identity)
            .GetComponent<Rigidbody>()
            .AddForce(startVelocity, ForceMode.VelocityChange);
    }
}
