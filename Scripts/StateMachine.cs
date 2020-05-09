using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine
{
    private readonly List<IState> states = new List<IState>();

    public StateMachine(params IState[] states)
    {
        AddStates(states);
    }

    public void AddStates(params IState[] states)
    {
        foreach (var state in states)
        {
            this.states.Add(state);
        }
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

    public void ManualUpdate()
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

    public void ManualLateUpdate()
    {
        if (current == null)
            return;

        current.OnLateUpdate();
    }

    public void ManualFixedUpdate()
    {
        if (current == null)
            return;

        current.OnFixedUpdate();
    }

    private IState GetState(string ID)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == ID)
                return states[i];
        }

        Debug.Log("Not found ID : " + ID);
        return null;
    }
}
