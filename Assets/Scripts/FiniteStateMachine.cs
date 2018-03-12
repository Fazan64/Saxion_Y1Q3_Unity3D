using System;
using System.Collections.Generic;

/// A simple stack-based finite state machine
public class FiniteStateMachine {

    Stack<Action> stack = new Stack<Action>();

    public FiniteStateMachine() {}
    public FiniteStateMachine(Action state) {
        PushState(state);
    }

    public void Update() {
        
        if (stack.Count > 0) {
            stack.Peek().Invoke();
        }
    }

    public void PushState(Action state) {
        stack.Push(state);
    }

    public void PopState() {
        stack.Pop();
    }
}

public static class FiniteStateMachineExtensions {

    /// A shortcut for PopState(); PushState(newState);
    public static void ReplaceState(this FiniteStateMachine fsm, Action state) {

        fsm.PopState();
        fsm.PushState(state);
    }
}
