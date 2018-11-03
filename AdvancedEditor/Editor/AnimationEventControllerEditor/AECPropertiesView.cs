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
        private float currentSamplingIndex;
        private float startSamplingTime;
        private float elapseSamplingTime;

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

            InitializeCurrentAnimationData();

            AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(selectedEventAnimationIndex);
        }

        private void InitializeAllAnimationsPopup()
        {
            int popupSize = animationEventController.animator.runtimeAnimatorController.animationClips.Length;
 
            allAnimationNames = new string[popupSize];
            for (int i = 0; i < popupSize; i++)
            {
                allAnimationNames[i] = animationEventController.animator.runtimeAnimatorController.animationClips[i].name;
            }

            selectedAllAnimationIndex = 0;
        }

        private void InitializeEventAnimationsPopup()
        {
            int popupSize = animationEventController.animationEvents.Count;

            eventAnimationNames = new string[popupSize];
            for (int i = 0; i < popupSize; i++)
            {
                eventAnimationNames[i] = animationEventController.animationEvents[i].clip.name;
            }

            selectedEventAnimationIndex = 0;
            selectedFrameRate = animationEventController.animationEvents[selectedEventAnimationIndex].clip.frameRate;
            //TODO : Instance.select .....
        }

        private void InitializeCurrentAnimationData()
        {
            selectedFrameRate = animationEventController.animationEvents[selectedEventAnimationIndex].clip.frameRate;

            interval = 1.0f / animationEventController.animationEvents[selectedEventAnimationIndex].clip.frameRate;

            samplingTime = 0.0f;
            currentSamplingIndex = 0.0f;
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

                string strCurrentSamplingIndex = currentSamplingIndex.ToString();
                EditorGUI.BeginChangeCheck();
                {
                    strCurrentSamplingIndex = GUILayout.TextField(strCurrentSamplingIndex);    
                }
                if(EditorGUI.EndChangeCheck())
                {
                    float.TryParse(strCurrentSamplingIndex, out currentSamplingIndex);   
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

            currentSamplingIndex++;

            samplingTime = interval * currentSamplingIndex;
            if (samplingTime >= animationEventController.animationEvents[selectedEventAnimationIndex].clip.length)
            {
                MoveStartSamplingTime();
            }
        }

        private void MoveNextSamplingTime()
        {
            currentSamplingIndex++;

            samplingTime = interval * currentSamplingIndex;
            if (samplingTime >= animationEventController.animationEvents[selectedEventAnimationIndex].clip.length)
            {
                MoveFinishSamplingTime();
            }
        }

        private void MoveFinishSamplingTime()
        {
            currentSamplingIndex = (animationEventController.animationEvents[selectedEventAnimationIndex].clip.length / interval) - 1.0f;

            samplingTime = currentSamplingIndex * interval;
        }

        private void MovePrevSamplingTime()
        {
            currentSamplingIndex--;

            samplingTime = interval * currentSamplingIndex;
            if (samplingTime < 0.0f)
            {
                MoveStartSamplingTime();
            }
        }

        private void MoveStartSamplingTime()
        {
            currentSamplingIndex = 0.0f;

            samplingTime = 0.0f;
        }

        private void DrawSecondBar()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Box(new GUIContent("Information"), (GUIStyle)"dragtabdropwindow");

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Current Count : ");
                    GUILayout.Label(animationEventController.animationEvents.Count.ToString());
                    GUILayout.Space(10.0f);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        selectedEventAnimationIndex = EditorGUILayout.Popup(selectedEventAnimationIndex, eventAnimationNames);    
                    }
                    if(EditorGUI.EndChangeCheck())
                    {
                        AnimationEventControllerEditorWindow.Instance.SelectAnimationEvent(selectedEventAnimationIndex);

                        InitializeCurrentAnimationData();
                    }


                    GUILayout.Label("samples : ");
                    GUILayout.Label(selectedFrameRate.ToString());
                }
                GUILayout.EndHorizontal();

                if(GUILayout.Button(new GUIContent("Remove")))
                {
                    if (AnimationEventControllerEditorWindow.Instance.RemoveSelectedAnimation())
                    {
                        InitializeEventAnimationsPopup();
                        InitializeCurrentAnimationData();
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
                            selectedAllAnimationIndex = EditorGUILayout.Popup(selectedAllAnimationIndex, allAnimationNames);
                        }
                        if (EditorGUI.EndChangeCheck())
                        {

                        }

                        GUILayout.Space(10.0f);
                        if(GUILayout.Button(new GUIContent("+")))
                        {
                            if(AnimationEventControllerEditorWindow.Instance.AddNewEventAnimation(selectedAllAnimationIndex))
                                InitializeEventAnimationsPopup();
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}