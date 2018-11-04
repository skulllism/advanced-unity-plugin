using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkTimelineView : ViewBase
    {
        private int currentFrameIndex;

        private float lineInterval;
        private float frameInterval;
        private float frameCount;
        private float frameRate;
        private Vector2 scrollPosition;


        public void Initialize()
        {
            
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);

            frameRate = AnimationEventControllerEditorWindow.Instance.selected.clip.frameRate;

            frameInterval = 1.0f / frameRate;

            frameCount = AnimationEventControllerEditorWindow.Instance.selected.clip.length / frameInterval;
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            GUILayout.BeginArea(viewRect, "", (GUIStyle)"MeBlendBackground");
            {
                DrawLine();
 
                Handles.DrawLine(new Vector3(0.0f, viewRect.height * 0.45f, 0), new Vector3(1000.0f, viewRect.height* 0.45f, 0));
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {

        }

        public void SetCurrentFrameIdex(int index)
        {
            currentFrameIndex = index;
        }

        private float startLine = 40.0f;
        private float labelStartX = 1.5f;
        private float labelStartY = -6.5f;
        private void DrawLine()
        {
            //뷰의 width / 해당 클립의 길이 
            if(frameRate < frameCount)
                lineInterval = (viewRect.width / frameRate);
            else
                lineInterval = (viewRect.width / frameCount);
            
            Handles.color = Color.black;

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            {
                GUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < frameCount; i++)
                    {
                        GUILayout.Space((lineInterval));
                    }
                }
                GUILayout.EndHorizontal();

                //해당 클립의 프레임 수 만큼 보여준다 
                for (int i = 0; i < frameCount; i++)
                {
                    Handles.DrawLine(new Vector3(startLine + (lineInterval * i), viewRect.height * 0.2f, 0), new Vector3(startLine + (lineInterval * i), viewRect.height * 0.45f, 0));

                    Handles.Label(new Vector3(startLine + (lineInterval * i) + labelStartX, (viewRect.height * 0.15f) + labelStartY, 0), new GUIContent((i).ToString()));

                    DrawKeyframeEventIcon(i, new Vector2(startLine + (lineInterval * i), viewRect.height * 0.5f));
                }

                Handles.DrawBezier(  new Vector3(GetFramePositionX(currentFrameIndex), 0.0f, 0.0f)
                                   , new Vector3(GetFramePositionX(currentFrameIndex), viewRect.height * 0.84f, 0.0f)
                                   , new Vector3(GetFramePositionX(currentFrameIndex), 0.0f, 0.0f)
                                   , new Vector3(GetFramePositionX(currentFrameIndex), viewRect.height * 0.84f, 0.0f)
                                   , Color.white
                                   , null
                                   , 2.0f
                  );
            }
            GUILayout.EndScrollView();
        }

        private void DrawKeyframeEventIcon(int frame, Vector2 position)
        {
            AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = AnimationEventControllerEditorWindow.Instance.selected;
            if (advancedAnimationEvent == null)
                return;

            for (int i = 0; i < advancedAnimationEvent.keyframeEvents.Count; i++)
            {
                if (advancedAnimationEvent.keyframeEvents[i].eventKeyframe == frame)
                {
                    if(currentFrameIndex == frame)
                        GUI.Button(new Rect(position.x, position.y, 10.0f, 10.0f), new GUIContent(""),(GUIStyle)"TL Playhead");
                    else
                        GUI.Button(new Rect(position.x - 2.3f, position.y - 1.8f, 10.0f, 10.0f), new GUIContent(""), (GUIStyle)"Icon.Event");
                }
            }
        }

        public bool IsInView(Vector2 mousePosition)
        {
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, viewRect.height * 0.8f);
            bool result = rect.Contains(mousePosition);

            return result;
        }

        public bool IsInTimelineView(Vector2 mousePosition)
        {
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, viewRect.height * 0.45f);
            bool result = rect.Contains(mousePosition);
 
            return result;
        }

        public bool IsInKeyframeView(Vector2 mousePosition)
        {
            Rect rect = new Rect(viewRect.x, viewRect.height * 0.45f, viewRect.width, viewRect.height * 0.55f);
            bool result = rect.Contains(mousePosition);

            return result;
        }

        public float GetFramePositionX(int index)
        {
            return startLine + (lineInterval * index);
        }

        public int GetFrameIndexOfRange(Vector2 mousePosition)
        {
            for (int i = 0; i < frameCount; i++)
            {
                Rect rect = new Rect( startLine  + (lineInterval * i) - (lineInterval * 0.5f)
                                     , 0.0f
                                     , lineInterval
                                     , viewRect.height * 0.84f);

                if(rect.Contains(new Vector2(mousePosition.x + scrollPosition.x, mousePosition.y + scrollPosition.y)))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}