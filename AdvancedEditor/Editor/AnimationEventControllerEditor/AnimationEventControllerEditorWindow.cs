using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

/*
    TODO : 이름 검색으로 변경
*/

namespace AdvancedUnityPlugin.Editor
{
    public class AnimationEventControllerEditorWindow : EditorWindow
    {
        public struct AECUndoData
        {
            public int group;
            public int selectedIndex;
            public int frameIndex;
        }

        public static AnimationEventControllerEditorWindow Instance;

        //=========================
        //  ## Data
        //=========================
        public SerializedObject serializedObject;

        public AnimationEventController animationEventController;
        public AnimationEventController.AdvancedAnimationEvent selected;
        private int selectedIndex;

        public int currentFrameIndex;

        private AECUndoData undoData;

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

            Instance.Initialize(data);

            Instance.Show();
        }

        //public void OnEnable()
        //{
        //    Undo.undoRedoPerformed += UndoRedoPerformed;
        //}

        //public void OnDisable()
        //{
        //    Undo.undoRedoPerformed -= UndoRedoPerformed;
        //}

        public void OnProjectChange()
        {
            Refresh();
        }

        public void OnSelectionChange()
        {
            Refresh();
        }

        private void Refresh()
        {
            GameObject gameObject = Selection.activeGameObject;
            if (gameObject == null)
                return;

            AnimationEventController data = gameObject.GetComponent<AnimationEventController>();
            if (data != null)
            {
                Initialize(data);
                Update();

                Repaint();
            }
        }

        //public void RecordObject(string text)
        //{
        //    if(animationEventController.animationEvents.Count >= selectedIndex)
        //    {
        //        selectedIndex = (animationEventController.animationEvents.Count - 1) < 0 ? 0 : animationEventController.animationEvents.Count - 1;
        //    }

        //    undoData.group = Undo.GetCurrentGroup();
        //    undoData.selectedIndex = selectedIndex;
        //    undoData.frameIndex = currentFrameIndex;

        //    Undo.RecordObject(animationEventController, text);

        //}

        //private void UndoRedoPerformed()
        //{
        //    Undo.CollapseUndoOperations(undoData.group);

        //    Initialize(animationEventController);

        //    selectedIndex = undoData.selectedIndex;
        //    currentFrameIndex = undoData.frameIndex;

        //    propertiesView.SetSelectAnimationIndex(selectedIndex);

        //    Repaint();
        //}

        private void Initialize(AnimationEventController data)
        {
            if (data == null)
            {
                Debug.LogError("[Editor]Not Found AnimationEventController");
                return;
            }

            animationEventController = data;

            InitializeSerializedObject();

            InitializeClip();
            InitializeKeyframeEventsInClip();
            //InitializeKeyframeEvents();
            InitializeView();

            currentFrameIndex = 0;
        }

        private void InitializeSerializedObject()
        {
            serializedObject = new SerializedObject(animationEventController);
        }

        private void InitializeView()
        {
            if (Instance == null)
            {
                Instance = GetWindow<AnimationEventControllerEditorWindow>();
                Instance.titleContent = new GUIContent("AnimationEventController");
                Instance.Show();
            }

            if(propertiesView == null)
                propertiesView = new AECPropertiesView();
            
            if(workView == null)
                workView = new AECWorkView();

            propertiesView.Initialize();
            workView.Initialize();
        }

        private void InitializeClip()
        {
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationUtility.SetAnimationEvents(animationEventController.animator.runtimeAnimatorController.animationClips[i], new AnimationEvent[0] { });        
            }

            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                //oreach(var iter in animationEventController.animationEvents)
            }
        }

        private void InitializeKeyframeEventsInClip()
        {
            //AEC에 등록된 클립에 이벤트를 등록시킨다.
            for (int i = 0; i < animationEventController.animationEvents.Count; i++)
            {
                foreach(var iter in animationEventController.animationEvents[i].keyframeEvents)
                {
                    AddKeyframeEvent(animationEventController.animationEvents[i], iter.eventKeyframe);   
                }
            }
        }

        //TODO : AEC 바탕으로 키프레임 재구성
        private void InitializeKeyframeEvents()
        {
            for (int i = 0; i < animationEventController.animationEvents.Count; i++)
            {
                if(animationEventController.animationEvents[i].clip == null)
                {
                    animationEventController.animationEvents.Remove(animationEventController.animationEvents[i]);
                    i--;
                    continue;
                }

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

            workView.Initialize();
        }

        public float GetSelectedClipLength()
        {
            if (selected == null)
                return 0;

            return selected.clip.length;
        }

        public float GetSelectedClipFrameRate()
        {
            if (selected == null || selected.clip == null)
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

        //TODO : 스트링 서치로 변경
        public bool AddNewEventAnimation(int index)
        {
            for (int i = 0; i < animationEventController.animationEvents.Count; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[index].name == animationEventController.animationEvents[i].clip.name)
                    return false;
            }

           // RecordObject("[AECE] Add Selected Animatin");

            AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = new AnimationEventController.AdvancedAnimationEvent();
            advancedAnimationEvent.clip = animationEventController.animator.runtimeAnimatorController.animationClips[index];
            advancedAnimationEvent.keyframeEvents = new List<AnimationEventController.UnityKeyframeEvent>();

            animationEventController.animationEvents.Add(advancedAnimationEvent);

            float frameRate = advancedAnimationEvent.clip.frameRate;
            float frameInterval = 1.0f / frameRate;
            float frameCount = Mathf.Round(advancedAnimationEvent.clip .length / frameInterval);

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

            //RecordObject("[AECE] Remove Selected Animation");

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

            //클립체크

            if (IsEvetInClip(selected.clip, frame))
                return false;

            //RecordObject("[AECE] Add keyframeEvent");

            string eventName = selected.clip.name + frame;
            AnimationEventController.UnityKeyframeEvent newKeyframeEvent = new AnimationEventController.UnityKeyframeEvent(eventName, frame, null);
            selected.keyframeEvents.Add(newKeyframeEvent);

            AddEventInClip(newKeyframeEvent, selected.clip);

            return true;
        }

        private bool IsClipInAnimator()
        {
            return false;
        }

        private void AddEventInClip(AnimationEventController.UnityKeyframeEvent keyframeEvent ,AnimationClip clip)
        {
            //Undo.RecordObject(clip, "Add Event in clip");

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

            if(clip == null)
            {
                Debug.LogError("[AnimationEventController] Not found clip");
                return false;
            }

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
            int frameCount = (int)Mathf.Round(selected.clip.length / (1.0f / selected.clip.frameRate));
            if (frame == 0 || frame == frameCount - 1)
                return false;

            //RecordObject("[AECE] Remove keyframeEvent");

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
            if (selected.clip.events.Length <= 0)
                return;

            //Undo.RecordObject(selected.clip, "Remove Event in clip");

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