using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class ASMStateModifyView : ViewBase
    {
        public ASMStateModifyView() : base("StateModifyView"){}

        private SerializedProperty stateProperty;
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
                DrawSerializedPropertyInformation(e);
                GUILayout.Space(10.0f);
                DrawSerializedPropertEvents();
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

            stateProperty = data;
            this.node = node;
                
            InitalizeListItems();
        }

        private void DrawSerializedPropertyInformation(Event e)
        {
            if (stateProperty == null)
                return;

            stateProperty.serializedObject.Update();

            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("State ID : ");
                    //================================================
                    //  ## 아이디 바뀔떄마다 자신을 가르키고 있는 트랜지션의 ID도 바꿔준.
                    //================================================
                    EditorGUI.BeginChangeCheck();
                    {
                        //node.myData.state.ID = GUILayout.TextField(node.myData.state.ID);
                        node.title = node.myData.state.ID;    
                    }
                    if(EditorGUI.EndChangeCheck())
                    {
                        AdvancedStateMachineEditorWindow.Instance.InitializePropertyData();

                        if(node.parentNodes != null && node.parentNodes.Count >= 0)
                        {
                            for (int i = 0; i < node.parentNodes.Count; i++)
                            {
                                if (node.parentNodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
                                {
                                    node.parentNodes[i].myData.transition.stateID = node.myData.state.ID;
                                }
                            }   
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Box("Transition", (GUIStyle)"dragtabdropwindow");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.Space(5.0f);

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Count : ");
                        GUILayout.Label(node.myData.state.Transition.ChildTransitions.Count.ToString());
                    }
                    GUILayout.EndHorizontal();

                    DrawListItems(e);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            stateProperty.serializedObject.ApplyModifiedProperties();
        }

        private List<ListItem<AdvancedTransition>> transitions;
        private void InitalizeListItems()
        {
            if (transitions == null)
                transitions = new List<ListItem<AdvancedTransition>>();

            transitions.Clear();

            for (int i = 0; i < node.myData.state.Transition.ChildTransitions.Count; i++)
            {
                ListItem<AdvancedTransition> item = new ListItem<AdvancedTransition>(i);

                item.SetData(node.myData.state.Transition.ChildTransitions[i]);

                transitions.Add(item);
            }
        }

        private Vector2 scrollPosition = Vector2.zero;
        private ListItem<AdvancedTransition> selectedItem;
        private void DrawListItems(Event e)
        {
            GUILayout.BeginVertical();
            {
                if (EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("transitions")))
                {
                    if (node != null)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button(new GUIContent("Up")))
                                ClickListItemMoveUp();
                            else if (GUILayout.Button(new GUIContent("Down")))
                                ClickListItemMoveDown();
                            else if (GUILayout.Button(new GUIContent("Delete")))
                                ClickRemoveItem();
                        }
                        GUILayout.EndHorizontal();


                        GUILayout.BeginVertical();
                        {
                            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
                            {
                                for (int i = 0; i < transitions.Count; i++)
                                {
                                    transitions[i].SetRect(GUILayoutUtility.GetRect(5, 25));
                                }

                                ProcessListItemEvents(e);

                                for (int i = 0; i < transitions.Count; i++)
                                {
                                    if(transitions[i].data != null)
                                        transitions[i].Draw((i + 1).ToString() + ". " + transitions[i].data.ID);
                                }
                            }
                            GUILayout.EndScrollView();
                        }
                        GUILayout.EndVertical();
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void ProcessListItemEvents(Event e)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if(transitions[i].ProcessEvents(e))
                {
                    SelectListItem(transitions[i]);
                    return;
                }
            }

            if(selectedItem != null)
            {
                if (!selectedItem.isSelected)
                    selectedItem = null;
            }
        }

        private void SelectListItem(ListItem<AdvancedTransition> item)
        {
            if(selectedItem != null)
            {
                selectedItem.isSelected = false;
            }

            selectedItem = item;
            selectedItem.isSelected = true;
        }

        private void ClickListItemMoveUp()
        {
            if (selectedItem == null || transitions == null || transitions.Count <= 0)
                return;

            int selectedIndex = selectedItem.index;

            if (selectedIndex > 0)
            {
                ListItem<AdvancedTransition> t = transitions[selectedIndex];
                transitions[selectedIndex] = transitions[selectedIndex - 1];
                transitions[selectedIndex - 1] = t;


                AdvancedTransition temp = node.myData.state.Transition.ChildTransitions[selectedIndex];
                node.myData.state.Transition.ChildTransitions[selectedIndex] = node.myData.state.Transition.ChildTransitions[selectedIndex - 1];
                node.myData.state.Transition.ChildTransitions[selectedIndex - 1] = temp;


                SelectListItem(transitions[selectedIndex - 1]);
            }
            else
            {
                ListItem<AdvancedTransition> t = transitions[selectedIndex];
                transitions[selectedIndex] = transitions[transitions.Count - 1];
                transitions[transitions.Count - 1] = t;


                AdvancedTransition temp = node.myData.state.Transition.ChildTransitions[selectedItem.index];
                node.myData.state.Transition.ChildTransitions[selectedItem.index] = node.myData.state.Transition.ChildTransitions[transitions.Count - 1];
                node.myData.state.Transition.ChildTransitions[transitions.Count - 1] = temp;

                SelectListItem(transitions[transitions.Count - 1]);
            }

            RefreshListItemIndex();

            EditorUtility.SetDirty(AdvancedStateMachineEditorWindow.Target);
        }

        private void ClickListItemMoveDown()
        {
            if (selectedItem == null)
                return;

            int selectedIndex = selectedItem.index;

            if (selectedIndex < transitions.Count - 1)
            {
                ListItem<AdvancedTransition> t = transitions[selectedIndex];
                transitions[selectedIndex] = transitions[selectedIndex + 1];
                transitions[selectedIndex + 1] = t;

                AdvancedTransition temp = node.myData.state.Transition.ChildTransitions[selectedItem.index];
                node.myData.state.Transition.ChildTransitions[selectedItem.index] = node.myData.state.Transition.ChildTransitions[selectedItem.index + 1];
                node.myData.state.Transition.ChildTransitions[selectedItem.index + 1] = temp;

                SelectListItem(transitions[selectedItem.index + 1]);
            }
            else
            {
                ListItem<AdvancedTransition> t = transitions[selectedIndex];
                transitions[selectedIndex] = transitions[0];
                transitions[0] = t;


                AdvancedTransition temp = node.myData.state.Transition.ChildTransitions[selectedItem.index];
                node.myData.state.Transition.ChildTransitions[selectedItem.index] = node.myData.state.Transition.ChildTransitions[0];
                node.myData.state.Transition.ChildTransitions[0] = temp;

                SelectListItem(transitions[0]);
            }

            RefreshListItemIndex();

            EditorUtility.SetDirty(AdvancedStateMachineEditorWindow.Target);
        }

        private void ClickRemoveItem()
        {
            if (selectedItem == null)
                return;

            node.myData.state.Transition.ChildTransitions.Remove(selectedItem.data);
            transitions.Remove(selectedItem);

            node.RemoveChildNode(AdvancedStateMachineEditorWindow.Instance.FindNodeByTransition(selectedItem.data));

            RefreshListItemIndex();
        }

        private void RefreshListItemIndex()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].index = i;
            }
        }

        private bool isEventFold = true;
        private Vector2 propertyScroll = Vector2.zero;
        private void DrawSerializedPropertEvents()
        {
            if (stateProperty == null)
                return;
            
            GUILayout.BeginHorizontal((GUIStyle)"AnimationKeyframeBackground");
            {
                propertyScroll = GUILayout.BeginScrollView(propertyScroll, false, false);
                {
                    GUILayout.BeginVertical("box");
                    {
                        

                        isEventFold = EditorGUILayout.Foldout(isEventFold, new GUIContent("Events"));
                        if(isEventFold)
                        {
                            stateProperty.serializedObject.Update();
                            EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("onEnter"), new GUIContent("onEnter"));
                            EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("onUpdate"), new GUIContent("onUpdate"));
                            EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("onFixedUpdate"), new GUIContent("onFixedUpdate"));
                            EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("onLateUpdate"), new GUIContent("onLateUpdate"));
                            EditorGUILayout.PropertyField(stateProperty.FindPropertyRelative("onExit"), new GUIContent("onExit"));    
                            stateProperty.serializedObject.ApplyModifiedProperties();
                        }


                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();               
        }
    }
}