using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkBaseEventModifyView : ViewBase
    {
        private Vector2 eventScrollPosition;

        public void Initialize()
        {
            eventScrollPosition = Vector2.zero;
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            GUILayout.BeginArea(viewRect, "", "box");
            {
                GUILayout.Box(new GUIContent("Base Events"), (GUIStyle)"dragtabdropwindow");

                eventScrollPosition = GUILayout.BeginScrollView(eventScrollPosition, "box");
                {
                    AnimationEventControllerEditorWindow.Instance.serializedObject.Update();

                    SerializedProperty serializedProperty = AnimationEventControllerEditorWindow.Instance.GetSerializedPropertyOfSelectedAnimation();
                    if(serializedProperty != null)
                    {
                        EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("onStartFrame"));
                        EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("onLastFrame"));       
                    }
                    AnimationEventControllerEditorWindow.Instance.serializedObject.ApplyModifiedProperties();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }
    }   
}