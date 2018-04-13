using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM<AgentT> where AgentT : Component, IAgent {
    
    private readonly Dictionary<Type, FSMState<AgentT>> stateCache = new Dictionary<Type, FSMState<AgentT>>();

    private FSMState<AgentT> currentState;

    private readonly AgentT agent;
    private readonly Transform statesTransform;

    public FSM(AgentT agent) {
        
        this.agent = agent;
        statesTransform = FindOrMakeStatesTransform();
    }

    public FSMState<AgentT> GetCurrentState() {
        return currentState;
    }

    public void Reset() {

        if (currentState != null) {
            currentState.Exit();
            currentState.Enter();
        }
    }

    public void ChangeState<StateT>() where StateT : FSMState<AgentT> {
        
        //check if a state like this was already in our cache
        if (!stateCache.ContainsKey(typeof(StateT))) {

            //if not, create it, passing in the target
            StateT state = statesTransform.gameObject.AddComponent<StateT>();
            stateCache[typeof(StateT)] = state;
            state.SetAgent(agent);
        }

        FSMState<AgentT> newState = stateCache[typeof(StateT)];
        ChangeState(newState);
    }

    private void ChangeState(FSMState<AgentT> pNewState) {
        
        if (currentState == pNewState) return;

        if (currentState != null) currentState.Exit();
        currentState = pNewState;
        if (currentState != null) currentState.Enter();
    }

    private Transform FindOrMakeStatesTransform() {

        const string name = "FSMStates";

        Transform transform = agent.transform.Find(name);
        if (transform == null) {

            var newGameObject = new GameObject(name);
            newGameObject.transform.SetParent(agent.transform, worldPositionStays: false);
            return newGameObject.transform;
        }

        FSMState<AgentT>[] states = transform.GetComponentsInChildren<FSMState<AgentT>>();
        foreach (FSMState<AgentT> state in states) {
            
            stateCache.Add(state.GetType(), state);
            state.SetAgent(agent);

            if (currentState != null) {
                state.enabled = false;
            } else if (state.isActiveAndEnabled) {
                ChangeState(state);
            }
        }

        return transform;
    }
}
