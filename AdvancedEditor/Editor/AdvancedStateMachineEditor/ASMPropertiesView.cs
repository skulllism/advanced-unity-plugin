//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace AdvancedUnityPlugin.Editor
//{
//    public class ASMPropertiesView : ViewBase
//    {
//        public ASMPropertiesView() : base("PropertiesView"){}

//        enum MainToolbar { STATE, TRANSITION}
//        enum StateToolbar { ALL, NEW }

//        //===================
//        //## [Common use]
//        //===================
//        //## Main Toolbar
//        private int mainToolbarIndex;
//        private string[] mainToolbarTaps = { "State", "Transition" };
//        //(Search)
//        private string strSearchText = "";
//        private bool isSearchCancel;

//        private GUIStyle scrollListStyle;

//        public bool isHightLight = false;

//        public void Initialize()
//        {
//            scrollListStyle = new GUIStyle("Button");
//            scrollListStyle.alignment = TextAnchor.MiddleLeft;
//        }

//        public override void UpdateView(Rect editorRect, Rect percentageRect)
//        {
//            base.UpdateView(editorRect, percentageRect);
//        }
           
//        public override void GUIView(Event e)
//        {
//            base.GUIView(e);
//            ProcessEvents(e);

//#region 1.Main Box
//            GUILayout.BeginArea(viewRect, "", "box");
//            {

//                EditorGUI.BeginChangeCheck();
//                {
//                    mainToolbarIndex = GUILayout.Toolbar(mainToolbarIndex, mainToolbarTaps);
//                }
//                if (EditorGUI.EndChangeCheck())
//                {
//                    strSearchText = string.Empty;
//                    GUI.FocusControl(null);
//                }

//    #region 2.Information Box
//                GUILayout.BeginVertical("box");
//                {
//                    DrawSearchTextField();
//                    GUILayout.Space(20.0f);

//                    GUILayout.Box("Information", (GUIStyle)"dragtabdropwindow");
                                 
//                    switch ((MainToolbar)mainToolbarIndex)
//                    {
//                        case MainToolbar.STATE:
//                            {   
//                                DrawStateInformation();
//                            }
//                            break;
//                        case MainToolbar.TRANSITION:
//                            {
//                                DrawTransitionInformation();
//                            }
//                            break;
//                    }
//                    GUILayout.Space(5.0f);

//                    #region 3-1. State Scroll Box
//                    GUILayout.BeginHorizontal("box");
//                    {
//                        switch ((MainToolbar)mainToolbarIndex)
//                        {
//                            case MainToolbar.STATE:
//                                {
//                                    DrawStateList(e);
//                                }
//                                break;
//                            case MainToolbar.TRANSITION:
//                                {
//                                    DrawStateTransitionList(e);
//                                }
//                                break;
//                        }
//                    }
//                    GUILayout.EndHorizontal();
//                    #endregion
//                }
//                GUILayout.EndVertical();
//    #endregion
//            }
//            GUILayout.EndArea();
//#endregion
//        }

//        public override void ProcessEvents(Event e)
//        {
//            base.ProcessEvents(e);
//        }

//        private void DrawSearchTextField()
//        {
//            GUILayout.BeginHorizontal();
//            {
//                strSearchText = GUILayout.TextField(strSearchText, (GUIStyle)"SearchTextField");
//                if (strSearchText == string.Empty)
//                {
//                    GUILayout.Button("", (GUIStyle)"SearchCancelButtonEmpty");
//                }
//                else
//                {
//                    isSearchCancel = GUILayout.Button("", (GUIStyle)"SearchCancelButton");
//                    if (isSearchCancel)
//                    {
//                        strSearchText = string.Empty;
//                    }
//                }
//            }
//            GUILayout.EndHorizontal();
//        }

//        private bool SearchString(string str, string findStr)
//        {
//            if (str == null || findStr == null)
//                return false;
            
//            int found = 0;
//            int total = 0;

//            for (int i = 0; i < str.Length; i++)
//            {
//                found = str.IndexOf(findStr, i);
//                if(found >= 0)
//                {
//                    total++;
//                    i = found;
//                }
//            }
//            if (total > 0)
//                return true;
//            else
//                return false;
//        }

     
//        private Vector2 stateScrollView = Vector2.zero;
//        private void DrawStateList(Event e)
//        {
//            if (!AdvancedStateMachineEditorWindow.Target)
//                return;

//            GUILayout.BeginHorizontal((GUIStyle)"AnimationKeyframeBackground");
//            {
//                stateScrollView = GUILayout.BeginScrollView(stateScrollView, false, false);
//                {
//                    if(!e.alt)
//                    {
//                        List<AdvancedStateMachine.AdvancedState> advancedStates = AdvancedStateMachineEditorWindow.Target.advancedStates;
//                        for (int i = 0; i < advancedStates.Count; i++)
//                        {
//                            if (SearchString(advancedStates[i].ID, strSearchText))
//                            {
//                                GUILayout.BeginHorizontal();
//                                {
//                                    if (GUILayout.Button(i.ToString() + ". " + advancedStates[i].ID, scrollListStyle))
//                                    {
//                                        AdvancedStateMachineEditorWindow.Instance.SelectNodeByState(advancedStates[i]);
//                                    }

//                                    //Error Icon Test
//                                    GUILayout.Label("", (GUIStyle)"CN EntryWarnIconSmall");   //TEST 
//                                }
//                                GUILayout.EndHorizontal();
//                            }
//                        }   
//                    }
//                }
//                GUILayout.EndScrollView();
//            }
//            GUILayout.EndVertical();

//            GUI.changed = true;
//        }

//        private Vector2 transitionScrollView = Vector2.zero;
//        private void DrawStateTransitionList(Event e)
//        {
//            if (!AdvancedStateMachineEditorWindow.Target)
//                return;

//            GUILayout.BeginHorizontal((GUIStyle)"AnimationKeyframeBackground");
//            {
//                transitionScrollView = GUILayout.BeginScrollView(transitionScrollView, false, true);
//                {
//                    if(!e.alt)
//                    {
//                        List<AdvancedStateMachine.AdvancedTransition> advancedTransitions = AdvancedStateMachineEditorWindow.Target.advancedTransitions;
//                        for (int i = 0; i < advancedTransitions.Count; i++)
//                        {
//                            if (SearchString(advancedTransitions[i].ID, strSearchText))
//                            {
//                                GUILayout.BeginHorizontal();
//                                {
//                                    if (GUILayout.Button(i.ToString() + ". " + advancedTransitions[i].ID, scrollListStyle))
//                                    {
//                                        AdvancedStateMachineEditorWindow.Instance.SelectNodeByTransition(advancedTransitions[i]);
//                                    }

//                                    //Error Icon Test
//                                    GUILayout.Label("", (GUIStyle)"CN EntryWarnIconSmall");   //TEST 
//                                }
//                                GUILayout.EndHorizontal();
//                            }
//                        }   
//                    }
//                }
//                GUILayout.EndScrollView();
//            }

//            GUI.changed = true;
//        }

//        private void DrawStateInformation()
//        {
//            GUILayout.BeginVertical("box");
//            {
//                GUILayout.BeginHorizontal();
//                {
//                    GUILayout.Label("State Count : ");
//                    if (AdvancedStateMachineEditorWindow.Target != null)
//                        GUILayout.Label(AdvancedStateMachineEditorWindow.Target.advancedStates.Count.ToString());
//                }
//                GUILayout.EndHorizontal();

//                GUILayout.BeginHorizontal();
//                {
//                    isHightLight = GUILayout.Toggle(isHightLight,new GUIContent("Hightlight"));;    
//                }
//                GUILayout.EndHorizontal();
//            }
//            GUILayout.EndVertical();
//        }

//        private void DrawTransitionInformation()
//        {
//            GUILayout.BeginVertical("box");
//            {
//                GUILayout.BeginHorizontal();
//                {
//                    GUILayout.Label("Transition Count : ");
//                    if (AdvancedStateMachineEditorWindow.Target != null)
//                        GUILayout.Label(AdvancedStateMachineEditorWindow.Target.advancedTransitions.Count.ToString());
//                }
//                GUILayout.EndHorizontal();
//            }
//            GUILayout.EndVertical();
//        }
//    }//end class
//}

