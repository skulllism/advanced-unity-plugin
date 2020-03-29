using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class ASMTransitionModifyView : ViewBase
    {
        public ASMTransitionModifyView() : base("StateModifyView"){}

        private SerializedProperty transitionProperty;
        private AdvancedTransition transition;
        private EditorNode<AdvancedStateMachineEditorWindow.NodeData> node;

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            ProcessEvents(e);

            GUILayout.BeginArea(viewRect, "", "box");
            {
                //DrawSerializedPropertyInformation();

                //DrawDecisionButtn();

                //DrawSerializedPropertyEvents();
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }

        public void Initialize(SerializedProperty data, EditorNode<AdvancedStateMachineEditorWindow.NodeData> node)
        {
            if (data == null || node == null)
                return;

            this.node = node;
            transitionProperty = data;
            transition = node.myData.transition;

            popupList = AdvancedStateMachineEditorWindow.Instance.stateNames;

            popupIndex = FindTransitionIndex();
        }

        private int FindTransitionIndex()
        {
            if (transition.stateID == string.Empty)
                return 0;
            
            for (int i = 0; i < AdvancedStateMachineEditorWindow.Target.advancedStates.Count; i++)
            {
                if(AdvancedStateMachineEditorWindow.Target.advancedStates[i].ID == transition.stateID)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private void DrawSerializedPropertyInformation()
        {
            if (transitionProperty == null)
                return;

            transitionProperty.serializedObject.Update();

            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Transition ID : ");
   
                    EditorGUI.BeginChangeCheck();
                    {
                        node.myData.transition.ID  = GUILayout.TextField(node.myData.transition.ID);
                        node.title = node.myData.transition.ID;

                    }
                    if(EditorGUI.EndChangeCheck())
                    {
                        AdvancedStateMachineEditorWindow.Instance.InitializePropertyData();    
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label("Target State : ");
                    DrawTransitionList();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();  

            transitionProperty.serializedObject.ApplyModifiedProperties();
        }

        private int popupIndex = 0;
        private string[] popupList = new string[15];
        private void DrawTransitionList()
        {
            transitionProperty.serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            {
                popupIndex = EditorGUILayout.Popup(popupIndex, popupList);    
            }
            if(EditorGUI.EndChangeCheck())
            {
                EditorNode<AdvancedStateMachineEditorWindow.NodeData> stateNode = AdvancedStateMachineEditorWindow.Instance.FindNodeByStateID(transition.stateID);
                if(stateNode != null)
                    stateNode.RemoveParentNode(node);
                
                node.RemoveChildNode(stateNode);
                       
                if (popupIndex > 0)
                {
                    node.myData.transition.stateID = AdvancedStateMachineEditorWindow.Target.advancedStates[popupIndex - 1].ID;

                    AdvancedStateMachineEditorWindow.Instance.AttachChildNodeInParentNode(AdvancedStateMachineEditorWindow.Instance.FindNodeByStateID(transition.stateID), node);
                }
                else
                {
                    node.myData.transition.stateID = string.Empty;
                }
                                                                                                   
                AdvancedStateMachineEditorWindow.Instance.InitializePropertyData();
            }

            transitionProperty.serializedObject.ApplyModifiedProperties();
        }

        private bool isCreate;
        private bool isDelete;
        private void DrawDecisionButtn()
        {
            GUILayout.BeginHorizontal();
            {
                isCreate = GUILayout.Button("+");
                isDelete = GUILayout.Button("-");

                if(isCreate)
                {
                 //   transition.decisions.Add(new AdvancedStateMachine.AdvancedDecision());
                }

                if(isDelete)
                {
                   // transition.decisions.RemoveAt(transition.decisions.Count - 1);
                }
            }
            GUILayout.EndHorizontal();
        }

        private SerializedObject decision;
        private Vector2 propertyScroll = Vector2.zero;
        private void DrawSerializedPropertyEvents()
        {
            if (transitionProperty == null)
                return;
            
            GUILayout.BeginHorizontal((GUIStyle)"AnimationKeyframeBackground");
            {
                propertyScroll = GUILayout.BeginScrollView(propertyScroll, false, false);
                {
                    transitionProperty.serializedObject.Update();

                    if (EditorGUILayout.PropertyField(transitionProperty.FindPropertyRelative("decisions")))
                    {
                        for (int i = 0; i < transitionProperty.FindPropertyRelative("decisions").arraySize; i++)
                        {
                            GUILayout.BeginVertical("box");
                            {
                                EditorGUILayout.PropertyField(transitionProperty.FindPropertyRelative("decisions").GetArrayElementAtIndex(i).FindPropertyRelative("isTrue"), new GUIContent("isTrue"));
                                EditorGUILayout.PropertyField(transitionProperty.FindPropertyRelative("decisions").GetArrayElementAtIndex(i).FindPropertyRelative("condition"), new GUIContent("Condition"));
                            }
                            GUILayout.EndVertical();
                        }
                    }

                    transitionProperty.serializedObject.ApplyModifiedProperties();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
        }
    }
}

