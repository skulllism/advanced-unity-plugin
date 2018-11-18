//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace AdvancedUnityPlugin.Editor
//{
//    public class AdvancedStateMachineEditorData : ScriptableObject
//    {
//        [System.Serializable]
//        public class NodeData
//        {
//            public string name;
//            public Rect rect;
//        }

//        [HideInInspector]
//        public List<NodeData> datas = new List<NodeData>();
//        [HideInInspector]
//        public float zoomscale = 0.0f;
//        [HideInInspector]
//        public Vector2 zoomCoordsOrigin = Vector2.zero;

//        public void Save(List<EditorNode<AdvancedStateMachineEditorWindow.NodeData>> nodes)
//        {
//            int saveCount = 0;
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                if (datas.Count - 1 < i)
//                {
//                    NodeData data = new NodeData();

//                    if (nodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.STATE)
//                    {
//                        data.name = "S_" + nodes[i].myData.state.ID;
//                    }
//                    else if (nodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
//                    {
//                        data.name = "T_" + nodes[i].myData.transition.ID;
//                    }

//                    data.rect = nodes[i].rect;

//                    datas.Add(data);
//                }
//                else
//                {
//                    if (nodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.STATE)
//                    {
//                        datas[i].name = "S_" + nodes[i].myData.state.ID;
//                    }
//                    else if (nodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
//                    {
//                        datas[i].name = "T_" + nodes[i].myData.transition.ID;
//                    }

//                    datas[i].rect = nodes[i].rect;
//                }

//                saveCount = i;
//            }

//            for (int i = saveCount + 1; i < datas.Count; i++)
//            {
//                datas.RemoveAt(i);
//            }

//            AssetDatabase.SaveAssets();
//            AssetDatabase.Refresh();
//        }

//        public void Load(List<EditorNode<AdvancedStateMachineEditorWindow.NodeData>> nodes)
//        {
//            if (datas == null || datas.Count <= 0)
//                return;

//            int nodeCount = nodes.Count - 1;
//            string name = string.Empty;
//            for (int i = 0; i < datas.Count; i++)
//            {
//                for (int j = 0; j < nodes.Count; j++)
//                {
//                    if (nodes[j].myData.type == AdvancedStateMachineEditorWindow.NodeType.STATE)
//                    {
//                        name = "S_" + nodes[j].myData.state.ID;
//                    }
//                    else if (nodes[j].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
//                    {
//                        name = "T_" + nodes[j].myData.transition.ID;
//                    }

//                    if (datas[i].name == name)
//                    {
//                        nodes[j].rect = datas[i].rect;
//                        break;
//                    }
//                }
//            }
//        }
//    }
//}