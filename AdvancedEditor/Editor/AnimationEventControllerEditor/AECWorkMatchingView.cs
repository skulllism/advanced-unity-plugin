using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkMatchingView : ViewBase
    {
        private AnimationEventController animationEventController;
        private int selectedIndex;

        private string[] allAnimationNames;

        public void Initialize(AnimationEventController data)
        {
            animationEventController = data;

            int length = 0;
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[i] == null)
                    continue;

                length++;
            }

            int count = 0;

            allAnimationNames = new string[length];
            for (int i = 0; i < animationEventController.animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animationEventController.animator.runtimeAnimatorController.animationClips[i] == null)
                    continue;

                allAnimationNames[count++] = animationEventController.animator.runtimeAnimatorController.animationClips[i].name;
            }
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            GUILayout.BeginArea(viewRect, "", "box");
            {
                GUILayout.BeginVertical();
                {
                    selectedIndex = EditorGUILayout.Popup(selectedIndex, allAnimationNames);  

                    if(GUILayout.Button("Apply"))
                    {
                        AnimationEventControllerEditorWindow.Instance.MatchingAnimationClip(allAnimationNames[selectedIndex]);
                        AnimationEventControllerEditorWindow.Instance.Initialize(animationEventController);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }
    }
}