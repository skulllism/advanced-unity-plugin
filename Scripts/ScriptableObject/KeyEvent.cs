using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Event/Key")]
    public class KeyEvent : GameEvent
    {
        public Input.Key key;
    }
}
