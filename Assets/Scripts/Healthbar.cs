using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Healthbar : MonoBehaviour {

    public Health health;

    Slider slider;

    void Start() {

        slider = GetComponent<Slider>();
    }

    void Update() {

        if (health == null) return;

        slider.normalizedValue = health.health / 100f;
    }
}
