using UnityEngine;
using System.Collections;

public class EnemySeekPlayerState : AbstractState<Enemy> {

    public override void Enter() {

        base.Enter();

        agent.navMeshAgent.isStopped = false;
        agent.material.color = Color.white;
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
