using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Looper")]
    public class Looper : ScriptableObject
    {
        public GameEvent onUpdate;
        public GameEvent onFixedUpdate;
        public GameEvent onLateUpdate;

        public bool isLooping = true;

        public LooperInstance monoBehaviour { set; get; }
    }
}

