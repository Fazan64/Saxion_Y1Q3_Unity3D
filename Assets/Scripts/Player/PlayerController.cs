using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

/// Processes player input for the shield and the gun.
[RequireComponent(typeof(Shield), typeof(PlayerGun))]
public class PlayerController : MonoBehaviour {

    private Shield shield;
    private PlayerGun playerGun;

    void Start() {

        shield = GetComponent<Shield>();
        playerGun = GetComponent<PlayerGun>();
    }

    void Update() {

        if (Input.GetButtonDown("Shield")) {

            shield.SetIsOn(!shield.isOn);
            if (shield.isOn) playerGun.Release();

        } else if (Input.GetButtonDown("Fire1")) {
            
            playerGun.Hold();

        } else if (Input.GetButtonUp("Fire1")) {

            if (shield.isOn) shield.TurnOff();
            playerGun.Release();
        }
    }
}
