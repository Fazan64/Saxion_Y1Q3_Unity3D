using System;
using UnityEngine;
using UnityEngine.Assertions;

public class AbstractState<T> : MonoBehaviour where T : class, IAgent {
    
    protected T agent;

    public void SetAgent(T agent) {

        Assert.IsNull(this.agent);
        this.agent = agent;
    }

    public virtual void Enter() {
        
        //agent.Print("entered state:" + this);
        enabled = true;
    }

    public virtual void Update() {
        
        //agent.Print("updating state:" + this);
    }

    public virtual void Exit() {
        
        //agent.Print("exited state:" + this);
        enabled = false;
    }
}


