using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

public class JumpHint : HintTransition {

    [SerializeField] float delayAfterJump = 1f;
    [SerializeField] TriggerEvents trigger;

    private bool didJump;

    protected override void Start() {

        base.Start();

        Assert.IsNotNull(trigger);
        trigger.onPlayerTriggerEnter.AddListener(OnPlayerTriggerEnter);
    }

    void OnDestroy() {

        trigger.onPlayerTriggerEnter.RemoveListener(OnPlayerTriggerEnter);
    }

    void Update() {

        if (isTransitionedIn && !didJump && IsJumping()) {

            trigger.onPlayerTriggerEnter.RemoveListener(OnPlayerTriggerEnter);
            didJump = true;
            Invoke("TransitionOut", delayAfterJump);
        }
    }

    private void OnPlayerTriggerEnter() {

        if (!isTransitionedIn) {
            TransitionIn();
        }
    }

    private bool IsJumping() {

        return Input.GetKeyDown(KeyCode.Space);
    }
}
