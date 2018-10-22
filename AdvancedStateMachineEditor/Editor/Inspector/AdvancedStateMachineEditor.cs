using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    [CustomEditor(typeof(AdvancedStateMachine))]
    public class AdvancedStateMachineEditor : EditorBase
    {
        private AdvancedStateMachine origin;
        private new SerializedObject serializedObject;
        private SerializedProperty initState;
 
        private int selectInitalState = 0;

        public void OnEnable()
        {
            origin = (AdvancedStateMachine)target;
            serializedObject = new SerializedObject(origin);
            initState = serializedObject.FindProperty("initialStateID");

            selectInitalState = FindStateIndex();
        }

        private int FindStateIndex()
        {
            for (int i = 0; i < origin.advancedStates.Count; i++)
            {
                if(origin.initialStateID == origin.advancedStates[i].ID)
                {
                    return i;
                }
            }
            return 0;
        }

        public override void OnInspectorGUI()
        {
            Space(10.0f);

            GUILayout.BeginVertical("box");
            {
                if (GUILayout.Button("Open Editor"))
                {
                    AdvancedStateMachineEditorWindow.OpenWindow(origin);
                }

                DrawStatePanel();
            }
            GUILayout.EndVertical();

            Space(10.0f);

            //base.OnInspectorGUI();
        }

        private void DrawStatePanel()
        {
            Space(10.0f);

            DrawInitialState();
        }

        private string[] currentStates = new string[10];
        private void DrawInitialState()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Initial State : ");

                if (currentStates.Length - 1 <= origin.advancedStates.Count)
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
                if (EditorGUI.EndChangeCheck())
                {

                }
                if (selectInitalState >= origin.advancedStates.Count)
                {
                    selectInitalState = 0;
                    if (origin.advancedStates.Count <= 0)
                    {
                        initState.stringValue = origin.initialStateID = string.Empty;
                    }
                }
                else
                {
                    initState.stringValue = origin.initialStateID = currentStates[selectInitalState];
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}