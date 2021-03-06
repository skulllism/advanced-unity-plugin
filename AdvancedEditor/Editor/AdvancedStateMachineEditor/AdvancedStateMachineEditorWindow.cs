﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow : EditorWindow
    {
        public static AdvancedStateMachineEditorWindow Instance;

        public enum NodeType { STATE, TRANSITION }
        public class NodeData
        {
            public NodeType type;
            public AdvancedState state;
            public AdvancedTransition transition;

            public NodeData(NodeType type, AdvancedState state, AdvancedTransition transition)
            {
                this.type = type;
                this.state = state;
                this.transition = transition;
            }
        }

        //======================
        //## Editor View
        //======================
        public ASMPropertiesView propertiesView;
        public ASMWorkView workView;
        public ASMStateModifyView stateModifyView;
        public ASMTransitionModifyView transitionModifyView;
        //======================
        //## Property Data
        //======================
        private SerializedObject   serializedObject;
        private SerializedProperty propertyStates;
        private SerializedProperty propertyTransitions;

        public const float NODE_WIDTH = 180.0f;
        public const float NODE_HEIGHT = 40.0f;

        public EditorNode<NodeData> selectNode;

        public string[] stateNames = new string[50];

        public static void OpenWindow(AdvancedStateMachine data)
        {
            Instance = GetWindow<AdvancedStateMachineEditorWindow>();
            Instance.titleContent = new GUIContent("AdvancedStateMachine");

            if (data != null)
            {
                Instance.InitializeView();

                Instance.selected = data;
                Instance.InitializePropertyData();
                {
                    Instance.InitializeNode();
                    Instance.InitializeConnection();
                    Instance.InitializeStateNames();
                }
                Instance.InitializePropertyData();

                Instance.LoadData();
            }
            else
                Debug.LogError("[Editor]]Not Found Advanced State Machine");

            Instance.Show();
        }

        private void OnLostFocus()
        {
            SaveData();
        }

        private void OnDestroy()
        {
            SaveData();
        }

        private void InitializeView()
        {
            if (Instance != null)
            {
                workView = new ASMWorkView();
                propertiesView = new ASMPropertiesView();
                stateModifyView = new ASMStateModifyView();
                transitionModifyView = new ASMTransitionModifyView();
            }
            else
            {
                Instance = GetWindow<AdvancedStateMachineEditorWindow>();
                Instance.titleContent = new GUIContent("AdvancedStateMachine");
                Instance.Show();

                workView = new ASMWorkView();
                propertiesView = new ASMPropertiesView();
                stateModifyView = new ASMStateModifyView();
                transitionModifyView = new ASMTransitionModifyView();
            }

            workView.Initialize();
            propertiesView.Initialize();

            isModifyViewOpen = false;
        }

        public void InitializePropertyData()
        {
            serializedObject = new SerializedObject(selected);
            propertyStates      = serializedObject.FindProperty("advancedStates");
            propertyTransitions = propertyStates.FindPropertyRelative("transitions");
        }

        private void InitializeNode()
        {
            if (editorNodes == null)
                editorNodes = new List<EditorNode<NodeData>>();

            editorNodes.Clear();

            if (selected == null)
                return;

            for (int i = 0; i < selected.advancedStates.Count; i++)
            {
                AdvancedState state = selected.advancedStates[i];

                CreateStateNode(state, new Vector2(30.0f + (i * 10.0f), 30.0f + (i * 10.0f)), state.ID);
            }

            for (int i = 0; i < Transitions.Length; i++)
            {
                CreateTransitionNode(Transitions[i], new Vector2(60.0f + (i * 10.0f), 30.0f + (i * 10.0f)), Transitions[i].ID);
            }
        }

        private void InitializeConnection()
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if (editorNodes[i].myData.type == NodeType.STATE)
                {
                    for (int j = 0; j < editorNodes[i].myData.state.Transition.ChildTransitions.Count; j++)
                    {
                        AttachChildNodeInParentNode(FindNodeByTransition(editorNodes[i].myData.state.Transition.ChildTransitions[j]), editorNodes[i]);
                    }
                }
                else if (editorNodes[i].myData.type == NodeType.TRANSITION)
                {
                    AttachChildNodeInParentNode(FindNodeByStateID(editorNodes[i].myData.transition.stateID), editorNodes[i]);
                }
            }
        }

        private void InitializeStateNames()
        {
            int size = selected.advancedStates.Count + 1;

            if (stateNames == null)
            {
                stateNames = new string[size];
            }
            else
            {
                if (size > stateNames.Length)
                {
                    stateNames = new string[size];
                }
            }

            for (int i = 0; i < stateNames.Length; i++)
                stateNames[i] = string.Empty;

            stateNames[0] = "None";
            for (int i = 1; i <= selected.advancedStates.Count; i++)
            {
                stateNames[i] = selected.advancedStates[i - 1].ID;
            }
        }

        private void Update()
        {
            if (selected == null || workView == null)
                return;

            if(selectNode != null)
            {
                switch(selectNode.myData.type)
                {
                    case NodeType.STATE :   
                        {
                            stateModifyView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                                       , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
                        }
                        break;
                    case NodeType.TRANSITION:
                        {
                            transitionModifyView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                                       , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
                        }
                        break;
                }//end switch
            }
            else
            {
                propertiesView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                        , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
            }

            workView.UpdateView(  new Rect(position.width, position.height, position.width, position.height)
                                , new Rect(0.3f, 0.0f, 0.7f, 1.0f));
        }

        private void OnGUI()
        {
            if(selected == null || workView == null)
            {
                EditorGUILayout.HelpBox("Please select a AdvancedStateMachine", MessageType.Info);
                return;
            }

            GUILayout.BeginHorizontal();
            {
                if (selectNode != null)
                {
                    switch (selectNode.myData.type)
                    {
                        case NodeType.STATE:
                            {
                                stateModifyView.GUIView(Event.current);
                            }
                            break;
                        case NodeType.TRANSITION:
                            {
                                transitionModifyView.GUIView(Event.current);
                            }
                            break;
                    }//end switch
                }
                else
                {
                    propertiesView.GUIView(Event.current);   
                }
                     
                workView.ProcessEvents(Event.current);

                workView.GUIView(Event.current);
            }
            GUILayout.EndHorizontal();

            //## Draw
            if (GUI.changed)
            {
                Repaint();
            }
        }

        //TODO : 동일한 ID 입력에 대한 예외처리 해주
        public void CreateState(Vector2 position)
        {
            //AdvancedState state = new AdvancedState();
            ////state.ID = "New State" + Target.advancedStates.Count;
            //Target.advancedStates.Add(state);

            //InitializePropertyData();

            //InitializeStateNames();

            //EditorNode<NodeData> node = CreateStateNode(state, position, state.ID);
            //SelectNode(node);

            //SaveData();
        }

        //TODO : 동일한 ID 입력에 대한 예외처리 해주
        public void CreateTransition(Vector2 position)
        {
        }

        public void DeleteNode(EditorNode<AdvancedStateMachineEditorWindow.NodeData> node)
        {
            if (node == null)
                return;

            if(node.myData.type == NodeType.STATE)
            {
                if(node.parentNodes != null)
                {
                    for (int i = 0; i < node.parentNodes.Count; i++)
                    {
                        node.parentNodes[i].myData.transition.stateID = string.Empty;
                    }    
                }

                selected.advancedStates.Remove(node.myData.state);
            }
                
            node.Relese();

            editorNodes.Remove(node);

            SelectNode(null);

            InitializePropertyData();

            SaveData();
        }

        private EditorNode<NodeData> CreateStateNode(AdvancedState state, Vector2 position, string title)
        {
            EditorNode<NodeData> node = new EditorNode<NodeData>(new NodeData(NodeType.STATE, state, null)
                                                                 ,editorNodes.Count
                                                                 ,new Rect(position.x - (NODE_WIDTH * 0.5f), position.y - (NODE_HEIGHT * 0.5f), NODE_WIDTH, NODE_HEIGHT)
                                                                 , title 
                                                                 ,(GUIStyle)"flow node hex 4", (GUIStyle)"flow node hex 4 on"
                                                                 , OnStateNodeGenericMenu);          
            editorNodes.Add(node);

            return node;
        }

        private EditorNode<NodeData> CreateTransitionNode(AdvancedTransition transition, Vector2 position, string title)
        {
            EditorNode<NodeData> node = new EditorNode<NodeData>(new NodeData(NodeType.TRANSITION, null, transition)
                                                                 , editorNodes.Count
                                                                 , new Rect(position.x - (NODE_WIDTH * 0.5f), position.y - (NODE_HEIGHT * 0.5f), NODE_WIDTH, NODE_HEIGHT)
                                                                 , title
                                                                 , (GUIStyle)"flow node hex 0", (GUIStyle)"flow node hex 0 on"
                                                                 , null);
            editorNodes.Add(node);

            return node;
        }

        private void OnStateNodeGenericMenu(EditorNode<NodeData> node)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Make Transition"), false, () => MakeTransition(node));
            genericMenu.AddItem(new GUIContent("Delete"), false, () => DeleteNode(node));

            genericMenu.ShowAsContext();
               
            Event.current.Use();
        }

        //안쓴다
        private void OnTransitionNodeGenericMenu(EditorNode<NodeData> node)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Connect State"), false, () => MakeTransition(node));
            genericMenu.AddItem(new GUIContent("Delete"), false, () => DeleteNode(node));

            genericMenu.ShowAsContext();

            Event.current.Use();
        }

        private void MakeTransition(EditorNode<NodeData> node)
        {
            if(workView != null)
                workView.MakeTransition(node);
        }

        public void CreateConnection(EditorNode<NodeData> startNode, EditorNode<NodeData> endNode)
        {
            if (startNode.myData.type == endNode.myData.type)
                return;

            if(startNode.myData.type == NodeType.STATE)
            {
                if (AttachTransitionInState(startNode, endNode))
                {
                    AttachChildNodeInParentNode(endNode, startNode);
                }
            }
            else if(startNode.myData.type == NodeType.TRANSITION)
            {
                startNode.myData.transition.stateID = endNode.myData.state.ID;

                if(startNode.childNodes != null)
                {
                    for (int i = 0; i < startNode.childNodes.Count;i++)
                    {
                        startNode.childNodes[i].parentNodes.Remove(startNode);
                    }

                    startNode.ClearChilds();
                }

                AttachChildNodeInParentNode(endNode, startNode);
            }
        }

        public void AttachChildNodeInParentNode(EditorNode<NodeData> childNode, EditorNode<NodeData> parentNode)
        {
            if(childNode != null && parentNode != null)
                childNode.AddParentNode(parentNode);

            if(parentNode != null && childNode != null)
                parentNode.AddChildNode(childNode);
        }

        private bool AttachTransitionInState(EditorNode<NodeData> stateNode, EditorNode<NodeData> transitionNode)
        {
            serializedObject.Update();

            for (int i = 0; i < stateNode.myData.state.Transition.ChildTransitions.Count; i++)
            {
                if (stateNode.myData.state.Transition.ChildTransitions[i] == transitionNode.myData.transition)
                    return false;
            }

            stateNode.myData.state.Transition.ChildTransitions.Add(transitionNode.myData.transition);

            EditorUtility.SetDirty(Target);
            serializedObject.ApplyModifiedProperties();

            return true;
        }

        public void SelectNodeByState(AdvancedState state)
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if(editorNodes[i].myData.type == NodeType.STATE)
                {
                    if(editorNodes[i].myData.state.ID == state.ID)
                    {
                        SelectNode(editorNodes[i]);
                        return;
                    }
                }
            }
        }

        public void SelectNodeByTransition(AdvancedTransition transition)
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if(editorNodes[i].myData.type == NodeType.TRANSITION)
                {
                    if(editorNodes[i].myData.transition.ID == transition.ID)
                    {
                        SelectNode(editorNodes[i]);
                        return;
                    }
                }
            }
        }

        private bool isModifyViewOpen = false;
        public void SelectNode(EditorNode<NodeData> node)
        {
            if (selectNode != null) 
            {
                selectNode.isSelected = false;

                if(selectNode != node)
                {
                    isModifyViewOpen = false; 
                }
            }

            selectNode = node;
            if (selectNode != null)
            {
                selectNode.isSelected = true;

                if(!isModifyViewOpen)
                {
                    InitializeStateNames();

                    switch (selectNode.myData.type)
                    {
                        case NodeType.STATE: SetDataInStateModifyView(selectNode); break;
                        case NodeType.TRANSITION: SetDataInTransitionModifyView(selectNode); break;
                    }
                    isModifyViewOpen = true;
                }
            }
            else
                isModifyViewOpen = false; 
        }

        private void SetDataInStateModifyView(EditorNode<NodeData> node)
        {
            if (node == null)
                return;
            
            stateModifyView.Initialize(propertyStates.GetArrayElementAtIndex(FindStateIndexByNode(node)), node);
        }

        private void SetDataInTransitionModifyView(EditorNode<NodeData> node)
        {
            if (node == null)
                return;
            
            //transitionModifyView.Initialize(propertyTransitions.GetArrayElementAtIndex(FindTransitionIndexByNode(node)), node);
        }

        //======================================================
        //## TODO
        //======================================================
        public void CopyNode()
        {
            if (selectNode == null)
                return;

            if(selectNode.myData.type == NodeType.STATE)
            {
                //AdvancedState state = new AdvancedState();
                ////state.ID = "copy_" + selectNode.myData.state.ID;

       
                //Target.advancedStates.Add(state);

                //InitializePropertyData();

                //InitializeStateNames();

                ////serializedObject.Update();
                ////serializedObject.Co
                ////SerializedProperty test2 = propertyStates.GetEndProperty();
                ////test2 = propertyStates.GetArrayElementAtIndex(FindStateIndexByNode(selectNode)).Copy();

                //serializedObject.ApplyModifiedProperties();

                //EditorNode<NodeData> node = CreateStateNode(state, new Vector2(selectNode.rect.position.x + 15, selectNode.rect.position.y + 15), state.ID);
                //SelectNode(node);
            }
            else if(selectNode.myData.type == NodeType.TRANSITION)
            {
                
            }
        }

    }//end class
}