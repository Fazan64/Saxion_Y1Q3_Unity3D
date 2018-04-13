using UnityEngine;
using System.Collections;

public class EnemyDeadState : FSMState<Enemy> {

    public override void Enter() {

        agent.rigidbody.isKinematic = false;
        agent.navMeshAgent.enabled = false;

        base.Enter();
    }
}
