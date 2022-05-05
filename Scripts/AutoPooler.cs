using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace AdvancedUnityPlugin
{
    public class AutoPooler : MonoBehaviour
    {
        [System.Serializable]
        public struct TermData
        {
            public bool  isUse;
            public float value;
        }

        public TermData lifeTime;
        public TermData lifeDistance;
        [Space(20)]
        public UnityEvent onAutoPool;

        private bool isRun;
        private void OnEnable()
        {
            Reset();
            Run();
        }

        private void OnDisable()
        {
            Pool();
        }

        private void Run()
        {
            if (isRun)
                return;

            isRun = true;

            if (lifeTime.isUse)
                StartCoroutine(WaitForLifeTime());
            if (lifeDistance.isUse)
                StartCoroutine(WaitForDistance());
        }

        private void Reset()
        {
            isRun = false;
            transform.localPosition = Vector3.zero;

            //StartCoroutine(WaitForFrame());
        }

        private void Pool()
        {
            onAutoPool.Invoke();

            Reset();
            gameObject.SetActive(false);
        }

        private IEnumerator WaitForLifeTime()
        {
            float time = 0.0f;
            while(isRun)
            {
                yield return null;

                time += Time.deltaTime;
                if (time >= lifeTime.value)
                {
                    Pool();
                    break;
                }
            }
        }

        private IEnumerator WaitForDistance()
        {
            Vector3 originDistance = transform.position;
            while(isRun)
            {
                yield return null;

                float distance = Vector3.Distance(transform.position, originDistance);
                if(Mathf.Abs(distance) >= Mathf.Abs(lifeDistance.value))
                {
                    Pool();
                    break;
                }
            }
        }
    }
}

