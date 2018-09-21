using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class KeyValuePairs
    {
        [Serializable]
        public struct StringPair
        {
            public string key;
            public string value;

            public StringPair(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct IntPair
        {
            public string key;
            public int value;

            public IntPair(string key, int value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct FloatPair
        {
            public string key;
            public float value;

            public FloatPair(string key, float value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct BoolPair
        {
            public string key;
            public bool value;

            public BoolPair(string key, bool value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct Vector2Pair
        {
            public string key;
            public Vector2 value;

            public Vector2Pair(string key, Vector2 value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public StringPair[] stringPairs;
        public IntPair[] intPairs;
        public FloatPair[] floatPairs;
        public BoolPair[] boolPairs;
        public Vector2Pair[] vector2Pairs;

        public KeyValuePairs(StringPair[] stringPairs = null, IntPair[] intPairs = null, FloatPair[] floatPairs = null, BoolPair[] boolPairs = null, Vector2Pair[] vector2Pairs = null)
        {
            this.stringPairs = stringPairs;
            this.intPairs = intPairs;
            this.floatPairs = floatPairs;
            this.boolPairs = boolPairs;
            this.vector2Pairs = vector2Pairs;
        }
    }
}
