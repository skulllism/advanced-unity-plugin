using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class UnityEventState : StateOld
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onUpdate;

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
