using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace AdvancedUnityPlugin.Editor
{
    [CustomEditor(typeof(AnimationEventController))]
    public class AECInspectorEditor : EditorBase
    {
        private AnimationEventController origin;

        //============================
        //  Animator Information
        //============================
        private int allAnimationCount;
        private string[] allAnimationNames = new string[0];
        private int allAnimationSelected;

        //============================
        //  AEC Information
        //============================
        private string[] registeredEventNames = new string[0];
        private int registeredEventSelected;

        private bool isDrawBaseInspector;

        public void OnEnable()
        {
            origin = (AnimationEventController)target;

            if (origin != null)
            {
                origin.animator = origin.gameObject.GetComponent<Animator>();

                if (origin.animator != null)
                {
                    if (origin.animator.runtimeAnimatorController != null)
                    {
                        RuntimeAnimatorController controller = origin.animator.runtimeAnimatorController;
                        allAnimationCount = controller.animationClips.Length;

                        InitializeAllAnimationNames(controller.animationClips);

                        InitializeRegisteredEventNames();
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (origin == null)
                return;

            Space(10.0f);

            isDrawBaseInspector = GUILayout.Toggle(isDrawBaseInspector, "Draw BaseInspector");

            Space(20.0f);

            GUILayout.BeginVertical("box");
            {
                origin.animator = (Animator)EditorGUILayout.ObjectField(origin.animator, typeof(Animator),true);
                if(origin.animator)
                {
                    if(origin.animator.runtimeAnimatorController)
                    {
#region Current Animatior Information
                        {
                            GUILayout.Box(new GUIContent("Animator Info"), (GUIStyle)"dragtabdropwindow");
                            DrawAnimatorInformation();
                        }
#endregion End Animator Information

                        Space(20.0f);

#region Animation Event Controller Information
                        GUILayout.Box(new GUIContent("AEC Info"), (GUIStyle)"dragtabdropwindow");
                        GUILayout.BeginVertical("box");
                        {
                            #region Current AEC MetaFile Information
                            {
                                DrawAECMetafileInformation();
                            }
                            #endregion End AEC MetaFile Information

                            DrawAECInformation();
                        }
                        GUILayout.EndVertical();
#endregion End Animation Event Controller Information
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Please add a AnimationController to Animator", MessageType.Error);    
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Please add a Animator", MessageType.Error);
                }
            }

            if(origin.metaFile)
            {
                Space(10.0f);
                ButtonOpenEditor();
            }

            GUILayout.EndVertical();

            if(isDrawBaseInspector)
                base.DrawDefaultInspector();
        }  

        private void InitializeAllAnimationNames(AnimationClip[] clips)
        {
            allAnimationNames = new string[clips.Length];

            for (int i = 0; i < clips.Length; i++)
                allAnimationNames[i] = clips[i].name;
        }

        private void InitializeRegisteredEventNames()
        {
            if (origin.animationEvents == null)
                origin.animationEvents = new List<AnimationEventController.AdvancedAnimationEvent>();
            
            registeredEventNames = new string[origin.animationEvents.Count];

            for (int i = 0; i < origin.animationEvents.Count; i++)
                registeredEventNames[i] = origin.animationEvents[i].clipName;
        }

        private void DrawAECMetafileInformation()
        {
            GUILayout.BeginHorizontal();
            {
                origin.metaFile = (AnimationEventControllerMetaFile)EditorGUILayout.ObjectField(origin.metaFile, typeof(AnimationEventControllerMetaFile), true);
                if (!origin.metaFile)
                {
                    if (GUILayout.Button("New File"))
                    {
                        string path = AssetDatabase.GetAssetPath(origin.animator.runtimeAnimatorController) + "Metafile.asset";

                        AnimationEventControllerMetaFile metaFile = ScriptableObject.CreateInstance<AnimationEventControllerMetaFile>();

                        AssetDatabase.CreateAsset(metaFile, path);

                        origin.metaFile = metaFile;
                        origin.metaFile.Initialize(path, origin.gameObject.scene.name, origin.gameObject.name, origin.gameObject.GetInstanceID());
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawAnimatorInformation()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("All Animation : " + allAnimationCount.ToString());

                    if(allAnimationNames.Length > 0)
                    {
                        allAnimationSelected = EditorGUILayout.Popup(allAnimationSelected, allAnimationNames);

                        if (GUILayout.Button("Find File"))
                        {
                           Selection.activeObject = origin.animator.runtimeAnimatorController.animationClips[allAnimationSelected];
                        }
                    }
                }
                GUILayout.EndHorizontal();

                Space(20.0f);
            }
            GUILayout.EndVertical();
        }

        private void DrawAECInformation()
        {
            int count = origin.animationEvents == null ? 0 : origin.animationEvents.Count;
            if (count != registeredEventNames.Length)
                InitializeRegisteredEventNames();

            GUILayout.BeginVertical("box");
            {
                Space(3.0f);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("", (GUIStyle)"TV Ping", GUILayout.Width(40.0f), GUILayout.Height(5.0f));
                    GUILayout.Label("Test");
                }
                GUILayout.EndHorizontal();

                Space(20.0f);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Resistered Events : " + count.ToString());

                    if(registeredEventNames.Length > 0)
                    {
                        registeredEventSelected = EditorGUILayout.Popup(registeredEventSelected, registeredEventNames);

                        if(GUILayout.Button("Find File"))
                        {
                            Selection.activeObject = AnimationEventControllerEditorWindow.Instance.FindClipInAnimator(origin.animationEvents[registeredEventSelected].clipName);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                if (origin.metaFile)
                {
                    Space(10.0f);

                    GUILayout.Label("Created  : " + origin.metaFile.GetCreationTime());
                    GUILayout.Label("Modified : " + origin.metaFile.GetLatelyModifiedTime());
                }
            }
            GUILayout.EndVertical();
        }

        private void ButtonOpenEditor()
        {
            if (GUILayout.Button("Open Editor"))
            {
                if (origin.animationEvents == null)
                    origin.animationEvents = new List<AnimationEventController.AdvancedAnimationEvent>();

                if (origin.animator.runtimeAnimatorController.animationClips.Length > 0)
                    AnimationEventControllerEditorWindow.OpenWindow(origin);
            }
        }
    }
}