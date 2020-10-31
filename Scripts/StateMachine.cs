using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine
{
    private readonly List<IState> states = new List<IState>();

    private float duration;

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

    public IState Current { private set; get; }
    public IState Prev { private set; get; }

    public void TransitionToState(string ID)
    {
        if (Current != null)
        {
            Current.OnExit();
        }

        Prev = Current;
        Current = GetState(ID);
        //Debug.Log(Current.ID);

        Debug.Assert(Current != null, "Not Found : " + ID + " / Prev : " + Prev);
        Current.OnEnter();
        duration = 0;
    }

    private Stack<string> nexts = new Stack<string>();

    public void ManualUpdate()
    {
        if (Current == null)
            return;
        
        if (Current.IsTransition(out string transition))
        {
            Debug.Log(nexts.Count);
            if (nexts.Contains(transition))
            {
                return;
            }
            nexts.Push(transition);
            return;
        }

        Current.OnUpdate();
    }

    public void ManualLateUpdate()
    {
        if (Current == null)
            return;

        Current.OnLateUpdate();
    }

    public void ManualFixedUpdate()
    {
        if (Current == null)
            return;
        if(nexts.Count > 0)
        {
            TransitionToState(nexts.Pop());
            nexts.Clear();

        }
        Current.OnFixedUpdate();
    }

    private IState GetState(string ID)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == ID)
                return states[i];
        }

        Debug.Log("[StateMachine] Not found state ID : " + ID);
        return null;
    }
}
