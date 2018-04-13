using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSM<AgentT> where AgentT : Component, IAgent {
    
    //maps the class name of a state to a specific instance of that state
    private readonly Dictionary<Type, AbstractState<AgentT>> stateCache = new Dictionary<Type, AbstractState<AgentT>>();

    //the current state we are in
    [SerializeField] private AbstractState<AgentT> currentState;

    //reference to our target so we can pass into our new states.
    private readonly AgentT agent;
    private readonly Transform statesTransform;

    public FSM(AgentT agent) {
        
        this.agent = agent;
        statesTransform = FindOrMakeStatesTransform();
    }

    public void Update() {

        if (currentState != null)
            currentState.Update();
    }

    public AbstractState<AgentT> GetCurrentState() {
        return currentState;
    }

    public void Reset() {

        if (currentState != null) {
            currentState.Exit();
            currentState.Enter();
        }
    }

    /**
	 * Tells the FSM to enter a state which is a subclass of AbstractState<T>.
	 * So for exampe for FSM<Bob> the state entered must be a subclass of AbstractState<Bob>
	 */
    public void ChangeState<StateT>() where StateT : AbstractState<AgentT> {
        
        //check if a state like this was already in our cache
        if (!stateCache.ContainsKey(typeof(StateT))) {

            //if not, create it, passing in the target
            StateT state = statesTransform.gameObject.AddComponent<StateT>();
            stateCache[typeof(StateT)] = state;
            state.SetAgent(agent);
        }

        AbstractState<AgentT> newState = stateCache[typeof(StateT)];
        ChangeState(newState);
    }

    private void ChangeState(AbstractState<AgentT> pNewState) {
        
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

        AbstractState<AgentT>[] states = transform.GetComponentsInChildren<AbstractState<AgentT>>();
        foreach (AbstractState<AgentT> state in states) {
            
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
