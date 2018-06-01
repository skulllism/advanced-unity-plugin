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

            public const int MAX_PENDING_SIZE = 99999999;

            public ScriptableComponent[] components;

            private ScriptableComponent[] clones;

            private Packet[] pendings = new Packet[MAX_PENDING_SIZE];
            private int head = 0;
            private int tail = 0;

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
                if (head == tail)
                    return;

                if (pendings[head].ID == null)
                {
                    for (int i = 0; i < clones.Length; i++)
                    {
                        clones[i].OnReceiveMessage(pendings[head].message, pendings[head].args);
                    }
                }
                else
                {
                    GetScriptableComponent(pendings[head].ID).OnReceiveMessage(pendings[head].message, pendings[head].args);
                }

                head = (head + 1) % MAX_PENDING_SIZE;
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
                Debug.Assert((tail + 1) % MAX_PENDING_SIZE != head);
                Packet packet = new Packet();
                packet.ID = ID;
                packet.message = message;
                packet.args = args;
                pendings[tail] = packet;
                tail = (tail + 1) % MAX_PENDING_SIZE;
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