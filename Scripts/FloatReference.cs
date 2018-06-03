using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdvancedUnityPlugin
{
    [Serializable]
    public class FloatReference
    {
        public bool useConstantValue = true;
        public float constantValue;
        public FloatVariable variable;

        public float Value
        {
            get
            {
                return useConstantValue ? constantValue : variable.initialValue;
            }
        }
    }

}