using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    [SerializeField] float maxEnergy = 100f;
    [SerializeField] float minEnergyToTurnOn = 100f;
    [SerializeField] float remainingEnergy = 100f;
    [SerializeField] float useTime = 1f;
    [SerializeField] float recoveryTime = 1f;
    [SerializeField][Range(0f, 1f)] float maxOpacity = 1f;

    [SerializeField] new Collider collider;
    [SerializeField] new Renderer renderer;

    Health health;
    bool defaultCanBeReducedValue = true;

    public bool isOn { get; private set; }

    private void Start() {

        health = GetComponent<Health>();
        defaultCanBeReducedValue = health.canBeReduced;

        collider = collider ?? GetComponentInChildren<Collider>();
        renderer = renderer ?? GetComponentInChildren<Renderer>();

        renderer.enabled = true;
        renderer.material = Instantiate(renderer.material);

        TurnOff();
    }

    private void FixedUpdate() {

        float value = isOn ? Mathf.InverseLerp(0f, maxEnergy, remainingEnergy) : 0f;
        Color color = renderer.material.color;
        color.a = Mathf.Lerp(0f, maxOpacity, value);;
        renderer.material.color = color;

        if (isOn && remainingEnergy > 0f) {

            remainingEnergy -= (maxEnergy / useTime) * Time.fixedDeltaTime;

            if (remainingEnergy <= 0f) {
                TurnOff();
            }

        } else if (remainingEnergy < maxEnergy) {

            remainingEnergy += (maxEnergy / recoveryTime) * Time.fixedDeltaTime;

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
        health.SetCanBeReduced(defaultCanBeReducedValue);
    }
}
