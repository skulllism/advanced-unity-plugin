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
        public struct TimelineData
        {
            public int frameIndex;
        }

        public struct Data
        {
            public int group;
            public int eventAnimationIndex;
            public AnimationEventController.AdvancedAnimationEvent eventAnimation;
            public TimelineData timeline;
        }

        public const string EVENT_FUNCTION_NAME = "StartAnimationKeyframeEvent";
        public const float  DEFAULT_CLIP_LENGTH = 0.0f;
        public const float  DEFAULT_CLIP_FRAMERATE = 1.0f;
        public const int    DEFAULT_CLIP_FRAMECOUNT = 0;

        public static AnimationEventControllerEditorWindow Instance;

        //=========================
        //  ## Data
        //=========================
        public SerializedObject serializedObject;

        public AnimationEventController animationEventController;
 
        public Data current;

        public List<AnimationEventController.AdvancedAnimationEvent> matchEvents;
        public List<AnimationEventController.AdvancedAnimationEvent> mismatchEvents;

        //=========================
        //  ## View
        //=========================
        public AECPropertiesView propertiesView;
        public AECWorkView workView;

        public static void OpenWindow(AnimationEventController data)
        {
            Instance = GetWindow<AnimationEventControllerEditorWindow>();
            Instance.titleContent = new GUIContent("AnimationEventController");

            Instance.Initialize(data);

            Instance.Show();
        }

        public void OnProjectChange()
        {
            RefreshEditorWindow();
        }

        public void OnSelectionChange()
        {
            RefreshEditorWindow();
        }

        public void OnHierarchyChange()
        {
            RefreshEditorWindow();
        }

        public void OnInspectorUpdate()
        {
            if (animationEventController == null || animationEventController.animator == null || animationEventController.animator.runtimeAnimatorController == null)
            {
                RefreshEditorWindow();
                return;
            }
            
            if (animationEventController.metaFile == null)
            {
                string path = AssetDatabase.GetAssetPath(animationEventController.animator.runtimeAnimatorController) + "Metafile.asset";
                animationEventController.metaFile = (AnimationEventControllerMetaFile)AssetDatabase.LoadAssetAtPath(path, typeof(AnimationEventControllerMetaFile));
                RefreshEditorWindow();
            }
        }

        private void RefreshEditorWindow()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            AnimationEventController data = activeGameObject != null ? activeGameObject.GetComponent<AnimationEventController>() : null;
            if (data != null )
            {
                //&& data.animator != null && data.animator.runtimeAnimatorController != null
                Initialize(data);
                Update();
                Repaint();
            }
        }

        public void Initialize(AnimationEventController data)
        {
            animationEventController = data;
            if (animationEventController == null || animationEventController.animator == null || animationEventController.animator.runtimeAnimatorController == null || animationEventController.metaFile == null)
                return;
            //TODO
            if (animationEventController.animationEvents == null)
                animationEventController.animationEvents = new List<AnimationEventController.AdvancedAnimationEvent>();

            InitializeSerializedObject();
            //TODO : Meta파일 기반으로 이전 기록된 로그 데이터 뽑아내기
            InitializeAllClipInAnimator();
            CompareWithClipNameInAnimator();
            InitializeKeyframeEventsInClip();
            CheckEventAnimationClipName();

            InitializeView();
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

            propertiesView.Initialize(animationEventController);
            workView.Initialize(animationEventController);
        }

        private void CheckEventAnimationClipName()
        {
            for (int i = 0; i < animationEventController.animationEvents.Count; i++)
            {
                if(animationEventController.animationEvents[i].clipName == string.Empty ||
                   animationEventController.animationEvents[i].clipName == null)
                {
                    if(animationEventController.animationEvents[i].keyframeEvents.Count > 0)
                    {
                        animationEventController.animationEvents[i].clipName = animationEventController.animationEvents[i].keyframeEvents[0].ID;
                    }
                    else
                        animationEventController.animationEvents[i].clipName = "empty name" + i.ToString();
                }
            }
        }

        private void InitializeAllClipInAnimator()
        {
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationUtility.SetAnimationEvents(animationEventController.animator.runtimeAnimatorController.animationClips[i], new AnimationEvent[0] { });        
            }
        }

        private void CompareWithClipNameInAnimator()
        {
            if (mismatchEvents == null)
                mismatchEvents = new List<AnimationEventController.AdvancedAnimationEvent>();

            if (matchEvents == null)
                matchEvents = new List<AnimationEventController.AdvancedAnimationEvent>();

            mismatchEvents.Clear();

            matchEvents.Clear();

            int length = animationEventController.animator.runtimeAnimatorController.animationClips.Length;

            foreach (var iter in animationEventController.animationEvents)
            {
                iter.clip = null;

                for (int i = 0; i < length; i++)
                {
                    if (iter.clipName == animationEventController.animator.runtimeAnimatorController.animationClips[i].name)
                    {
                        iter.clip = animationEventController.animator.runtimeAnimatorController.animationClips[i];
                        matchEvents.Add(iter);
                        break;
                    }
                }

                if(iter.clip == null)
                {
                    iter.clip = null;
                    mismatchEvents.Add(iter);
                }
            }
        }

        private void InitializeKeyframeEventsInClip()
        {
            for (int i = 0; i < matchEvents.Count; i++)
            {
                foreach(var iter in matchEvents[i].keyframeEvents)
                {
                    AddKeyframeEvent(matchEvents[i], iter.eventKeyframe);
                }
            }
        }

        private void Update()
        {
            if (workView == null || animationEventController == null || animationEventController.metaFile == null)
                return;

            propertiesView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                      , new Rect(0.0f, 0.0f, 0.3f, 1.0f));  


            workView.UpdateView( new Rect(position.width, position.height, position.width, position.height)
                                ,new Rect(0.3f, 0.0f, 0.7f, 1.0f));

        }

        private void OnGUI()
        {
            if (workView == null || animationEventController == null || animationEventController.metaFile == null)
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
            index = index < 0 ? 0 : index;
            index = index >= animationEventController.animationEvents.Count? animationEventController.animationEvents.Count - 1 : index;
           
            current.eventAnimationIndex = index;

            if (animationEventController.animationEvents.Count > 0)
                current.eventAnimation = animationEventController.animationEvents[current.eventAnimationIndex];
            else
                current.eventAnimation = null;

            workView.Initialize(animationEventController);
        }

        public SerializedProperty GetSerializedPropertyOfSelectedAnimation()
        {
            if (serializedObject == null)
                InitializeSerializedObject();

            if(serializedObject.FindProperty("animationEvents").arraySize > 0)
            {
                int index = FindEventAnimationIndex(current.eventAnimation);
                SerializedProperty serializedProperty = serializedObject.FindProperty("animationEvents").GetArrayElementAtIndex(index);

                return serializedProperty;                
            }
            return null;
        }


        //재검토a
        public int FindEventAnimationIndex(AnimationEventController.AdvancedAnimationEvent data)
        {
            if (data == null)
                return 0;
            
            int count = 0;
            foreach(var iter in animationEventController.animationEvents)
            {
                if (iter.clipName == data.clipName)
                    break;

                count++;
            }

            return count;
        }

        //TODO : 스트링 서치로 변경
        public bool AddEventAnimation(string clipName)
        {
            if (animationEventController.Contains(clipName))
                return false;

            // RecordObject("[AECE] Add Selected Animatin");

            AnimationClip clip = FindClipInAnimator(clipName);
            if (!clip)
                return false;
            
            AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = animationEventController.Add(clip.name);

            float frameCount = GetClipFrameCount(clipName);
            if(frameCount > 0.0f)
            {
                AddKeyframeEvent(advancedAnimationEvent, 0);
                AddKeyframeEvent(advancedAnimationEvent, (int)frameCount - 1);
            }

            return true;
        }

        public bool RemoveEventAnimation(string clipName)
        {
            if (!animationEventController.Contains(clipName))
                return false;

            //RecordObject("[AECE] Remove Selected Animation");

            if (animationEventController.Remove(clipName))
            {
                SelectAnimationEvent(animationEventController.animationEvents.Count - 1);

                AnimationClip clip = FindClipInAnimator(GetCurrentClipName());
                if (clip)
                {
                    AnimationUtility.SetAnimationEvents(clip, new AnimationEvent[0] { });
                }

                return true;
            }

            return false;
        }

        public bool AddKeyframeEvent(AnimationEventController.AdvancedAnimationEvent selected, int frame)
        {
            AnimationClip clip = FindClipInAnimator(selected.clipName);
            if (!clip)
                return false;
    
            if (!IsUnityAnimationEventInClip(clip, frame))
            {
                if (!IsEventFrameInAdvancedAnimationEvents(selected, frame))
                {
                    string eventName = selected.clipName + frame;
                    AnimationEventController.UnityKeyframeEvent newKeyframeEvent = new AnimationEventController.UnityKeyframeEvent(eventName, frame, null);
                    selected.keyframeEvents.Add(newKeyframeEvent);

                    AttachUnityAnimationEventToClip(newKeyframeEvent, clip);

                    return true;
                }

                for (int i = 0; i < selected.keyframeEvents.Count;i++)
                {
                    if(selected.keyframeEvents[i].eventKeyframe == frame)
                    {
                        AttachUnityAnimationEventToClip(selected.keyframeEvents[i], clip);
                        return true;
                    }
                }
            }

            //TODO : RecordObject("[AECE] Add keyframeEvent");

            return false;
        }

        private void AttachUnityAnimationEventToClip(AnimationEventController.UnityKeyframeEvent keyframeEvent ,AnimationClip clip)
        {
            //TODO : Undo.RecordObject(clip, "Add Event in clip");

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

        public bool IsEventFrameInAdvancedAnimationEvents(AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent, int frame)
        {
            AnimationEventController.AdvancedAnimationEvent animationEvent = advancedAnimationEvent;
            foreach(var iter in animationEvent.keyframeEvents)
            {
                if (iter.eventKeyframe == frame)
                    return true;
            }

            return false;
        }

        private bool IsUnityAnimationEventInClip(AnimationClip clip, int frame)
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
            AnimationClip clip = FindClipInAnimator(selected.clipName);

            int frameCount = GetClipFrameCount(selected.clipName);
            if (frame == 0 || frame == frameCount - 1)
                return false;

            //TODO : RecordObject("[AECE] Remove keyframeEvent");

            for (int i = 0; i < selected.keyframeEvents.Count; i++)
            {
                if (selected.keyframeEvents[i].eventKeyframe == frame)
                {
                    selected.keyframeEvents.Remove(selected.keyframeEvents[i]);

                    RemoveEventInClip(clip, frame);
                    return true;
                }
            }

            return false;
        }

        private void RemoveEventInClip(AnimationClip clip, int frame)
        {
            if (clip.events.Length <= 0)
                return;

            //TODO : Undo.RecordObject(selected.clip, "Remove Event in clip");

            AnimationEvent[] events = new AnimationEvent[clip.events.Length - 1];

            int index = 0;
            for (int i = 0; i < clip.events.Length; i++)
            {
                int currentFrame = (int)(clip.events[i].time * clip.frameRate);
                if (currentFrame == frame)
                    continue;

                events[index++] = clip.events[i];
            }
            
            AnimationUtility.SetAnimationEvents(clip, events);
        }

        public AnimationClip FindClipInAnimator(string name)
        {
            foreach(var iter in animationEventController.animator.runtimeAnimatorController.animationClips)
            {
                if (iter.name == name)
                    return iter;
            }

            return null;
        }

        public AnimationEventController.UnityKeyframeEvent SwapKeyFrameEvent(int targetIndex, AnimationEventController.UnityKeyframeEvent b)
        {
            AnimationEventController.UnityKeyframeEvent target = GetKeyframeEventInCurrentEventAnimation(targetIndex);
            AnimationEventController.UnityKeyframeEvent temp = new AnimationEventController.UnityKeyframeEvent(b.ID, b.eventKeyframe, b.onKeyframe);

            AnimationEventController.UnityKeyframeEvent result = null;
            if(target == null)
            {
                AddKeyframeEvent(current.eventAnimation, targetIndex);

                RemoveKeyframeEvent(current.eventAnimation, b.eventKeyframe);

                result = GetKeyframeEventInCurrentEventAnimation(targetIndex);

                result.onKeyframe = temp.onKeyframe;
            }
            else
            {
                b.onKeyframe = target.onKeyframe;
                target.onKeyframe = temp.onKeyframe;

                result = target;
            }

            return result;
        }

        public void MatchingAnimationClip(string clipName)
        {
            foreach(var iter in matchEvents)
            {
                if (iter.clipName == clipName)
                    RemoveEventAnimation(clipName);
            }

            current.eventAnimation.clipName = clipName;
            current.eventAnimation.clip = FindClipInAnimator(clipName);
        }

        public float GetClipLength(string clipName)
        {
            AnimationClip clip = FindClipInAnimator(clipName);

            return clip != null ? clip.length : DEFAULT_CLIP_LENGTH;
        }

        public float GetClipFrameRate(string clipName)
        {
            AnimationClip clip = FindClipInAnimator(clipName);

            return clip != null ? clip.frameRate : DEFAULT_CLIP_FRAMERATE;
        }

        public int GetClipFrameCount(string clipName)
        {
            AnimationClip clip = FindClipInAnimator(clipName);

            return clip != null ? (int)Mathf.Round(clip.length / (1.0f / clip.frameRate)) : DEFAULT_CLIP_FRAMECOUNT;
        }

        public float GetCurrentEventAnimationClipLength()
        {
            return GetClipLength(current.eventAnimation != null ? current.eventAnimation.clipName : string.Empty);
        }

        public float GetCurrentEventAnimationClipFrameRate()
        {
            return GetClipFrameRate(current.eventAnimation != null ? current.eventAnimation.clipName : string.Empty);
        }

        public int GetCurrentEventAnimationClipFrameCount()
        {
            return GetClipFrameCount(current.eventAnimation != null ? current.eventAnimation.clipName : string.Empty);
        }

        public int GetCurrentKeyframeEventCount()
        {
            return current.eventAnimation != null ? (current.eventAnimation.keyframeEvents != null ? current.eventAnimation.keyframeEvents.Count : 0) : 0;
        }

        public AnimationClip GetCurrentClip()
        {
            return FindClipInAnimator(current.eventAnimation.clipName);
        }

        public string GetCurrentClipName()
        {
            return current.eventAnimation != null ? current.eventAnimation.clipName : string.Empty;
        }

        public AnimationEventController.AdvancedAnimationEvent GetCurrentEventAnimation()
        {
            return current.eventAnimation;
        }

        public AnimationEventController.UnityKeyframeEvent GetKeyframeEventInCurrentEventAnimation(int frame)
        {
            foreach(var iter in current.eventAnimation.keyframeEvents)
            {
                if (iter.eventKeyframe == frame)
                    return iter;
            }

            return null;
        }
    }
}

/*
 *      Undo
       //public void OnEnable()
        //{
        //    Undo.undoRedoPerformed += UndoRedoPerformed;
        //}

        //public void OnDisable()
        //{
        //    Undo.undoRedoPerformed -= UndoRedoPerformed;
        //}

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
*/