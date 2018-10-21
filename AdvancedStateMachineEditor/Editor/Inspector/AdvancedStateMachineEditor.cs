using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    [CustomEditor(typeof(AdvancedStateMachine))]
    public class AdvancedStateMachineEditor : EditorBase
    {
        enum MainToolbar { STATE, TRANSITION }

        private AdvancedStateMachine origin;

        //======================
        //## Editor Data
        //======================
        //## [Main Toolbar]
        private int mainTapIndex;
        private string[] mainTapNames = { "State", "State Transition" };

        //## [State Panel]
        private int stateTapIndex;
        private string[] stateTapNames = { "Information" };

        private int selectInitalState;


        public void OnEnable()
        {
            origin = (AdvancedStateMachine)target;
        }

        private void RefreshData()
        {

        }

        public override void OnInspectorGUI()
        {
            Space(10.0f);

#region 1_MainToolbar

    #region 2_MainBox
            GUILayout.BeginVertical("box");
            {
                if(GUILayout.Button("Open Editor"))
                {
                    AdvancedStateMachineEditorWindow.OpenWindow(origin);
                }

                switch ((MainToolbar)mainTapIndex)
                {
                    //## State
                    case MainToolbar.STATE:
                        {
                       
                            DrawStatePanel();
                        }
                        break;
                    //## State Transition
                    case MainToolbar.TRANSITION:
                        {
              
                        }
                        break;
                }
            }
            GUILayout.EndVertical();
    #endregion
            Space(10.0f);
#endregion


            base.OnInspectorGUI();
        }

        private void DrawStatePanel()
        {
            Space(10.0f);

            DrawInitalState();

            //Space(10.0f);

            //stateTapIndex = GUILayout.Toolbar(stateTapIndex, stateTapNames, (GUIStyle)"dragtabdropwindow");

            //GUILayout.BeginHorizontal("box");
            //{
            //    GUILayout.BeginVertical((GUIStyle)"ColorPickerSliderBackground");
            //    {
            //        stateTapIndex = GUILayout.Toolbar(stateTapIndex, stateTapNames, (GUIStyle)"MiniToolbarButton");
            //        Space(10.0f);
            //        Space(10.0f);
            //        Space(10.0f);
            //        Space(10.0f);
            //    }
            //    GUILayout.EndVertical();

            //    GUILayout.BeginVertical((GUIStyle)"ColorPickerSliderBackground");
            //    {
            //        stateTapIndex = GUILayout.Toolbar(stateTapIndex, stateTapNames, (GUIStyle)"MiniToolbarButton");
            //        Space(10.0f);
            //        Space(10.0f);
            //        Space(10.0f);
            //        Space(10.0f);
            //    }
            //    GUILayout.EndVertical();
            //}
            //GUILayout.EndHorizontal();
        }

        private string[] currentStates = new string[10];
        private void DrawInitalState()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Inital State : ");

                if(currentStates.Length - 1 <= origin.advancedStates.Count)
                {
                    currentStates = new string[origin.advancedStates.Count];
                }

                for (int i = 0; i < currentStates.Length; i++)
                {
                    currentStates[i] = string.Empty;
                }

                for (int i = 0; i < origin.advancedStates.Count; i++)
                {
                    currentStates[i] = origin.advancedStates[i].ID;
                }

                EditorGUI.BeginChangeCheck();
                {
                    selectInitalState = EditorGUILayout.Popup(selectInitalState, currentStates);
                }
                if(EditorGUI.EndChangeCheck())
                {
                      
                }

                if (selectInitalState >= origin.advancedStates.Count)
                {
                    selectInitalState = 0;
                    if(origin.advancedStates.Count <= 0)
                        origin.initialStateID = string.Empty;
                }
                else
                    origin.initialStateID = currentStates[selectInitalState];    
            }
            GUILayout.EndHorizontal();
        }
    }
}