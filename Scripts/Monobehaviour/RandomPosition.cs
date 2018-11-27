using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class RandomPosition : MonoBehaviour
    {
        public Transform target;
        public UnityEvent onSetPosition;

        private static bool isComplete;
        private static RandomPosition selected;

        private void Start()
        {
            if (isComplete)
                return;

            if (!selected)
            {
                var positions = FindObjectsOfType<RandomPosition>();
                int index = UnityEngine.Random.Range(0, positions.Length);
                selected = positions[index];
            }

            if (selected != this)
                return;

            target.position = transform.position;
            onSetPosition.Invoke();
            isComplete = true;
        }
    }
}


