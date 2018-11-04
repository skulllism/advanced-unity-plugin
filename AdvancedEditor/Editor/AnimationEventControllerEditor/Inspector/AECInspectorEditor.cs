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
        private Animator animator;

        public void OnEnable()
        {
            origin = (AnimationEventController)target;
        }

        public override void OnInspectorGUI()
        {
            if (origin == null)
                return;
            
            Space(10.0f);

            GUILayout.BeginVertical("box");
            {
                origin.animator = (Animator)EditorGUILayout.ObjectField(origin.animator, typeof(Animator),true);

                Space(10.0f);
                if(GUILayout.Button("Open Editor"))
                {
                    if(origin.animator != null)
                    {
                        if(origin.animator.runtimeAnimatorController != null)
                        {
                            AnimationEventControllerEditorWindow.OpenWindow(origin);        
                        }
                        else
                        {
                            Debug.LogError("[AnimationEventController] not found AnimationController");
                        }
                    }
                    else
                    {
                        Debug.LogError("[AnimationEventController] not found Animator");
                    }
                }
            }
            GUILayout.EndVertical();

           // base.OnInspectorGUI();
        }
    }
}