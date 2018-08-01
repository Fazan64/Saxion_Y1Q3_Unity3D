using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

[RequireComponent(typeof(Slider))]
public class GunChargeupIndicator : MonoBehaviour {

    private PlayerGun playerGun;
    private Slider slider;

    void Start() {

        slider = GetComponent<Slider>();
        playerGun = Player.instance.GetComponent<PlayerGun>();
    }

    void Update() {

        slider.normalizedValue = playerGun.currentChargeup;
    }
}
