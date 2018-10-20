using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow
    {
        private const string PATH = "Assets/AdvancedStateMachineEditor/cache/";
        private void SaveData()
        {
            if (editorNodes == null || editorNodes.Count <= 0)
                return;

            NodeAsset asset = ScriptableObject.CreateInstance<NodeAsset>();
            asset.Save(editorNodes);

            AssetDatabase.CreateAsset(asset, PATH + Target.transform.root.name + ".asset");
        }

        private void LoadData()
        {

        }
    }
}