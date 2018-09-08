using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class Worker : MonoBehaviour
    {
        public StringUnityEvent onStart;
        public StringUnityEvent onFinished;

        private Dictionary<string, Queue<IEnumerator>> currents = new Dictionary<string, Queue<IEnumerator>>();

        public Coroutine StartWork(string name, Queue<IEnumerator> queue)
        {
            if(currents.ContainsKey(name))
            {
                while(queue.Count > 0)
                {
                    currents[name].Enqueue(queue.Dequeue());
                }

                Debug.Log(" [continue] / " + currents[name].Count);
                return null;
            }

            return StartCoroutine(WorkingRoutine(name , queue));
        }

        public Coroutine StartWork(string name , params IEnumerator[] works)
        {
            Queue<IEnumerator> queue = new Queue<IEnumerator>();

            foreach (var work in works)
            {
                queue.Enqueue(work);
            }

            return StartWork(name , queue);
        }

        private IEnumerator WorkingRoutine(string name , Queue<IEnumerator> queue)
        {
            currents.Add(name, queue);
            onStart.Invoke(name);
            Debug.Log(name + " [added] / " + currents.Count);

            while (queue.Count > 0)
            {
                yield return StartCoroutine(queue.Dequeue());
                //Debug.Log("start new work");
            }

            currents.Remove(name);
            onFinished.Invoke(name);
            Debug.Log(name + " [removed] / " + currents.Count);
        }
    }
}