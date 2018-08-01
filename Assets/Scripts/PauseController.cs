using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : Singleton<PauseController> {

    private bool isPaused;
    private float timeScaleBeforePause = 1f;

    private bool _canPause = true;
    public bool canPause { get { return _canPause; } }

    private bool _canUnpause = true;
    public bool canUnpause { get { return _canUnpause; } }

    void Start() {

        Player.instance.OnDeath += OnPlayerDeath;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            
            if (!isPaused && canPause) {
                Pause();
            } else if (isPaused && canUnpause) {
                Unpause();
            }
        }
    }

    void OnDestroy() {
        
        Unpause();
    }

    public void Pause() {

        if (isPaused) return;

        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;

        isPaused = true;
        GlobalEvents.OnGamePause.Invoke();
    }

    public void Unpause() {

        if (!isPaused) return;

        Time.timeScale = timeScaleBeforePause;
       
        isPaused = false;
        GlobalEvents.OnGameUnpause.Invoke();
    }

    public void SetCanPause(bool newCanPause) {
        
        _canPause = newCanPause;
    }

    public void SetCanUnpause(bool newCanUnpause) {

        _canUnpause = newCanUnpause;
    }

    private void OnPlayerDeath(Player player) {

        SetCanPause(false);
        SetCanUnpause(false);
    }
}
