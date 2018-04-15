using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDHint : HintTransition {

    [SerializeField] float delayAfterMovement = 1f;

    private bool didMove;

    void Update() {

        if (isTransitionedIn && !didMove && IsMoving()) {

            didMove = true;
            Invoke("TransitionOut", delayAfterMovement);
        }
    }

    private bool IsMoving() {

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.01f)   return true;
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) return true;
        return false;
    }
}
