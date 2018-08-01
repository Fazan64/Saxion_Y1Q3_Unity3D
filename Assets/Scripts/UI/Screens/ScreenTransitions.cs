using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

[RequireComponent(typeof(TransitionableScreen))]
public class ScreenTransitions : MonoBehaviour {

    [System.Serializable]
    struct Transition {

        public KeyCode keyPress;
        public TransitionableScreen target;
    }
    
    [SerializeField] TransitionableScreen targetScreen;
    [SerializeField] Transition[] transitions;

    void Start() {

        targetScreen = targetScreen ?? GetComponent<TransitionableScreen>();
    }

    void Update() {

        if (!targetScreen.isCurrentlySelected)  return;
        if (!targetScreen.wasSelectedLastFrame) return;
        
        foreach (Transition transition in transitions) {

            if (Input.GetKeyDown(transition.keyPress)) {

                transition.target.TransitionTo();
                break;
            }
        }
    }
}
