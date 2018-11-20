using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TriggerListEvent2D : MonoBehaviour
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onFirstEnter;
        public UnityEvent onEmpty;

        [TagSelector]
        public string[] valids;

        private readonly List<GameObject> gameObjects = new List<GameObject>();

        private void Update()
        {
            if (gameObjects.Count <= 0)
                return;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].activeSelf)
                    continue;

                Remove(gameObjects[i]);
            }
        }

        private void Add(GameObject gameObject)
        {
            Debug.Assert(!gameObjects.Contains(gameObject));

            gameObjects.Add(gameObject);
            onEnter.Invoke();

            if (gameObjects.Count == 1)
                onFirstEnter.Invoke();
        }

        private void Remove(GameObject gameObject)
        {
            Debug.Assert(gameObjects.Contains(gameObject));

            gameObjects.Remove(gameObject);
            onExit.Invoke();

            if (gameObjects.Count <= 0)
                onEmpty.Invoke();
        }

        private bool IsValid(string tag)
        {
            if (valids.Length <= 0)
                return true;

            for (int i = 0; i < valids.Length; i++)
            {
                if (tag == valids[i])
                    return true;
            }

            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsValid(collision.tag))
                return;

            Add(collision.gameObject);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!IsValid(collision.tag))
                return;

            Remove(collision.gameObject);
        }
    }
}