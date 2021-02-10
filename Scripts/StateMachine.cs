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

    private float duration;
   
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

    public void SetInitState(string ID)
	{
        InitState = ID;
    }

    public IState Current { private set; get; }
    public IState Prev { private set; get; }
    public string InitState { private set; get; }

    public void TransitionToState(string ID)
    {
        if (Current != null)
        {
            Current.OnExit();
        }

        Prev = Current;
        Current = GetState(ID);

        //if(Prev!=null)
        //Debug.Log("current : "+ Current.ID + " / prev : " + Prev.ID);
        

        Debug.Assert(Current != null, "Not Found : " + ID + " / Prev : " + Prev);
        foreach (var  eventHandler in eventHandlers)
        {
            eventHandler.OnTransitionToState(Prev, Current);
        }
        Current.OnEnter();
        
        duration = 0;
    }

    public void ManualUpdate()
    {
        if (Current == null)
            return;
        
        if (duration >= Current.MinDuration && Current.IsTransition(out string transition))
        {
            TransitionToState(transition);

            //Debug.Log(nexts.Count);
            return;
        }

        Current.OnUpdate();
        duration += Time.deltaTime;
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

    public void ResetState()
	{
        TransitionToState(InitState);


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
