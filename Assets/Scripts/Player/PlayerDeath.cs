﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour {

    [SerializeField] float timeTillRestart = 2f;
    [SerializeField] float timeScaleMultiplier = 0.4f;
    [SerializeField] Collider afterDeathCollider;

    private bool didDie;
    private float originalTimeScale;
    private float originalFixedDeltaTime;

    void Start() {

        var health = GetComponent<Health>();
        health.OnDeath += OnDeathHandler;
    }

    void OnDestroy() {

        if (didDie) {
            Time.timeScale = originalTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }

    private void OnDeathHandler(Health sender) {

        var characterController = GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        var firstPersonController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        if (firstPersonController != null) firstPersonController.enabled = false;

        var playerController = GetComponent<PlayerController>();
        if (playerController != null) playerController.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        if (afterDeathCollider != null) afterDeathCollider.enabled = true;

        originalTimeScale      = Time.timeScale;
        originalFixedDeltaTime = Time.fixedDeltaTime;

        Time.timeScale      *= timeScaleMultiplier;
        Time.fixedDeltaTime *= timeScaleMultiplier;

        didDie = true;
    }
}
