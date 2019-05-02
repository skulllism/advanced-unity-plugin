using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkView : ViewBase
    {
        private AnimationEventController animationEventController;

        private AECWorkTimelineView workTimelineView;
        private AECWorkBaseEventModifyView baseEventModifyView;
        private AECWorkKeyFrameEventModifyView keyFrameEventModifyView;
        public AECWorkMatchingView matchingView;

        private AnimationEventController.UnityKeyframeEvent selectedKeyframeEvent;

        private bool isDragged;
        private bool isEventDragged;

        public void Initialize(AnimationEventController data)
        {
            animationEventController = data;

            selectedKeyframeEvent = null;

            InitializeSubView();
        }

        private void InitializeSubView()
        {
            if (workTimelineView == null)
            {
                workTimelineView = new AECWorkTimelineView();
                baseEventModifyView = new AECWorkBaseEventModifyView();
                keyFrameEventModifyView = new AECWorkKeyFrameEventModifyView();
                matchingView = new AECWorkMatchingView();
            }

            workTimelineView.Initialize();
            baseEventModifyView.Initialize();
            keyFrameEventModifyView.Initialize();
            matchingView.Initialize(animationEventController);
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            if (workTimelineView == null)
                return;

            base.UpdateView(editorRect, percentageRect);

            if(AnimationEventControllerEditorWindow.Instance.current.eventAnimation != null)
            {
                if(AnimationEventControllerEditorWindow.Instance.current.eventAnimation.clip != null)
                {
                    workTimelineView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                        , new Rect(0.0f, 0.0f, 1.0f, 0.2f));


                    baseEventModifyView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                                    , new Rect(0.0f, 0.2f, 0.5f, 0.9f));

                    keyFrameEventModifyView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                                    , new Rect(0.5f, 0.2f, 0.5f, 0.9f));
                }
                else
                {
                    matchingView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                            , new Rect(0.0f, 0.0f, 1.0f, 1.0f));
                }
            }
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation != null)
            {
                if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation.clip != null)
                {
                    ProcessEvents(e);
                }

            }

            GUILayout.BeginArea(viewRect, "", "box");
            {
                if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation != null)
                {
                    if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation.clip != null)
                    {
                        workTimelineView.SetCurrentFrameIdex(AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex);
                        workTimelineView.GUIView(e);
                        baseEventModifyView.GUIView(e);

                        keyFrameEventModifyView.SetCurrentFrameIndex(AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex);
                        keyFrameEventModifyView.GUIView(e);
                    }
                    else
                    {
                        matchingView.GUIView(e);
                    }
                }
            }
            GUILayout.EndArea();
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
      
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        if(workTimelineView.IsInTimelineView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            if(e.button == 0)
                            {
                                int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                                if (index != -1)
                                {
                                    AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = index;
                                    isDragged = true;
                                    e.Use();
                                }
                            }           
                        }
                        else if(workTimelineView.IsInKeyframeView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            //선택한 곳에 EventAnimation이 존재하면 
                            //드래그 된 곳에 EventAnimation이 존재하면 스왑 1,마지막프레임 제외
                            //혹은 이동
                            if(e.button == 0)
                            {
                                int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                                if (index != -1)
                                {
                                    //선택한 곳에 eventAnimation이 존재 하면 ?
                                    AnimationEventController.UnityKeyframeEvent keyframeEvent = AnimationEventControllerEditorWindow.Instance.GetKeyframeEventInCurrentEventAnimation(index);
                                    if(keyframeEvent != null)
                                    {
                                        if (keyframeEvent.eventKeyframe != 0 && keyframeEvent.eventKeyframe != AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameCount() - 1)
                                        {
                                            selectedKeyframeEvent = keyframeEvent;
                                        }
                                        else
                                            selectedKeyframeEvent = null;
                                    }

                                    AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = index;
                                    isEventDragged = true;
                                    e.Use();
                                }
                            }  
                            else if (e.button == 1)
                            {
                                int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                                if(index != -1)
                                {
                                    AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = AnimationEventControllerEditorWindow.Instance.current.eventAnimation;
                                    for (int i = 0; i < advancedAnimationEvent.keyframeEvents.Count; i++)
                                    {
                                        if(advancedAnimationEvent.keyframeEvents[i].eventKeyframe == index)
                                        {
                                            KeyframeEventRemoveGenericMenu(e, AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex);
                                            return;
                                        }
                                    }

                                    KeyframeEventAddGenericMenu(e, AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex);
                                }
                            }
                        }
                    }
                    break;
                case EventType.MouseUp:
                    {
                        isDragged = false;
                        isEventDragged = false;
                        selectedKeyframeEvent = null;
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (isDragged && workTimelineView.IsInView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                            if (index != -1)
                            {
                                AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = index;

                                e.Use();
                            }
                        }
                        else if (isEventDragged && workTimelineView.IsInView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                            if (index != -1 && AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex != index)
                            {                       
                                if (index != 0 && index != AnimationEventControllerEditorWindow.Instance.GetCurrentEventAnimationClipFrameCount() - 1)
                                {
                                    AnimationEventController.UnityKeyframeEvent keyframeEvent = AnimationEventControllerEditorWindow.Instance.GetKeyframeEventInCurrentEventAnimation(index);
                                    if (selectedKeyframeEvent != null)
                                    {
                                        selectedKeyframeEvent = AnimationEventControllerEditorWindow.Instance.SwapKeyFrameEvent(index, selectedKeyframeEvent);
                                    }
                                }

                                AnimationEventControllerEditorWindow.Instance.current.timeline.frameIndex = index;

                                e.Use();
                            }
                        }
                    }
                    break;
            }
        }

        private void KeyframeEventAddGenericMenu(Event e, int index)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Add KeyframeEvent"), false, () => CreateKeyframeNode(e.mousePosition, index));

            genericMenu.ShowAsContext();

            e.Use();
            GUI.changed = true;
        }

        private void KeyframeEventRemoveGenericMenu(Event e, int index)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Remove KeyframeEvent"), false, () => DeleteKeyframeNode(e.mousePosition, index));

            genericMenu.ShowAsContext();

            e.Use();
            GUI.changed = true;
        }

        private void CreateKeyframeNode(Vector2 mousePosition, int index)
        {
            if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation == null)
                return;

            AnimationEventControllerEditorWindow.Instance.AddKeyframeEvent(AnimationEventControllerEditorWindow.Instance.current.eventAnimation, index);
        }

        private void DeleteKeyframeNode(Vector2 mousePosition, int index)
        {
            if (AnimationEventControllerEditorWindow.Instance.current.eventAnimation == null)
                return;

            AnimationEventControllerEditorWindow.Instance.RemoveKeyframeEvent(AnimationEventControllerEditorWindow.Instance.current.eventAnimation, index);
        }
    }
}