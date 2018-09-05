using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class Worker : MonoBehaviour
    {
        public UnityEvent onStart;
        public UnityEvent onFinished;

        public Coroutine StartWork(Queue<IEnumerator> queue)
        {
            return StartCoroutine(WorkingRoutine(queue));
        }

        public Coroutine StartWork(params IEnumerator[] works)
        {
            Queue<IEnumerator> queue = new Queue<IEnumerator>();

            foreach (var work in works)
            {
                queue.Enqueue(work);
            }

            return StartWork(queue);
        }

        private IEnumerator WorkingRoutine(Queue<IEnumerator> queue)
        {
            //Debug.Log("start");
            onStart.Invoke();

            while (queue.Count > 0)
            {
                yield return StartCoroutine(queue.Dequeue());
                //Debug.Log("start new work");
            }

            //Debug.Log("finished");
            onFinished.Invoke();
        }
    }
}