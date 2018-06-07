using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class Updater : GameEvent
    {
        public class Looper : MonoBehaviour
        {
            private GameEvent onUpdateEvent;

            public void Init(GameEvent onUpdateEvent)
            {
                this.onUpdateEvent = onUpdateEvent;
            }

            private void Update()
            {
                if (!onUpdateEvent)
                    onUpdateEvent.Raise();
            }
        }

        [SerializeField]
        private Looper looper;

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

