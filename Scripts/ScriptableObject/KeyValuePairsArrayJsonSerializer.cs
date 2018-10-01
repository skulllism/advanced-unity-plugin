using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName ="AdvancedUnityPlugin/JsonSerializer/KeyValuePairsArray")]
    public class KeyValuePairsArrayJsonSerializer : ScriptableObject
    {
        public string Serialize(KeyValuePairsArrayVariable keyValuePairsVariable)
        {
            return new JsonSerializer<KeyValuePairsArray>().Serialize(keyValuePairsVariable.Get());
        }
    }
}