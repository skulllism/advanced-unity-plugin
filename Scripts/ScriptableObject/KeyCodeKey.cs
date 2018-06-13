using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu]
    public class KeyCodeKey : Input.Key
    {
        public string[] buttons;
        public KeyCode[] keyCodes;

        public override bool GetKeyDown()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (UnityEngine.Input.GetButtonDown(buttons[i]))
                    return true;
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(keyCodes[i]))
                    return true;
            }

            return false;
        }

        public override bool GetKeyUp()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (UnityEngine.Input.GetButtonUp(buttons[i]))
                    return true;
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (UnityEngine.Input.GetKeyUp(keyCodes[i]))
                    return true;
            }

            return false;
        }
    }
}