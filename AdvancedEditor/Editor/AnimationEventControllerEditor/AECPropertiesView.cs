using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECPropertiesView : ViewBase
    {
        private AnimationEventController animationEventController;

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
        private string[] allAnimationNames;
        private string[] eventAnimationNames;
        private int   selectedAllAnimationIndex;
        private int   selectedEventAnimationIndex;
        private float selectedFrameRate;

        public void Initialize()
        {
            animationEventController = AnimationEventControllerEditorWindow.Instance.animationEventController;
            InitializeAllAnimationsPopup();
            InitializeEventAnimationsPopup();

          
            AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(selectedEventAnimationIndex);
        }

        public void SetSelectAnimationIndex(int index)
        {
            selectedEventAnimationIndex = index;
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
                    if (iter.clip.name == animationEventController.animator.runtimeAnimatorController.animationClips[i].name)
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

        private bool IsClipInEvents(string name)
        {
            foreach (var iter in animationEventController.animationEvents)
            {
                if (iter.clip.name == name)
                {
                    return true;
                }
            }  

            return false;
        }

        private void InitializeEventAnimationsPopup()
        {
            if (animationEventController.animationEvents == null)
                return;
            
            int popupSize = animationEventController.animationEvents.Count;
 
            eventAnimationNames = new string[popupSize];
            for (int i = 0; i < popupSize; i++)
            {
                eventAnimationNames[i] = animationEventController.animationEvents[i].clip.name;
            }

            AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(selectedEventAnimationIndex);

            if (popupSize <= 0)
                return;

            selectedFrameRate = AnimationEventControllerEditorWindow.Instance.GetSelectedClipFrameRate();
            //TODO : Instance.select .....

            InitializeCurrentAnimationData();
        }

        private void InitializeCurrentAnimationData()
        {
            if(animationEventController.animationEvents.Count > 0)
            {
                selectedFrameRate = AnimationEventControllerEditorWindow.Instance.GetSelectedClipFrameRate();
                interval = 1.0f / AnimationEventControllerEditorWindow.Instance.GetSelectedClipFrameRate();
            }
            else
            {
                selectedFrameRate = 0;
                interval = 0.0f;    
            }

            samplingTime = 0.0f;
            isAutoSampling = false;

            AnimationEventControllerEditorWindow.Instance.currentFrameIndex = 0;
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
                    samplingTime = interval * AnimationEventControllerEditorWindow.Instance.currentFrameIndex;
                    AnimationMode.SampleAnimationClip(animationEventController.animator.gameObject, animationEventController.animationEvents[selectedEventAnimationIndex].clip, samplingTime);
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


                strCurrentSamplingIndex = AnimationEventControllerEditorWindow.Instance.currentFrameIndex.ToString();
                EditorGUI.BeginChangeCheck();
                {
                    strCurrentSamplingIndex = GUILayout.TextField(strCurrentSamplingIndex);    
                }
                if(EditorGUI.EndChangeCheck())
                {
                    
                    int.TryParse(strCurrentSamplingIndex, out AnimationEventControllerEditorWindow.Instance.currentFrameIndex);   
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

            AnimationEventControllerEditorWindow.Instance.currentFrameIndex++;

            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.currentFrameIndex;
            if (samplingTime >= AnimationEventControllerEditorWindow.Instance.GetSelectedClipLength())
            {
                MoveStartSamplingTime();
            }

            AnimationEventControllerEditorWindow.Instance.Repaint();
        }

        private void MoveNextSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.currentFrameIndex++;

            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.currentFrameIndex;
            if (samplingTime >= AnimationEventControllerEditorWindow.Instance.GetSelectedClipLength())
            {
                MoveFinishSamplingTime();
            }
        }

        private void MoveFinishSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.currentFrameIndex = (int)((AnimationEventControllerEditorWindow.Instance.GetSelectedClipLength() / interval) - 1.0f);

            samplingTime = AnimationEventControllerEditorWindow.Instance.currentFrameIndex * interval;
        }

        private void MovePrevSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.currentFrameIndex--;

            samplingTime = interval * AnimationEventControllerEditorWindow.Instance.currentFrameIndex;
            if (samplingTime < 0.0f)
            {
                MoveStartSamplingTime();
            }
        }

        private void MoveStartSamplingTime()
        {
            AnimationEventControllerEditorWindow.Instance.currentFrameIndex = 0;

            samplingTime = 0.0f;
        }

        private void DrawSecondBar()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Space(10.0f);
                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        if(eventAnimationNames.Length > 0)
                            selectedEventAnimationIndex = EditorGUILayout.Popup(selectedEventAnimationIndex, eventAnimationNames, (GUIStyle)"PreDropDown");
                        else
                            EditorGUILayout.Popup(0, new string[] {"Empty"}, (GUIStyle)"PreDropDown");
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(selectedEventAnimationIndex);

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
                        if (AnimationEventControllerEditorWindow.Instance.selected != null)
                            Selection.activeObject = AnimationEventControllerEditorWindow.Instance.selected.clip;
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
                        GUILayout.Label(selectedFrameRate.ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        int frameCount = (int)(AnimationEventControllerEditorWindow.Instance.GetSelectedClipLength() / (1.0f / AnimationEventControllerEditorWindow.Instance.GetSelectedClipFrameRate()));
                        GUILayout.Label("FrameCount : ");
                        GUILayout.Label(frameCount.ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("KeyframeEvenets : ");
                        GUILayout.Label(AnimationEventControllerEditorWindow.Instance.GetSelectedKeyframeEventsCount().ToString());
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                if (GUILayout.Button(new GUIContent("Remove")))
                {
                    if (AnimationEventControllerEditorWindow.Instance.RemoveSelectedAnimation())
                    {
                        selectedEventAnimationIndex = animationEventController.animationEvents.Count - 1;

                        selectedEventAnimationIndex = selectedEventAnimationIndex < 0 ? 0 : selectedEventAnimationIndex;

                        InitializeEventAnimationsPopup();
                        InitializeCurrentAnimationData();

                        InitializeAllAnimationsPopup();
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
                                if (AnimationEventControllerEditorWindow.Instance.AddNewEventAnimation(FindIndex(allAnimationNames[selectedAllAnimationIndex])))
                                {
                                    AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(animationEventController.animationEvents.Count - 1);

                                    selectedEventAnimationIndex = animationEventController.animationEvents.Count - 1;

                                    InitializeCurrentAnimationData();
                                    InitializeEventAnimationsPopup();

                                    InitializeAllAnimationsPopup();
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
        private int FindIndex(string name)
        {
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if(animationEventController.animator.runtimeAnimatorController.animationClips[i].name == name)
                {
                    return i;    
                }
            }

            return 0;
        }
    }
}