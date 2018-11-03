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
        public AnimationEventController animationEventController;
        public AnimationEventController.AdvancedAnimationEvent selected;
        private int selectedIndex;

        public SerializedObject serializedObject;

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
                GUILayout.Label("No Animation Event Controller Selected");
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
            if (animationEventController.animationEvents.Count <= index)
                return;
            
            selectedIndex = index;
            selected = animationEventController.animationEvents[index];
        }

        public SerializedProperty GetSerializedPropertyOfSelectedAnimation()
        {
            if (serializedObject == null)
                InitializeSerializedObject();
            
            SerializedProperty serializedProperty = serializedObject.FindProperty("animationEvents").GetArrayElementAtIndex(selectedIndex);

            return serializedProperty;
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

            animationEventController.animationEvents.Add(advancedAnimationEvent);

            return true;
        }

        public bool RemoveSelectedAnimation()
        {
            if (selected == null)
                return false;
            
            animationEventController.animationEvents.Remove(selected);

            SelectAnimationEvent(0);
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