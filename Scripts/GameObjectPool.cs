using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class GameObjectPool
    {
        public static event EventHandler onCreate;
        public static event EventHandler onGet;
        public static event EventHandler onPool;
        public static event EventHandler onDestroy;

        public delegate void EventHandler(GameObject gameObject);

        private GameObject origin;

        private List<GameObject> gameObjects = new List<GameObject>();

        public GameObjectPool(GameObject origin)
        {
            this.origin = origin;
        }

        public void CreateAndDisable(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateAndDisable();
            }
        }

        public virtual GameObject Create()
        {
            GameObject gameObject = Object.Instantiate(origin);
            gameObjects.Add(gameObject);

            if (onCreate != null)
                onCreate(gameObject);

            return gameObject;
        }

        public void CreateAndDisable()
        {
            GameObject created = Create();

            if (!created)
            {
                Debug.Log("[AGOC] Not Enough Pool Capacity : " + origin.name);
                return;
            }

            created.SetActive(false);
        }

        public GameObject Get()
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.activeSelf)
                    continue;

                gameObject.SetActive(true);

                if (onGet != null)
                    onGet(gameObject);

                return gameObject;
            }

            Debug.Log("[AGOC] Not Enough Pool : " + origin.name);
            return null;
        }

        public static void Disable(GameObject target)
        {
            if (!target)
                return;

            if (onPool != null)
                onPool(target);

            target.gameObject.SetActive(false);
        }

        public void DisableAll()
        {
            foreach (var gameObject in gameObjects)
            {
                Disable(gameObject);
            }
        }

        public void DestroyAll()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                Object.Destroy(gameObjects[i]);
            }

            gameObjects.Clear();
        }

        public void Destroy(GameObject target)
        {
            if (!gameObjects.Contains(target))
                return;

            if (onDestroy != null)
                onDestroy(target);

            Object.Destroy(target);
        }
    }

}