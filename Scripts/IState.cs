using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class IState
{
    public abstract string ID { get; }

    public virtual float MinDuration => 0;

    public abstract bool IsTransition(out string ID);

    public abstract void OnEnter();

    public abstract void OnFixedUpdate();

    public abstract void OnUpdate();

    public abstract void OnLateUpdate();

    public abstract void OnExit();
}