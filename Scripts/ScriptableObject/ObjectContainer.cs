using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class ObjectContainer : ScriptableObject
    {
        public Object[] origins;

        public Object GetOrigin(string objectName)
        {
            for (int i = 0; i < origins.Length; i++)
            {
                if(origins[i].name == name)
                    return origins[i];
            }

            Debug.Log("[ObjectContainer] Not Found : " + name);
            return null;
        }
    }
}