using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Looper")]
    public class Looper : ScriptableObject
    {
        public GameEvent onUpdate;
        public GameEvent onFixedUpdate;
        public GameEvent onLateUpdate;

        public GameObject origin;

        public bool isLooping = true;

        public LooperInstance monoBehaviour
        {
            get
            {
                if (instance == null)
                {
                    instance = Instantiate(origin).GetComponent<LooperInstance>();
                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }

        private LooperInstance instance = null;
    }
}

