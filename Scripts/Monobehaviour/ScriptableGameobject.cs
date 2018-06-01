using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    /*
* @brief GameObject for scriptable components
* @author Kay
* @date 2018-05-31
* @version 0.0.1
* */
    public class ScriptableGameobject : MonoBehaviour
    {
        public class Packet
        {
            public string ID;
            public string message;
            public object[] args;
        }

        public ScriptableComponent[] components;

        private ScriptableComponent[] clones;
        private Queue<Packet> pendings = new Queue<Packet>();

        private void Awake()
        {
            clones = PrototypeScriptableObject.SetClones(this, components);
            RequestSendMessageBroadcast("TransitionToState", "1");
        }

        private void Update()
        {
            Handle();

            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].ManualUpdate(this);
            }
        }

        private void Handle()
        {
            if (pendings.Count == 0)
                return;

            Packet pending = pendings.Dequeue();

            if (pending.ID == null)
            {
                for (int i = 0; i < clones.Length; i++)
                {
                    clones[i].OnReceiveMessage(pending.message, pending.args);
                }
            }
            else
            {
                GetScriptableComponent(pending.ID).OnReceiveMessage(pending.message, pending.args);
            }
        }

        private ScriptableComponent GetScriptableComponent(string ID)
        {
            for (int i = 0; i < clones.Length; i++)
            {
                if (clones[i].ID == ID)
                    return clones[i];
            }

            Debug.Log("[AUP] Not Found Component");
            return null;
        }

        public void RequestSendMessageBroadcast(string message, params object[] args)
        {
            RequestSendMessageSinglecast(null, message, args);
        }

        public void RequestSendMessageSinglecast(string ID, string message, params object[] args)
        {
            Packet packet = new Packet();
            packet.ID = ID;
            packet.message = message;
            packet.args = args;

            pendings.Enqueue(packet);
        }

        public T GetScriptableComponent<T>() where T : ScriptableComponent
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetType() == typeof(T))
                    return components[i] as T;
            }

            Debug.Log("[AUP] Not Found Component");
            return null;
        }
    }
}