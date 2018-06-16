using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Command : PrototypeScriptableObject
    {
        public Action[] onCommand;

        public override PrototypeScriptableObject Clone()
        {
            return Instantiate(this);
        }

        public void Execute(GameObject gameObject)
        {
            for (int i = 0; i < onCommand.Length; i++)
            {
                onCommand[i].OnAction(gameObject);
            }
        }

        public override void Init(GameObject gameObj)
        {   
            for (int i = 0; i < onCommand.Length; i++)
            {
                onCommand[i].Init(gameObj);
            }
        }
    }
}

