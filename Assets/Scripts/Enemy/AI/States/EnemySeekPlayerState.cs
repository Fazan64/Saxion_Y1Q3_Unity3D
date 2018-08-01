using UnityEngine;
using System.Collections;

public class EnemySeekPlayerState : FSMState<Enemy> {

    public override void Enter() {

        base.Enter();

        agent.navMeshAgent.enabled = true;
        agent.navMeshAgent.isStopped = false;
    }

    public override void Update() {
        
        base.Update();
        if (agent.GetDistanceToPlayer() <= agent.startShootDistance && agent.shootingController.CanShootAt(Player.instance.gameObject)) {

            agent.fsm.ChangeState<EnemyShootState>();
            return;
        }

        agent.navMeshAgent.destination = Player.instance.transform.position;
    }
}
