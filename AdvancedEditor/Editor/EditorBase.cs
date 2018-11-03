using UnityEngine;

namespace AdvancedUnityPlugin.Editor
{
    public class EditorBase : UnityEditor.Editor
    {
        public void Space(float value)
        {
            GUILayout.Space(value);
        }
    }
}