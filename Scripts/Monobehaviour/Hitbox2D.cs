using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    /*@brief the trigger of Hitbox2D
   * @detail trigging collider2D events
   * @author Kay
   * @date 2018-06-01
   * @version 0.0.1
   * */
    [RequireComponent(typeof(Collider2D))]
    public class Hitbox2D : MonoBehaviour
    {
        public interface EventListener
        {
            void OnEnter(Collider2D enter);
            void OnStay(Collider2D stay);
            void OnExit(Collider2D exit);
        }

        public UnityEvent onEnter;
        public UnityEvent onStay;
        public UnityEvent onExit;

        private List<EventListener> listeners = new List<EventListener>();

        public void RegistEventListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregistEventListener(EventListener listener)
        {
            listeners.Remove(listener);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnEnter(collision);
            }

            onEnter.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnStay(collision);
            }

            onStay.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].OnExit(collision);
            }

            onExit.Invoke();
        }
    }
}

