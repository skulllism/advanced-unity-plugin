using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    public abstract class Decision : ScriptableObject
    {
        public bool isTrue;
        public abstract bool Decide(Actor actor);
    }
}

