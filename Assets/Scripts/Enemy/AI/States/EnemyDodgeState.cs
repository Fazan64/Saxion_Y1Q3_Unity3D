using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 0649

public class EnemyDodgeState : FSMState<Enemy> {

    [SerializeField] float minRange = 1f;
    [SerializeField] float maxRange = 2f;
    [SerializeField][Range(0f, 180f)] float randomizationAngle = 60f;

    public override void Enter() {

        base.Enter();

        agent.navMeshAgent.enabled = true;
        agent.navMeshAgent.destination = CalculateDestination();
        agent.navMeshAgent.isStopped = false;
    }

    public override void Update() {
        
        base.Update();

        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance) {
            
            agent.fsm.ChangeState<EnemySeekPlayerState>();
        }
    }

    private Vector3 CalculateDestination() {
        
        Vector3 ownPosition = transform.position;

        Vector3 toPlayer = Player.instance.transform.position - ownPosition;
        toPlayer = Vector3.ProjectOnPlane(toPlayer, Vector3.up);
        Vector3 sideways = Vector3.Cross(Vector3.up, toPlayer).normalized;

        float sign = Random.value > 0.5f ? 1f : -1f;
        Vector3 delta = sideways * sign * Random.Range(minRange, maxRange);

        float rotationAngle = Random.Range(-randomizationAngle / 2f, randomizationAngle / 2f);
        delta = Quaternion.AngleAxis(rotationAngle, Vector3.up) * delta;

        Vector3 targetPosition = ownPosition + delta;

        NavMeshHit hit;
        if (agent.navMeshAgent.Raycast(targetPosition, out hit)) {
            targetPosition = hit.position;
        }

        return targetPosition;
    }
}
