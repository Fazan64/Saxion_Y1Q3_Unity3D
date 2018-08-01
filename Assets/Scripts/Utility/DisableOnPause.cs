using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

/// Disables specified behaviours when the game is paused and 
/// enables them when the game is unpaused
public class DisableOnPause : MonoBehaviour {

    [SerializeField] Behaviour[] behaviours;

    void Awake() {

        GlobalEvents.OnGamePause.AddListener(OnPause);
        GlobalEvents.OnGameUnpause.AddListener(OnUnpause);
    }

    void OnDestroy() {

        GlobalEvents.OnGamePause.RemoveListener(OnPause);
        GlobalEvents.OnGameUnpause.RemoveListener(OnUnpause);
    }

    void OnPause() {

        foreach (Behaviour behaviour in behaviours) {
            behaviour.enabled = false;
        }
    }

    void OnUnpause() {

        foreach (Behaviour behaviour in behaviours) {
            behaviour.enabled = true;
        }
    }
}
