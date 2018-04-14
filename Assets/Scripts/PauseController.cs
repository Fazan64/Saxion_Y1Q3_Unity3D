using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : Singleton<PauseController> {

    private bool isPaused;
    private float timeScaleBeforePause = 1f;

    private bool canPause = true;

    void Start() {

        Player.instance.OnDeath += OnPlayerDeath;;
    }

    void Update() {

        if (canPause && Input.GetKeyDown(KeyCode.Escape)) {

            if (isPaused) {
                Unpause();
            } else {
                Pause();
            }
        }
    }

    void OnDestroy() {
        
        Unpause();
    }

    private void Pause() {

        if (isPaused) return;

        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;

        isPaused = true;
        GlobalEvents.OnGamePause.Invoke();
    }

    private void Unpause() {

        if (!isPaused) return;

        Time.timeScale = timeScaleBeforePause;

        isPaused = false;
        GlobalEvents.OnGameUnpause.Invoke();
    }

    private void OnPlayerDeath(Player player) {

        canPause = false;
    }
}
