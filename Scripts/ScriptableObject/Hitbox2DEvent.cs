using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    /*@brief global scriptable Hitbox2D Event
     * @detail notify hitbox2D event to listener
     * @author Kay
     * @date 2018-06-01
     * @version 0.0.1
     * */
    [CreateAssetMenu(fileName = "Hitbox 2D Event")]
    public class Hitbox2DEvent : ScriptableObject
    {
        public interface EventListener
        {
            void OnEnter(Collider2D enter, Collider2D host);
            void OnStay(Collider2D stay, Collider2D host);
            void OnExit(Collider2D exit, Collider2D host);
        }

        private List<EventListener> listeners = new List<EventListener>();

        public void RegistEventListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregistEventListener(EventListener listener)
        {
            listeners.Remove(listener);
        }

        public void OnEnter(Collider2D enter, Collider2D host)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEnter(enter, host);
            }
        }

        public void OnStay(Collider2D stay, Collider2D host)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnStay(stay, host);
            }
        }

        public void OnExit(Collider2D exit, Collider2D host)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnExit(exit, host);
            }
        }
    }
}