using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState[] states;

    public StateMachine(IState[] states)
    {
        this.states = states;
    }

    public IState current { private set; get; }

    public void TransitionToState(string ID)
    {
        if (current != null)
        {
            current.OnExit();
        }

        current = GetState(ID);

        Debug.Assert(current != null, ID);
        current.OnEnter();
    }

    protected virtual void ManualUpdate()
    {
        if (current == null)
            return;

        string transition = null;

        if (current.IsTransition(out transition))
        {
            TransitionToState(transition);
            return;
        }

        current.OnUpdate();
    }

    private void ManualLateUpdate()
    {
        if (current == null)
            return;

        current.OnLateUpdate();
    }

    private void ManualFixedUpdate()
    {
        if (current == null)
            return;

        current.OnFixedUpdate();
    }

    private IState GetState(string ID)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].ID == ID)
                return states[i];
        }

        Debug.Log("Not found ID : " + ID);
        return null;
    }
}
