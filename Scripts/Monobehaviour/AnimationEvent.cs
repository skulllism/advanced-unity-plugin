using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class AnimationEvent : MonoBehaviour
    {
        public string animationName;
        public KeyframeActionEvent[] keyframeEvents;
        public TermActionEvent[] termEvents;
    }
}