using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float minEnergyToTurnOn = 100f;
    [SerializeField] float powerDraw = 100f;
    [SerializeField] float recoveryRate = 100f;
    [SerializeField] float remainingEnergy = 100f;

    [SerializeField] new Collider collider;
    [SerializeField] new Renderer renderer;

    Health health;

    public bool isOn { get; private set; }

    private void Awake() {

        collider = collider ?? GetComponentInChildren<Collider>();
        renderer = renderer ?? GetComponentInChildren<Renderer>();

        health = GetComponent<Health>();

        TurnOff();
    }

    private void FixedUpdate() {

        float value = isOn ? Mathf.InverseLerp(0f, maxEnergy, remainingEnergy) : 0f;
        renderer.material.SetFloat("_InvFade", value);

        if (isOn && remainingEnergy > 0f) {

            remainingEnergy -= powerDraw * Time.fixedDeltaTime;

            if (remainingEnergy <= 0f) {
                TurnOff();
            }

        } else if (remainingEnergy < maxEnergy) {

            remainingEnergy += recoveryRate * Time.fixedDeltaTime;

            if (remainingEnergy >= maxEnergy) {
                remainingEnergy = maxEnergy;
            }
        }
    }

    public void SetIsOn(bool value) {

        if (isOn == value) return;

        if (value) TurnOn();
        else       TurnOff();
    }

    public void TurnOn() {

        if (remainingEnergy < minEnergyToTurnOn) return;

        collider.enabled = true;
        isOn = true;
        health.SetCanBeReduced(false);
    }

    public void TurnOff() {

        collider.enabled = false;
        isOn = false;
        health.SetCanBeReduced(true);
    }
}
