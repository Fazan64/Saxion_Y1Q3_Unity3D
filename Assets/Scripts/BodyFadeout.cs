using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFadeout : MonoBehaviour {

    [SerializeField] float fadeoutDelay = 0f;
    [SerializeField] float fadeoutTime  = 1f;

    new Renderer renderer;
    float initialAlpha;

    bool isFadingOut;
    float timeSinceStartedFadeout;

    void Awake() {

        renderer = GetComponentInChildren<Renderer>();
        Debug.Assert(renderer != null);
    }

    void OnEnable() {

        initialAlpha = renderer.material.color.a;

        Invoke("StartFadeout", fadeoutDelay);
    }

    // Update is called once per frame
    void Update() {

        if (!isFadingOut) return;

        timeSinceStartedFadeout += Time.deltaTime;

        Color color = renderer.material.color;
        color.a = Mathf.Lerp(initialAlpha, 0f, timeSinceStartedFadeout / fadeoutTime);
        renderer.material.color = color;

        if (timeSinceStartedFadeout >= 1f) {
            Destroy(gameObject);
        }
    }

    public BodyFadeout SetFadeoutDelay(float newValue) {

        fadeoutDelay = newValue;
        return this;
    }

    public BodyFadeout SetFadeoutTime(float newValue) {

        fadeoutTime = newValue;
        return this;
    }

    private void StartFadeout() {

        isFadingOut = true;
        timeSinceStartedFadeout = 0f;
    }
}
