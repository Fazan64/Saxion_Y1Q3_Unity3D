using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour {

    public float timeTillRestart = 2f;
    public float timeScaleMultiplier = 0.4f;

    private float originalTimeScale;
    private float originalFixedDeltaTime;

    void Start() {

        var health = GetComponent<Health>();
        health.OnDeath += OnDeathHandler;
    }

    private void OnDeathHandler(Health sender) {

        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var firstPersonController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        if (firstPersonController != null) firstPersonController.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        originalTimeScale      = Time.timeScale;
        originalFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale      *= timeScaleMultiplier;
        Time.fixedDeltaTime *= timeScaleMultiplier;
        Invoke("Restart", timeTillRestart);
    }

    private void Restart() {

        Time.timeScale      = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        DG.Tweening.DOTween.Clear(destroy: true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
