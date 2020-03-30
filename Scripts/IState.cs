using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    string ID { get; }

    bool IsTransition(out string ID);

    void OnEnter();

    void OnFixedUpdate();

    void OnUpdate();

    void OnLateUpdate();

    void OnExit();
}