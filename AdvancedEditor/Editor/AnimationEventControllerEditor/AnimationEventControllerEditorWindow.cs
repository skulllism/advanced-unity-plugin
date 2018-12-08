using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

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

        public const string EVENT_FUNCTION_NAME = "StartAnimationKeyframeEvent";

        public static void OpenWindow(AnimationEventController data)
        {
            Instance = GetWindow<AnimationEventControllerEditorWindow>();
            Instance.titleContent = new GUIContent("AnimationEventController");

            if(data != null)
            {
                Instance.animationEventController = data;

                Instance.InitializeSerializedObject();
                Instance.InitializeView();
                Instance.InitializeKeyframeEvents();
            }
            else
                Debug.LogError("[Editor]Not Found AnimationEventController");

            Instance.Show();
        }
        public void OnSelectionChange()
        {
            //AnimationEventController target = null;
            //GameObject gameObject = Selection.activeGameObject;

            //if(gameObject != null)
            //{
            //    target = gameObject.GetComponent<AnimationEventController>();
            //    if (target != null)
            //    {
            //        Instance = GetWindow<AnimationEventControllerEditorWindow>();
            //        Debug.Log("awad");
            //        Instance.animationEventController = target;

            //        //OpenWindow(target);
            //    }
            //}
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

        private void InitializeKeyframeEvents()
        {
            int clipCount = animationEventController.animationEvents.Count;
            for (int i = 0; i < clipCount; i++)
            {
                float frameRate = animationEventController.animationEvents[i].clip.frameRate;

                int eventCount = animationEventController.animationEvents[i].keyframeEvents.Count;
                for (int j = 0; j < eventCount; j++)
                {
                    int frame = animationEventController.animationEvents[i].keyframeEvents[j].eventKeyframe;
                    if(IsEvetInClip(animationEventController.animationEvents[i].clip, frame))
                        continue;

                    RemoveKeyframeEvent(animationEventController.animationEvents[i], frame);
                }
            }
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
                    return false;
            }

            AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = new AnimationEventController.AdvancedAnimationEvent();
            advancedAnimationEvent.clip = animationEventController.animator.runtimeAnimatorController.animationClips[index];
            advancedAnimationEvent.keyframeEvents = new List<AnimationEventController.UnityKeyframeEvent>();

            animationEventController.animationEvents.Add(advancedAnimationEvent);

            float frameRate = advancedAnimationEvent.clip.frameRate;
            float frameInterval = 1.0f / frameRate;
            float frameCount = advancedAnimationEvent.clip .length / frameInterval;

            if(frameCount > 0.0f)
            {
                AddKeyframeEvent(advancedAnimationEvent, 0);
                AddKeyframeEvent(advancedAnimationEvent, (int)frameCount - 1);
            }

            return true;
        }

        public bool RemoveSelectedAnimation()
        {
            if (selected == null)
                return false;

            //리스트에서 삭제하고 동시에 클립의 애니메이션을 없애줘야한다 .
            AnimationUtility.SetAnimationEvents(selected.clip, new AnimationEvent[0] { });

            animationEventController.animationEvents.Remove(selected);

            if(animationEventController.animationEvents != null)
            {
                SelectAnimationEvent(animationEventController.animationEvents.Count - 1);
            }

            return true;
        }

        public bool AddKeyframeEvent(AnimationEventController.AdvancedAnimationEvent selected, int frame)
        {
            if (IsEventInAdvancedAnimationEvents(selected, frame))
                return false;

            if (IsEvetInClip(selected.clip, frame))
                return false;

            string eventName = selected.clip.name + frame;
            AnimationEventController.UnityKeyframeEvent newKeyframeEvent = new AnimationEventController.UnityKeyframeEvent(eventName, frame, null);
            selected.keyframeEvents.Add(newKeyframeEvent);

            AddEventInClip(newKeyframeEvent, selected.clip);

            return true;
        }

        private void AddEventInClip(AnimationEventController.UnityKeyframeEvent keyframeEvent ,AnimationClip clip)
        {
            float interval = 1.0f / clip.frameRate;

            AnimationEvent newEvent = new AnimationEvent();
            newEvent.functionName    = EVENT_FUNCTION_NAME;
            newEvent.stringParameter = keyframeEvent.ID;
            newEvent.time = keyframeEvent.eventKeyframe * interval;

            int length = clip.events.Length + 1;
            AnimationEvent[] events = new AnimationEvent[length];
            for (int i = 0; i < clip.events.Length; i++)
            {
                events[i] = clip.events[i];
            }

            events[length - 1] = newEvent;

            AnimationUtility.SetAnimationEvents(clip, events);
        }

        private bool IsEventInAdvancedAnimationEvents(AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent, int frame)
        {
            AnimationEventController.AdvancedAnimationEvent animationEvent = advancedAnimationEvent;
            for (int i = 0; i < animationEvent.keyframeEvents.Count; i++)
            {
                if (animationEvent.keyframeEvents[i].eventKeyframe == frame)
                    return true;
            }

            return false;
        }

        private bool IsEvetInClip(AnimationClip clip, int frame)
        {
            float frameRate = clip.frameRate;

            for (int i = 0; i < clip.events.Length; i++)
            {
                int currentFrame = (int)(clip.events[i].time * frameRate);
                if (currentFrame == frame)
                    return true;
            }

            return false;
        }

        public bool RemoveKeyframeEvent(AnimationEventController.AdvancedAnimationEvent selected, int frame)
        {
            //클립의 퍼스트 라스트 이벤트는 손 안된? ㅇㅋ 
            int frameCount = (int)(selected.clip.length / (1.0f / selected.clip.frameRate));
            if (frame == 0 || frame == frameCount - 1)
                return false;

            for (int i = 0; i < selected.keyframeEvents.Count; i++)
            {
                if (selected.keyframeEvents[i].eventKeyframe == frame)
                {
                    selected.keyframeEvents.Remove(selected.keyframeEvents[i]);

                    RemoveEventInClip(selected, frame);
                    return true;
                }
            }

            return false;
        }

        private void RemoveEventInClip(AnimationEventController.AdvancedAnimationEvent selected, int frame)
        {
            AnimationEvent[] events = new AnimationEvent[selected.clip.events.Length - 1];

            int index = 0;
            for (int i = 0; i < selected.clip.events.Length; i++)
            {
                int currentFrame = (int)(selected.clip.events[i].time * selected.clip.frameRate);
                if (currentFrame == frame)
                    continue;

                events[index++] = selected.clip.events[i];
            }

            AnimationUtility.SetAnimationEvents(selected.clip, events);
        }
    }
}