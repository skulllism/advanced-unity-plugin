using UnityEngine;

namespace AdvancedUnityPlugin
{
    public abstract class StateOld : MonoBehaviour
    {
        public string id;

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnExit();
    }
}