using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/ObejctContainer")]
    public class ObjectContainer : ScriptableObject
    {
        public Object[] origins;

        public Object GetOrigin(string objectName)
        {
            for (int i = 0; i < origins.Length; i++)
            {
                if(origins[i].name == objectName)
                    return origins[i];
            }

            Debug.Log("[ObjectContainer] Not Found : " + objectName);
            return null;
        }
    }
}