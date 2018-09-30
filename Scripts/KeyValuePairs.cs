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
            public StringVariable key;
            public string value;

            public StringPair(StringVariable key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct IntPair
        {
            public StringVariable key;
            public int value;

            public IntPair(StringVariable key, int value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct FloatPair
        {
            public StringVariable key;
            public float value;

            public FloatPair(StringVariable key, float value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct BoolPair
        {
            public StringVariable key;
            public bool value;

            public BoolPair(StringVariable key, bool value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [Serializable]
        public struct Vector2Pair
        {
            public StringVariable key;
            public Vector2 value;

            public Vector2Pair(StringVariable key, Vector2 value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public StringVariable key;

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
