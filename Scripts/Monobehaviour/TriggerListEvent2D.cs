using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerListEvent2D : MonoBehaviour
    {
        [TagSelector]
        public string[] valids;

        public GameObjectUnityEvent onEnter;
        public GameObjectUnityEvent onExit;
        public GameObjectUnityEvent onFirstEnter;
        public GameObjectUnityEvent onEmpty;

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
            onEnter.Invoke(gameObject);

            if (gameObjects.Count == 1)
                onFirstEnter.Invoke(gameObject);
        }

        private void Remove(GameObject gameObject)
        {
            Debug.Assert(gameObjects.Contains(gameObject));

            gameObjects.Remove(gameObject);
            onExit.Invoke(gameObject);

            if (gameObjects.Count <= 0)
                onEmpty.Invoke(gameObject);
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