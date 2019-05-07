using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    /*
     * TODO : [1] 서칭 알고리즘 보완
     *        [2]
    */
    public class AECWorkMatchingView : ViewBase
    {
        private AnimationEventController animationEventController;
        private int selectedIndex;

        private string[] allAnimationNames = new string[0];
        private int[] selectedIndexes = new int[0];
        private Vector2 scrollPosition = Vector2.zero;

        public void Initialize(AnimationEventController data)
        {
            animationEventController = data;

            selectedIndexes = new int[AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count];
            for (int i = 0; i < selectedIndexes.Length; i++)
            {
                selectedIndexes[i] = 0;
            }

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

            InitializeExpectationList();
        }

        private void InitializeExpectationList()
        {
            char c_split = '_';

            for (int i = 0; i < AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count; i++)
            {
                string targetName = AnimationEventControllerEditorWindow.Instance.mismatchEvents[i].clipName;
                string searchingName = string.Empty;
                string prevName = string.Empty;

                string[] splits = targetName.Split(c_split);
                searchingName = splits.Length > 0 ? splits[0] : string.Empty;
                for (int j = 0; j < splits.Length; j++)
                {
                    int searchingCount = 0;
                    int index = -1;
                   
                    for (int k = 0; k < allAnimationNames.Length; k++)
                    {
                        if (SearchString(allAnimationNames[k], searchingName))
                        {
                            searchingCount++;
                            index = k;
                        }
                    }

                    if (searchingCount <= 1)
                    {
                        selectedIndexes[i] = searchingCount <= 0 ? Searching(prevName, j > 0 ? "_" + splits[j] : splits[j]) : index;
                        break;
                    }

                    prevName = searchingName;
                    searchingName = j + 1 < splits.Length ? searchingName + ("_" + splits[j + 1]) : string.Empty;
                }
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
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                {
                    GUILayout.BeginVertical();
                    {
                        for (int i = 0; i < AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count; i++)
                        {
                            if (selectedIndexes[i] < 0)
                            {
                                GUI.Label(new Rect(EditorGUILayout.GetControlRect().x, EditorGUILayout.GetControlRect().y - 1.8f, 0.1f, 1.0f), new GUIContent(""), (GUIStyle)"CN EntryWarnIconSmall");
                            }
                            GUILayout.BeginHorizontal();
                            {

                                GUILayout.Label(AnimationEventControllerEditorWindow.Instance.mismatchEvents[i].clipName, GUILayout.Width(250.0f));
                                selectedIndexes[i] = EditorGUILayout.Popup(selectedIndexes[i], allAnimationNames);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Apply"))
                {
                    for (int i = 0; i < AnimationEventControllerEditorWindow.Instance.mismatchEvents.Count; i++)
                    {
                        AnimationEventControllerEditorWindow.Instance.MatchingAnimationClip(AnimationEventControllerEditorWindow.Instance.mismatchEvents[i], allAnimationNames[selectedIndexes[i]]);
                    }
                    AnimationEventControllerEditorWindow.Instance.Initialize(animationEventController);
                }
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }

        private bool SearchString(string str, string findStr)
        {
            if (str == null || findStr == null)
                return false;

            int found = 0, total = 0;

            for (int i = 0; i < str.Length; i++)
            {
                found = str.IndexOf(findStr, i, System.StringComparison.CurrentCulture);
                if (found >= 0)
                {
                    total++;
                    i = found;
                }
            }

            return total > 0 ? true : false;
        }

        private int Searching(string prev, string last)
        {
            int index = -1 , searchingCount = 0, perfect = -1;
            string text = prev;

            for (int i = 0; i < last.Length; i++)
            {
                text = text + last[i];
                for (int k = 0; k < allAnimationNames.Length; k++)
                {
                    if (text == allAnimationNames[k])
                        perfect = k;

                    if (SearchString(allAnimationNames[k], text))
                    {
                        searchingCount++;
                        index = k;
                    }
                }

                if (searchingCount <= 1)
                {
                    return index;
                }
            }

            return perfect;
        }
    }
}