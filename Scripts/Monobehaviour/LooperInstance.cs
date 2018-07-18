using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class LooperInstance : MonoBehaviour
    {
        private Looper looper;

        public void Init(Looper looper)
        {
            this.looper = looper;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
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