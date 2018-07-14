using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class UnityEventState : State
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onUpdate;

        public override bool IsTransition(out string next)
        {
            next = "";
            return false;
        }

        public override void OnEnter()
        {
            onEnter.Invoke();
        }

        public override void OnExit()
        {
            onExit.Invoke();
        }

        public override void OnUpdate()
        {
            onUpdate.Invoke();
        }
    }
}
