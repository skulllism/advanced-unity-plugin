using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class AECWorkView : ViewBase
    {
        private AECWorkTimelineView workTimelineView;
        private AECWorkBaseEventModifyView baseEventModifyView;
        private AECWorkKeyFrameEventModifyView keyFrameEventModifyView;

        public void Initialize()
        {
            InitializeSubView();
        }

        private void InitializeSubView()
        {
            if (workTimelineView == null)
            {
                workTimelineView = new AECWorkTimelineView();
                baseEventModifyView = new AECWorkBaseEventModifyView();
                keyFrameEventModifyView = new AECWorkKeyFrameEventModifyView();
            }

            workTimelineView.Initialize();
            baseEventModifyView.Initialize();
            keyFrameEventModifyView.Initialize();
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);

            workTimelineView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                            , new Rect(0.0f, 0.0f, 1.0f, 0.2f));
            

            baseEventModifyView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                            , new Rect(0.0f, 0.2f, 0.5f, 0.9f));

            keyFrameEventModifyView.UpdateView(new Rect(viewRect.width, viewRect.height, viewRect.width, viewRect.height)
                            , new Rect(0.5f, 0.2f, 0.5f, 0.9f));
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);

            ProcessEvents(e);

            GUILayout.BeginArea(viewRect, "", "box");
            {
                workTimelineView.SetCurrentFrameIdex(AnimationEventControllerEditorWindow.Instance.currentFrameIndex);
                workTimelineView.GUIView(e);
                baseEventModifyView.GUIView(e);

                keyFrameEventModifyView.SetCurrentFrameIndex(AnimationEventControllerEditorWindow.Instance.currentFrameIndex);
                keyFrameEventModifyView.GUIView(e);
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
                                    AnimationEventControllerEditorWindow.Instance.currentFrameIndex = index;

                                    e.Use();
                                }
                            }           
                        }
                        else if(workTimelineView.IsInKeyframeView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            if(e.button == 0)
                            {
                                int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                                if (index != -1)
                                {
                                    AnimationEventControllerEditorWindow.Instance.currentFrameIndex = index;

                                    e.Use();
                                }
                            }  
                            else if (e.button == 1)
                            {
                                int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                                if(index != -1)
                                {
                                    AnimationEventController.AdvancedAnimationEvent advancedAnimationEvent = AnimationEventControllerEditorWindow.Instance.selected;
                                    for (int i = 0; i < advancedAnimationEvent.keyframeEvents.Count; i++)
                                    {
                                        if(advancedAnimationEvent.keyframeEvents[i].eventKeyframe == index)
                                        {
                                            KeyframeEventRemoveGenericMenu(e, AnimationEventControllerEditorWindow.Instance.currentFrameIndex);
                                            return;
                                        }
                                    }

                                    KeyframeEventAddGenericMenu(e, AnimationEventControllerEditorWindow.Instance.currentFrameIndex);
                                }
                            }
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (workTimelineView.IsInView(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y)))
                        {
                            int index = workTimelineView.GetFrameIndexOfRange(new Vector2(e.mousePosition.x - viewRect.x, e.mousePosition.y - viewRect.y));
                            if (index != -1)
                            {
                                AnimationEventControllerEditorWindow.Instance.currentFrameIndex = index;

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
            AnimationEventControllerEditorWindow.Instance.AddKeyframeEvent(index);
        }

        private void DeleteKeyframeNode(Vector2 mousePosition, int index)
        {
            AnimationEventControllerEditorWindow.Instance.RemoveKeyframeEvent(index);
        }
    }
}