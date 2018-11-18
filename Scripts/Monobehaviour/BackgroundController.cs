using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class BackgroundController : MonoBehaviour
    {
        public GameObject backgrounds;

        [TagSelector]
        public string[] valids;

        private readonly List<GameObject> gameObjects = new List<GameObject>();

        private void Start()
        {
            backgrounds.SetActive(false);
        }

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

            if (backgrounds.activeSelf)
                return;

            backgrounds.SetActive(true);
        }

        private void Remove(GameObject gameObject)
        {
            Debug.Assert(gameObjects.Contains(gameObject));

            gameObjects.Remove(gameObject);

            if (gameObjects.Count <= 0)
                backgrounds.SetActive(false);
        }

        private bool IsValid(string tag)
        {
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