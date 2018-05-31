using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Advanced
{
    public abstract class Action : ScriptableObject
    {
        public abstract void OnAction(Actor actor);
    }
}
