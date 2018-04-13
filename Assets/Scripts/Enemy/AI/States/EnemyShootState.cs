using UnityEngine;
using System.Collections;

#pragma warning disable 0649

public class EnemyShootState : FSMState<Enemy> {

    [SerializeField][Range(0f, 1f)] float dodgeChance = 0.5f;

    private float timeTillCanShoot = 0f;
    
    public override void Enter() {

        agent.navMeshAgent.isStopped = true;
        agent.material.color = Color.white;

        timeTillCanShoot = agent.aimingTime;

        base.Enter();
    }

    public override void Exit() {
        
        base.Exit();

        agent.material.color = Color.white;
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
                if (Random.value <= dodgeChance) {
                    agent.fsm.ChangeState<EnemyDodgeState>();
                    return;
                }

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
