using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class TransitionModifyView : ViewBase
    {
        public TransitionModifyView() : base("StateModifyView"){}

        private SerializedProperty transitionProperty;
        private AdvancedStateMachine.AdvancedTransition transition;
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
                DrawSerializedPropertyInformation();

                DrawDecisionButtn();

                DrawSerializedPropertyEvents();
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
          //  AllocatePopupList();

            if(popupIndex == 0)
                popupIndex = FindTransitionIndex();
        }

        private int FindTransitionIndex()
        {
            if (transition.state == null)
                return 0;
            
            for (int i = 0; i < AdvancedStateMachineEditorWindow.Target.advancedStates.Count; i++)
            {
                if(AdvancedStateMachineEditorWindow.Target.advancedStates[i].ID == transition.state.ID)
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
            
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Transition ID : ");
                    node.myData.transition.ID = GUILayout.TextField(node.myData.transition.ID);
                    node.title = node.myData.transition.ID;

                    AdvancedStateMachineEditorWindow.Instance.InitializePropertyData();
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
        }

        private bool AllocatePopupList()
        {
            int size = AdvancedStateMachineEditorWindow.Target.advancedStates.Count + 1;

            if (popupList == null)
            {
                popupList = new string[size];
            }
            else
            {
                if(size > popupList.Length)
                {
                    popupList = new string[size];
                }
            }

            for (int i = 0; i < popupList.Length; i++)
                popupList[i] = string.Empty;

            popupList[0] = "None";
            for (int i = 1; i <= AdvancedStateMachineEditorWindow.Target.advancedStates.Count; i++)
            {
                popupList[i] = AdvancedStateMachineEditorWindow.Target.advancedStates[i - 1].ID;
            }

            return true;
        }

        private int popupIndex = 0;
        private string[] popupList = new string[50];
        private void DrawTransitionList()
        {
            EditorGUI.BeginChangeCheck();
            {
                popupIndex = EditorGUILayout.Popup(popupIndex, popupList);    
            }
            if(EditorGUI.EndChangeCheck())
            {
                if(AdvancedStateMachineEditorWindow.Instance.FindNodeByState(transition.state) != null)
                    AdvancedStateMachineEditorWindow.Instance.FindNodeByState(transition.state).RemoveParentNode(node);
                
                node.RemoveChildNode(AdvancedStateMachineEditorWindow.Instance.FindNodeByState(transition.state));
                       
                if (popupIndex > 0)
                {
                    node.myData.transition.state = AdvancedStateMachineEditorWindow.Target.advancedStates[popupIndex - 1];

                    AdvancedStateMachineEditorWindow.Instance.AttachChildNodeInParentNode(AdvancedStateMachineEditorWindow.Instance.FindNodeByState(transition.state), node);
                }
                else
                {
                    node.myData.transition.state = null;
                }
                                                                                                   
                AdvancedStateMachineEditorWindow.Instance.InitializePropertyData();
            }
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
                    transition.decisions.Add(new AdvancedStateMachine.AdvancedDecision());
                }

                if(isDelete)
                {
                    transition.decisions.RemoveAt(transition.decisions.Count - 1);
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

            transitionProperty.serializedObject.Update();

            GUILayout.BeginHorizontal((GUIStyle)"AnimationKeyframeBackground");
            {
                propertyScroll = GUILayout.BeginScrollView(propertyScroll, false, false);
                {
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
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();

            transitionProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}

