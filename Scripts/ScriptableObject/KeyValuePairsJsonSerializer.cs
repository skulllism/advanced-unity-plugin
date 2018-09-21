using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName ="AdvancedUnityPlugin/JsonSerializer/KeyValuePairs")]
    public class KeyValuePairsJsonSerializer : ScriptableObject
    {
        public string Serialize(KeyValuePairsVariable keyValuePairsVariable)
        {
            return new JsonSerializer<KeyValuePairs>().Serialize(keyValuePairsVariable.Get());
        }
    }
}