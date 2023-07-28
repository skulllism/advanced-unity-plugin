using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine
{
    public interface IEventHandler
    {
        void OnTransitionToState(IState prev ,IState next);
    }

    private readonly List<IState> states      = new List<IState>();
    private List<IEventHandler> eventHandlers = new List<IEventHandler>();

    public void ResisterEvent(IEventHandler eventHandler)
    {
        eventHandlers.Add(eventHandler);
    }

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

    public void Remove(string ID)
    {
        if(TryGetState(ID, out IState state))
        {
            Remove(state);
        }
    }

    public void Remove(IState state)
    {
        states.Remove(state);
    }

    public IState Current { private set; get; }
    public IState Prev { private set; get; }

    public void TransitionToState(string ID)
    {
        if (Current != null)
        {
            Current.OnExit(ID);
        }

        Prev = Current;
        if(!TryGetState(ID,out IState state))
        {
            Debug.LogError("Request From : " + Prev.ID);
            return;
        }

        Current = state;

        if (Prev != null)
            //Debug.Log("current : " + Current.ID + " / prev : " + Prev.ID);


        Debug.Assert(Current != null, "Not Found : " + ID + " / Prev : " + Prev);
        foreach (var  eventHandler in eventHandlers)
        {
            eventHandler.OnTransitionToState(Prev, Current);
        }
        Current.OnEnter();
    }

    public void ManualReset()
    {
        this.Current?.OnExit(null);
        this.Current = null;
        this.Prev = null;
    }

    public void ManualUpdate()
    {
        if (Current == null)
            return;

        if (Current.IsTransition(out string transition))
        {
            TransitionToState(transition);

            //Debug.Log(nexts.Count);
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

        Current.OnFixedUpdate();
    }

   
    private bool TryGetState(string ID, out IState state)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].ID == ID)
            {
                state = states[i];
                return true;
            }
        }

        Debug.Log("[StateMachine] Not found state ID : " + ID);
        state = null;
        return false;
    }

    
}
