using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour {

    private Health health;
    private Slider slider;

    void Start() {

        slider = GetComponent<Slider>();
        health = Player.instance.health;
    }

    void Update() {

        slider.normalizedValue = health.health / (float)health.maxHealth;
    }
}
