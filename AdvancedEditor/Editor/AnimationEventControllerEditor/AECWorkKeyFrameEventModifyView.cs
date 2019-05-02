using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkKeyFrameEventModifyView : ViewBase
    {
        private Vector2 eventScrollPosition;
        private int currentFrameIndex;

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
                GUILayout.Box(new GUIContent("Keyframe Events"), (GUIStyle)"dragtabdropwindow");

                eventScrollPosition = GUILayout.BeginScrollView(eventScrollPosition, "box");
                {
                    AnimationEventControllerEditorWindow.Instance.serializedObject.Update();

                    SerializedProperty serializedProperty = AnimationEventControllerEditorWindow.Instance.GetSerializedPropertyOfSelectedAnimation();
                    if(serializedProperty != null)
                    {
                        int frameCount = AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameCount();

                        for (int i = 0; i < serializedProperty.FindPropertyRelative("keyframeEvents").arraySize; i++)
                        {
                            int frame = serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("eventKeyframe").intValue;
                            if (frame == 0 || frame == frameCount - 1)
                                continue;
                            
                            if (serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("eventKeyframe").intValue == currentFrameIndex)
                            {
                                EditorGUILayout.PropertyField(serializedProperty.FindPropertyRelative("keyframeEvents").GetArrayElementAtIndex(i).FindPropertyRelative("onKeyframe"));
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

        public void SetCurrentFrameIndex(int index)
        {
            currentFrameIndex = index;
        }
    }
}