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
                        int size = serializedProperty.FindPropertyRelative("keyframeEvents").arraySize;
                        if(size > 0)
                        {
                            int frameCount = AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameCount();

                            for (int i = 0; i < serializedProperty.FindPropertyRelative("keyframeEvents").arraySize; i++)
                            {
                                int frame = serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("eventKeyframe").intValue;
                                if(frame == 0)
                                {
                                    GUILayout.BeginVertical();
                                    {
                                        GUILayout.Label("onStartKeyframeEvent");
                                        EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("onKeyframe"));        
                                    }
                                    GUILayout.EndVertical();
                                }
                                else if(frame == frameCount - 1)
                                {
                                    GUILayout.BeginVertical();
                                    {
                                        GUILayout.Label("onLastKeyframeEvent");
                                        EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("onKeyframe"));        
                                    }
                                    GUILayout.EndVertical();
                                }
                            }
                        }
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