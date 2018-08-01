using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

public class MouseHint : HintTransition {

    [SerializeField] float initialDelay = 0.5f;
    [SerializeField] float delayAfterMouseMovement = 1f;

    private bool didMouseMove;

    protected override void Start() {

        base.Start();
        
        Invoke("TransitionIn", initialDelay);
    }

    void Update() {
		
        if (isTransitionedIn && !didMouseMove && IsMouseMoving()) {
            
            didMouseMove = true;
            Invoke("TransitionOut", delayAfterMouseMovement);
        }
	}

    private bool IsMouseMoving() {

        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f) return true;
        if (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f) return true;
        return false;
    }
}
