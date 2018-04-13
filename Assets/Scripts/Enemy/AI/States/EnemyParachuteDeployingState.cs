using UnityEngine;
using System.Collections;

#pragma warning disable 0649

public class EnemyParachuteDeployingState : AbstractState<Enemy> {

    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckRadius = 1f;

    public override void Enter() {

        base.Enter();

        agent.rigidbody.isKinematic = false;
        agent.navMeshAgent.enabled  = false;
    }

    public override void Update() {
        
        base.Update();

        bool isLanded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayerMask);
        if (isLanded) {
            
            DetachParachuteIfAttached();
            agent.fsm.ChangeState<EnemySeekPlayerState>();
        }
    }

    public override void Exit() {
        
        base.Exit();

        agent.rigidbody.isKinematic = true;
        agent.navMeshAgent.enabled  = true;
    }

    private void DetachParachuteIfAttached() {

        var parachute = agent.GetComponentInChildren<Parachute>();
        if (parachute == null) return;

        parachute.Detach();
    }
}