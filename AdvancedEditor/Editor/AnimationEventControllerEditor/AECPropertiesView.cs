using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECPropertiesView : ViewBase
    {
        private AnimationEventController animationEventController;
        private RuntimeAnimatorController runtimeAnimatorController;

        //===========================================
        //  ##  Animation Preview
        //===========================================
        private bool  isAutoSampling;
        private float interval;
        private float samplingTime;
        private float startSamplingTime;
        private float elapseSamplingTime;
        private string strCurrentSamplingIndex;

        //===========================================
        //  ##  Selected Animation - Popup Control
        //===========================================
        private string[] allAnimationNames   = new string[0];
        private string[] eventAnimationNames = new string[0];
        private int   selectedAllAnimationIndex;
      
        public void Initialize(AnimationEventController data)
        {
            animationEventController = data;
            runtimeAnimatorController = animationEventController.animator.runtimeAnimatorController;

            InitializeAllAnimationsPopup();
            InitializeEventAnimationsPopup();

            AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(AnimationEventControllerEditorWindow.Instance.current.eventAnimationIndex);
        }

        private void InitializeAllAnimationsPopup()
        {
            int popupSize = 0;
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[i] == null)
                    continue;

                popupSize++;

                foreach (var iter in animationEventController.animationEvents)
                {
                    if (iter.clipName == animationEventController.animator.runtimeAnimatorController.animationClips[i].name)
                    {
                        popupSize--;
                        break;
                    }
                }
            }

            allAnimationNames = new string[popupSize];


            int count = 0;
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[i] == null)
                    continue;

                if(IsClipInEvents(animationEventController.animator.runtimeAnimatorController.animationClips[i].name))
                {
                    continue;
                }

                allAnimationNames[count++] = animationEventController.animator.runtimeAnimatorController.animationClips[i].name;
            }

            selectedAllAnimationIndex = 0;
        }

        //필요하나 ?? 에디터 윈도우에 존재하는데 ?
        private bool IsClipInEvents(string name)
        {
            foreach (var iter in animationEventController.animationEvents)
            {
                if (iter.clipName == name)
                {
                    return true;
                }
            }  

            return false;
        }

        private void InitializeEventAnimationsPopup()
        {
            int popupSize = AnimationEventControllerEditorWindow.Instance.matchEvents.Count + AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count;
            if (popupSize <= 0)
            {
                eventAnimationNames = new string[0];
                return;
            }

            int count = 0;
            eventAnimationNames = new string[popupSize];
            for (int i = 0; i < popupSize; i++)
            {
                foreach(var match in AnimationEventControllerEditorWindow.Instance.matchEvents)
                {
                    if(match == animationEventController.animationEvents[i])
                    {
                        eventAnimationNames[i] = match.clipName;
                        break;
                    }
                }

                foreach(var misMatch in AnimationEventControllerEditorWindow.Instance.mismatchEvents)
                {
                    if (misMatch == animationEventController.animationEvents[i])
                    {
                        eventAnimationNames[i] = "[Mismatch] empty clip " + count.ToString() + "-" + misMatch.clipName;
                        break;
                    }
                }

                count++;
            }

            InitializeCurrentAnimationData();
        }

        private void InitializeCurrentAnimationData()
        {
            interval = animationEventController.animationEvents.Count > 0 ? 1.0f / AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameRate() : 0.0f;
            samplingTime = 0.0f;
            isAutoSampling = false;
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);

            if(!EditorApplication.isPlaying && AnimationMode.InAnimationMode())
            {
                if(isAutoSampling)
                {
                    elapseSamplingTime = Time.realtimeSinceStartup;
                    if(elapseSamplingTime - startSamplingTime >= interval)
                    {
                        AutoSampling();    
                    }
                }

                AnimationMode.BeginSampling();
                {
                    samplingTime = interval * AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex;

                    AnimationClip clip = AnimationEventControllerEditorWindow.Instance.GetCurrentClip();

                    AnimationMode.SampleAnimationClip(animationEventController.animator.gameObject, clip, samplingTime);
                }
                AnimationMode.EndSampling();

                SceneView.RepaintAll();
            }
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            GUILayout.BeginArea(viewRect, "", "box");
            {
                GUILayout.BeginVertical();
                {
                    DrawFirstBar();
                    DrawSecondBar();
                    GUILayout.Space(20.0f);
                    DrawAddAnimationBox();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }

        private void DrawFirstBar()
        {
            GUILayout.BeginHorizontal("box");
            {
                if(GUILayout.Button(new GUIContent("Preview"), (GUIStyle)"PreButton", GUILayout.MinWidth(50.0f)))
                {
                    ToggleAnimationMode();
                }
                else if(GUILayout.Button(new GUIContent("l︎◀︎◀︎"), (GUIStyle)"toolbarbutton"))
                {
                    StartAnimationMode();

                    MoveStartSamplingTime();
                }
                else if(GUILayout.Button(new GUIContent("l◀︎"), (GUIStyle)"toolbarbutton"))
                {
                    StartAnimationMode();

                    MovePrevSamplingTime();
                }
                else if(GUILayout.Button(new GUIContent("︎▶︎"), (GUIStyle)"toolbarbutton"))
                {
                    StartAnimationMode();

                    isAutoSampling = !isAutoSampling;
                }
                else if(GUILayout.Button(new GUIContent("▶︎︎l"), (GUIStyle)"toolbarbutton"))
                {
                    StartAnimationMode();

                    MoveNextSamplingTime();
                }
                else if(GUILayout.Button(new GUIContent("▶︎︎▶︎︎l"), (GUIStyle)"toolbarbutton"))
                {
                    StartAnimationMode();

                    MoveFinishSamplingTime();
                }


                strCurrentSamplingIndex = AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex.ToString();
                EditorGUI.BeginChangeCheck();
                {
                    strCurrentSamplingIndex = GUILayout.TextField(strCurrentSamplingIndex);    
                }
                if(EditorGUI.EndChangeCheck())
                {
                    int.TryParse(strCurrentSamplingIndex, out AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex);   
                }
            }
            GUILayout.EndHorizontal();
        }

        private void ToggleAnimationMode()
        {
            if (AnimationMode.InAnimationMode())
                StopAnimationMode();
            else
                StartAnimationMode();
        }

        private void StartAnimationMode()
        {
            if (!AnimationMode.InAnimationMode())
            {
                AnimationMode.StartAnimationMode();
            }
        }

        private void StopAnimationMode()
        {
            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.StopAnimationMode();
                InitializeCurrentAnimationData();
            }
        }

        private void AutoSampling()
        {
            startSamplingTime = elapseSamplingTime = Time.realtimeSinceStartup;

            AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex++;
            //TODO : 함수화
            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex;
            if (samplingTime >= AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipLength())
            {
                MoveStartSamplingTime();
            }

            AnimationEventControllerEditorWindow.Instance.Repaint();
        }

        private void MoveNextSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex++;

            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex;
            if (samplingTime >= AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipLength())
            {
                MoveFinishSamplingTime();
            }
        }

        private void MoveFinishSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = (int)((AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipLength() / interval) - 1.0f);

            samplingTime = AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex * interval;
        }

        private void MovePrevSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex--;

            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex;
            if (samplingTime < 0.0f)
            {
                MoveStartSamplingTime();
            }
        }

        private void MoveStartSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = 0;

            samplingTime = 0.0f;
        }

        private void DrawSecondBar()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Space(10.0f);

                if (AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUI.Label(new Rect(EditorGUILayout.GetControlRect().x, EditorGUILayout.GetControlRect().y - 1.8f, 0.1f, 1.0f), new GUIContent(""), (GUIStyle)"CN EntryWarnIconSmall");
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        if(eventAnimationNames.Length > 0)
                        {
                            AnimationEventControllerEditorWindow.Instance.current.eventAnimationIndex = EditorGUILayout.Popup(AnimationEventControllerEditorWindow.Instance.current.eventAnimationIndex, eventAnimationNames, (GUIStyle)"PreDropDown");
                        }
                        else
                            EditorGUILayout.Popup(0, new string[] {"Empty"}, (GUIStyle)"PreDropDown");
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(AnimationEventControllerEditorWindow.Instance.current.eventAnimationIndex);

                        InitializeCurrentAnimationData();
                    }

                    GUILayout.Label("Count : ");
                    GUILayout.Label(animationEventController.animationEvents.Count.ToString());
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(new GUIContent("Find Clip")))
                    {
                        if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation != null)
                        {
                            //함수화a
                            AnimationClip clip =  AnimationEventControllerEditorWindow.Instance.FindClipInAnimator(AnimationEventControllerEditorWindow.Instance.current.eventAnimation.clipName);
                            Selection.activeObject = clip;
                        }
                    }

                    if (GUILayout.Button(new GUIContent("Find Hierarchy Object")))
                    {
                        if (AnimationEventControllerEditorWindow.Instance.animationEventController != null)
                            Selection.activeGameObject = AnimationEventControllerEditorWindow.Instance.animationEventController.gameObject;
                    }

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(15.0f);


                GUILayout.Box(new GUIContent("Information"), (GUIStyle)"dragtabdropwindow");

                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("FrameRate : ");
                        GUILayout.Label(AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameRate().ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        int frameCount = AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameCount();
                        GUILayout.Label("FrameCount : ");
                        GUILayout.Label(frameCount.ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("KeyframeEvenets : ");
                        GUILayout.Label(AnimationEventControllerEditorWindow.Instance.GetCurrentKeyframeEventCount().ToString());
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                if (GUILayout.Button(new GUIContent("Remove")))
                {
                    if (AnimationEventControllerEditorWindow.Instance.RemoveEventAnimation(AnimationEventControllerEditorWindow.Instance.GetCurrentClipName()))
                    {
                        AnimationEventControllerEditorWindow.Instance.Initialize(animationEventController);
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private void DrawAddAnimationBox()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Box(new GUIContent("Create Event"), (GUIStyle)"dragtabdropwindow");

                GUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label("All Animations");

                    GUILayout.BeginVertical();
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            if (allAnimationNames.Length > 0)
                                selectedAllAnimationIndex = EditorGUILayout.Popup(selectedAllAnimationIndex, allAnimationNames);
                            else
                                EditorGUILayout.Popup(selectedAllAnimationIndex, new string[] { "Empty"});
                        }
                        if (EditorGUI.EndChangeCheck())
                        {

                        }

                        GUILayout.Space(10.0f);
                        if(GUILayout.Button(new GUIContent("+")))
                        {
                            if(allAnimationNames.Length > 0)
                            {
                                if (AnimationEventControllerEditorWindow.Instance.AddEventAnimation(FindIndex(allAnimationNames[selectedAllAnimationIndex])))
                                {
                                    AnimationEventControllerEditorWindow.Instance.Initialize(animationEventController);

                                    AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(animationEventController.animationEvents.Count - 1);
                                }   
                            }
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        //TODO : 임시
        private string FindIndex(string name)
        {
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if(animationEventController.animator.runtimeAnimatorController.animationClips[i].name == name)
                {
                    return animationEventController.animator.runtimeAnimatorController.animationClips[i].name;    
                }
            }

            return string.Empty;
        }
    }
}