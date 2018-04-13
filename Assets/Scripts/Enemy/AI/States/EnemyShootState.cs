using UnityEngine;
using System.Collections;

public class EnemyShootState : AbstractState<Enemy> {

    float timeTillCanShoot = 0f;
    
    public override void Enter() {

        agent.navMeshAgent.isStopped = true;
        agent.material.color = Color.white;

        timeTillCanShoot = agent.aimingTime;

        base.Enter();
    }

    public override void Update() {

        if (agent.GetDistanceToPlayer() > agent.endShootDistance || !agent.CanShootAtPlayer()) {

            agent.fsm.ChangeState<EnemySeekPlayerState>();
            return;
        }

        if (timeTillCanShoot > 0f) {

            timeTillCanShoot -= Time.deltaTime;
            if (timeTillCanShoot <= 0f) {
                
                agent.shootingController.ShootAt(Player.instance.gameObject);
                timeTillCanShoot = agent.aimingTime;
            }
        }

        agent.material.color = 
            Color.Lerp(
                Color.white,
                Color.yellow, 
                (1f - timeTillCanShoot) * (1f - timeTillCanShoot)
            );
    }
}
