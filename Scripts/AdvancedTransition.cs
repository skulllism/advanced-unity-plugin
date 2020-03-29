using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdvancedTransition
{
    public string ID;
    public string stateID;

    public abstract bool IsTransition();
}