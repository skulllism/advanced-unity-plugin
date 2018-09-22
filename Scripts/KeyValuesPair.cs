using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class KeyValuesPair
    {
        public StringVariable key;

        public string[] stringValues;
        public int[] intValues;
        public float[] floatValues;
        public bool[] boolValues;
        public Vector2[] vector2Values;

        public KeyValuesPair(StringVariable key, string[] stringValues = null, int[] intValues = null, float[] floatValues = null, bool[] boolValues = null, Vector2[] vector2Values = null)
        {
            this.key = key;
            this.stringValues = stringValues;
            this.intValues = intValues;
            this.floatValues = floatValues;
            this.boolValues = boolValues;
            this.vector2Values = vector2Values;
        }
    }
}