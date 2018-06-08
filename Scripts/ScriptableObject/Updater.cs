using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Updater : GameEvent
    {
        public class Looper : MonoBehaviour
        {
            private Updater updater;

            public void Init(Updater updater)
            {
                this.updater = updater;
            }

            private void Update()
            {
                if (!updater.isLooping)
                    return;

                if (!updater)
                    updater.Raise();
            }
        }

        public Looper looper;
        public bool isLooping = true;

        private Looper CreateLooper()
        {
            Looper tmp = new GameObject("Looper").AddComponent<Looper>();
            DontDestroyOnLoad(tmp.gameObject);
            return tmp;
        }

        public override void RegisterListener(Listener listener)
        {
            if (!looper)
            {
                looper = CreateLooper();
                looper.Init(this);
            }

            base.RegisterListener(listener);
        }
    }
}

