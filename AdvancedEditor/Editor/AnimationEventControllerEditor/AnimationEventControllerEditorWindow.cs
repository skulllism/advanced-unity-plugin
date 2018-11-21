using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AnimationEventControllerEditorWindow : EditorWindow
    {
        public static AnimationEventControllerEditorWindow Instance;

        //=========================
        //  ## Data
        //=========================
        public SerializedObject serializedObject;

        public AnimationEventController animationEventController;
        public AnimationEventController.AdvancedAnimationEvent selected;
        private int selectedIndex;

        public int currentFrameIndex;

        //=========================
        //  ## View
        //=========================
        public AECPropertiesView propertiesView;
        public AECWorkView workView;

        public static void OpenWindow(AnimationEventController data)
        {
            Instance = GetWindow<AnimationEventControllerEditorWindow>();
            Instance.titleContent = new GUIContent("AnimationEventController");

            if(data != null)
            {
                Instance.animationEventController = data;

                Instance.InitializeSerializedObject();
                Instance.InitializeView();
            }
            else
                Debug.LogError("[Editor]Not Found AnimationEventController");

            Instance.Show();
        }

        public void OnSelectionChange()
        {
            
        }

        private void InitializeSerializedObject()
        {
            serializedObject = new SerializedObject(animationEventController);

            currentFrameIndex = 0;
        }

        private void InitializeView()
        {
            if (Instance != null)
            {
                propertiesView = new AECPropertiesView();
                workView = new AECWorkView();
            }
            else
            {
                Instance = GetWindow<AnimationEventControllerEditorWindow>();
                Instance.titleContent = new GUIContent("AnimationEventController");
                Instance.Show();

                propertiesView = new AECPropertiesView();
                workView = new AECWorkView();
            }

            propertiesView.Initialize();
            workView.Initialize();
        }

        private void Update()
        {
            if (workView == null || animationEventController == null)
                return;

            propertiesView.UpdateView( new Rect(position.width, position.height, position.width, position.height)
                                      ,new Rect(0.0f, 0.0f, 0.3f, 1.0f));

            workView.UpdateView( new Rect(position.width, position.height, position.width, position.height)
                                ,new Rect(0.3f, 0.0f, 0.7f, 1.0f));
        }

        private void OnGUI()
        {
            if (workView == null || animationEventController == null)
            {
                EditorGUILayout.HelpBox("Please select a AnimationEventController", MessageType.Info);
                return;
            }

            GUILayout.BeginHorizontal();
            {
                propertiesView.GUIView(Event.current);
                workView.GUIView(Event.current);
            }
            GUILayout.EndHorizontal();

            if (GUI.changed)
                Repaint();
        }


        public void SelectAnimationEvent(int index)
        {
            selectedIndex = index;

            if (animationEventController.animationEvents.Count > 0)
                selected = animationEventController.animationEvents[index];
            else
                selected = null;
        }

        public float GetSelectedClipLength()
        {
            if (selected == null)
                return 0;

            return selected.clip.length;
        }

        public float GetSelectedClipFrameRate()
        {
            if (selected == null)
                return 1.0f;

            return selected.clip.frameRate;
        }

        public int GetSelectedKeyframeEventsCount()
        {
            if (selected == null)
                return 0;

            return selected.keyframeEvents.Count;
        }



        public SerializedProperty GetSerializedPropertyOfSelectedAnimation()
        {
            if (serializedObject == null)
                InitializeSerializedObject();

            if(serializedObject.FindProperty("animationEvents").arraySize > 0)
            {
                SerializedProperty serializedProperty = serializedObject.FindProperty("animationEvents").GetArrayElementAtIndex(selectedIndex);

                return serializedProperty;                
            }
            return null;
        }

        public bool AddNewEventAnimation(int index)
        {
            for (int i = 0; i < animationEventController.animationEvents.Count; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[index].name == animationEventController.animationEvents[i].clip.name)
                {
                    return false;
                }
            }

            AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = new AnimationEventController.AdvancedAnimationEvent();
            advancedAnimationEvent.clip = animationEventController.animator.runtimeAnimatorController.animationClips[index];
            advancedAnimationEvent.keyframeEvents = new List<AnimationEventController.UnityKeyframeEvent>();

            animationEventController.animationEvents.Add(advancedAnimationEvent);

            return true;
        }

        public bool RemoveSelectedAnimation()
        {
            if (selected == null)
                return false;
            
            animationEventController.animationEvents.Remove(selected);

            if(animationEventController.animationEvents != null)
            {
                SelectAnimationEvent(animationEventController.animationEvents.Count - 1);
            }

            return true;
        }

        public bool AddKeyframeEvent(int frame)
        {
            if (selected == null)
                return false;

            float interval = 1.0f / selected.clip.frameRate;
            int length = (int)((selected.clip.length / interval) - 1.0f);
            if (frame == 0 || frame >= length)
                return false;

            //해당 키프레임에 이벤트가 없으면 추가한다
            for (int i = 0; i < selected.keyframeEvents.Count; i++)
            {
                if (selected.keyframeEvents[i].eventKeyframe == frame)
                    return false;
            }

            AnimationEventController.UnityKeyframeEvent newKeyframeEvent = new AnimationEventController.UnityKeyframeEvent("", frame, null);
            selected.keyframeEvents.Add(newKeyframeEvent);

            return true;
        }

        public bool RemoveKeyframeEvent(int frame)
        {
            if (selected == null)
                return false;

            float interval = 1.0f / selected.clip.frameRate;
            int length = (int)((selected.clip.length / interval) - 1.0f);
            if (frame == 0 || frame >= length)
                return false;

            for (int i = 0; i < selected.keyframeEvents.Count; i++)
            {
                if (selected.keyframeEvents[i].eventKeyframe == frame)
                {
                    selected.keyframeEvents.Remove(selected.keyframeEvents[i]);
                    return true;
                }
            }

            return false;
        }
    }
}