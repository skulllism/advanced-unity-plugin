using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class GameObjectPool
    {
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

                return gameObject;
            }

            Debug.Log("[AGOC] Not Enough Pool : " + origin.name);
            return null;
        }

        public static void Disable(GameObject target)
        {
            if (!target)
                return;

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
            foreach (var item in gameObjects)
            {
                Object.Destroy(item);
            }

            gameObjects.Clear();
        }

        public void Destroy(GameObject target)
        {
            if (!gameObjects.Contains(target))
                return;

            Object.Destroy(target);
        }
    }

}