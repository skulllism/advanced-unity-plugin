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

        public bool HasBeenTriggered()
        {
            return gameObjects.Count > 0;
        }

        public bool HasBeenTriggered(GameObject target)
        {
            return gameObjects.Contains(target);
        }

        public bool Contains(string tag, out GameObject gameObject)
        {
            gameObject = null;
            foreach(var iter in gameObjects)
            {
                if(iter.tag == tag)
                {
                    gameObject = iter;
                    break;
                }
            }

            return gameObject != null ? true : false;
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
            if (!gameObjects.Contains(gameObject))
                return;

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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!IsValid(collision.tag))
                return;

            if (gameObjects.Contains(collision.gameObject))
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