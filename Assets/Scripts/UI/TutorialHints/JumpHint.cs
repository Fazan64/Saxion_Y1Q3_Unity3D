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
        trigger.onTriggerEnter.AddListener(OnTargetTriggerEnter);
    }

    void OnDestroy() {

        trigger.onTriggerEnter.RemoveListener(OnTargetTriggerEnter);
    }

    void Update() {

        if (isTransitionedIn && !didJump && IsJumping()) {

            trigger.onTriggerEnter.RemoveListener(OnTargetTriggerEnter);
            didJump = true;
            Invoke("TransitionOut", delayAfterJump);
        }
    }

    private void OnTargetTriggerEnter(Collider collidee) {

        if (isTransitionedIn) return;
        if (collidee.attachedRigidbody.gameObject != Player.instance.gameObject) return;

        TransitionIn();
    }

    private bool IsJumping() {

        return Input.GetKeyDown(KeyCode.Space);
    }
}
