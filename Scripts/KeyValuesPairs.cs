using System;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class KeyValuesPairs
    {
        public KeyValuesPair[] pairs;

        public KeyValuesPairs(KeyValuesPair[] pairs)
        {
            this.pairs = pairs;
        }
            
    }
}