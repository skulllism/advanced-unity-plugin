using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        private string[] allAnimationNames;
        private int allAnimationSelected;

        //============================
        //  AEC Information
        //============================
        private string[] registeredEventNames;
        private int registeredEventSelected;

        public void OnEnable()
        {
            origin = (AnimationEventController)target;

            if (origin != null)
            {
                origin.animator = origin.gameObject.GetComponent<Animator>();

                if (origin.creationDate == string.Empty)
                    origin.creationDate = System.DateTime.UtcNow.ToString();

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
            Debug.Log(origin.animator.runtimeAnimatorController.GetInstanceID());
            GUILayout.BeginVertical("box");
            {
                origin.animator = (Animator)EditorGUILayout.ObjectField(origin.animator, typeof(Animator),true);
                if(origin.animator)
                {
                    if(origin.animator.runtimeAnimatorController)
                    {
#region Current Animatior Information
                        {
                            DrawAnimatorInformation();
                        }
#endregion End Animator Information

                        Space(20.0f);

#region Animation Event Controller Information
                        {
                            GUILayout.Box(new GUIContent("AEC Infomation"), (GUIStyle)"dragtabdropwindow");

                            DrawAECInformation();
                        }
#endregion End Animation Event Controller Information

                        ButtonOpenEditor();
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
            GUILayout.EndVertical();
        }  

        private void InitializeAllAnimationNames(AnimationClip[] clips)
        {
            allAnimationNames = new string[clips.Length];

            for (int i = 0; i < clips.Length; i++)
                allAnimationNames[i] = clips[i].name;
        }

        private void InitializeRegisteredEventNames()
        {
            registeredEventNames = new string[origin.animationEvents.Count];

            for (int i = 0; i < origin.animationEvents.Count; i++)
                registeredEventNames[i] = origin.animationEvents[i].clip.name;
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
                GUILayout.Label("Creation Date : " + origin.creationDate);
                GUILayout.Label("InstanceID : " + origin.GetInstanceID());
                //string stse = AssetDatabase.GetAssetPath(-1);
                //AssetDatabase.GetAssetPath()
                Space(10.0f);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Resistered Events : " + count.ToString());

                    if(registeredEventNames.Length > 0)
                    {
                        registeredEventSelected = EditorGUILayout.Popup(registeredEventSelected, registeredEventNames);

                        if(GUILayout.Button("Find File"))
                        {
                            Selection.activeObject = origin.animationEvents[registeredEventSelected].clip;
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void ButtonOpenEditor()
        {
            if (GUILayout.Button("Open Editor"))
            {
                if (origin.animator.runtimeAnimatorController.animationClips.Length > 0)
                    AnimationEventControllerEditorWindow.OpenWindow(origin);
            }
        }
    }
}