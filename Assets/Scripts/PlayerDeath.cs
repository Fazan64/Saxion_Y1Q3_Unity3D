using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour {

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

        Invoke("Restart", 5f);
    }

    private void Restart() {

        SceneManager.LoadScene("main");
    }
}
