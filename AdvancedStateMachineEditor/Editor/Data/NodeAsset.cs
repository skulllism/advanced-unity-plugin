using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin.Editor
{
    public class NodeAsset : ScriptableObject
    {
        public class AssetData
        {
            public AdvancedStateMachineEditorWindow.NodeType type;
            public Rect rect;
        }

        [HideInInspector]
        public List<AssetData> datas = new List<AssetData>();

        public void Save(List<EditorNode<AdvancedStateMachineEditorWindow.NodeData>> nodes)
        {
            datas.Clear();

            for (int i = 0; i < nodes.Count; i++)
            {
                AssetData data = new AssetData();
                data.type = nodes[i].myData.type;
                data.rect = nodes[i].rect;

                datas.Add(data);
            }
        }
    }    
}