using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFadeout : MonoBehaviour {

    [SerializeField] float fadeoutTime = 1f;

    new Renderer renderer;
    float initialAlpha;
    float timeSinceActivation;

    void Awake() {

        renderer = GetComponent<Renderer>();
        Debug.Assert(renderer != null);
    }

    void OnEnable() {

        initialAlpha = renderer.material.color.a;
        timeSinceActivation = 0f;
    }

    // Update is called once per frame
    void Update() {

        timeSinceActivation += Time.deltaTime;

        Color color = renderer.material.color;
        color.a = Mathf.Lerp(initialAlpha, 0f, timeSinceActivation / fadeoutTime);
        renderer.material.color = color;

        if (timeSinceActivation >= 1f) {
            Destroy(gameObject);
        }
    }

    public void SetFadeoutTime(float newValue) {

        fadeoutTime = newValue;
    }
}
