using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class LooperInstance : MonoBehaviour
    {
        public Looper looper;
        public Worker worker;

        private void Awake()
        {
            looper.monoBehaviour = this;
        }

        private void FixedUpdate()
        {
            if (!looper.isLooping)
                return;
            looper.onFixedUpdate.Raise();
        }

        private void LateUpdate()
        {
            if (!looper.isLooping)
                return;
            looper.onLateUpdate.Raise();
        }

        private void Update()
        {
            if (!looper.isLooping)
                return;
            looper.onUpdate.Raise();
        }
    }
}