using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class Pool
    {
        public GameObject origin;
        public int count;

        private List<GameObject> gameObjects = new List<GameObject>();

        public void Init()
        {
            Debug.Assert(origin);
            Debug.Assert(count > 0);

            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(origin);
                gameObject.SetActive(false);
                gameObjects.Add(gameObject);
            }
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

        public void PoolAll()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            DestroyAll();
            Init();
        }

        public void DestroyAll()
        {
            foreach (var item in gameObjects)
            {
                GameObject.Destroy(item);
            }

            gameObjects.Clear();
        }
    }

}