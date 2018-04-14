using UnityEngine;
using System.Collections;

public class EnemyDeadState : FSMState<Enemy> {

    public override void Enter() {

        base.Enter();

        agent.rigidbody.isKinematic = false;
        agent.navMeshAgent.enabled = false;
        StartFadeout();
    }

    private void StartFadeout() {

        var fadeout =
                agent.GetComponent<BodyFadeout>() ??
                agent.gameObject.AddComponent<BodyFadeout>();

        fadeout.enabled = true;
    }
}
