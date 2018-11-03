using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class ASMWorkView : ViewBase
    {
        public ASMWorkView() : base("WorkView"){}

        private const float minZoomScale = 0.4f;
        private const float maxZoomScale = 1.5f;
        public float   zoomScale = 1.0f;
        public Vector2 zoomCoordsOrigin = Vector2.zero;
        private Vector2 originMousePos = Vector2.zero;
        private Vector2 zoomMousePos = Vector2.zero;

        private bool isConnectionStart;
        private Connection tempConnect;
      

        public void Initialize()
        {
            DecendingOrder();
        }

        private void DecendingOrder()
        {
            if (AdvancedStateMachineEditorWindow.EditorNodes == null)
                return;
            
            int count = AdvancedStateMachineEditorWindow.EditorNodes.Count;
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - 1 - i; j++)
                {
                    if(AdvancedStateMachineEditorWindow.EditorNodes[j].rect.y > AdvancedStateMachineEditorWindow.EditorNodes[j + 1].rect.y)
                    {
                        EditorNode<AdvancedStateMachineEditorWindow.NodeData> temp = AdvancedStateMachineEditorWindow.EditorNodes[j];
                        AdvancedStateMachineEditorWindow.EditorNodes[j] = AdvancedStateMachineEditorWindow.EditorNodes[j + 1];
                        AdvancedStateMachineEditorWindow.EditorNodes[j + 1] = temp;
                    }
                }
            }
        }

        public override void ProcessEvents(Event e)
        {
            if (e.type == EventType.Layout)
                return;

            originMousePos = e.mousePosition;

            if (!viewRect.Contains(e.mousePosition))
                return;
            
            if (ProcessContextMenu(e))
                return;
            
            EditorZoomArea.Begin(zoomScale, viewRect);
            {
                zoomMousePos = e.mousePosition;

                if(e.type == EventType.MouseDown || e.type == EventType.MouseUp || e.type == EventType.MouseDrag)
                {
                    if (!e.alt && ProcessNodeEvents(e))
                    {
                        //## 최적화를 위해서 따로 뺴야한디
                        if (AdvancedStateMachineEditorWindow.Instance.selectNode != null)
                        {
                            if (!AdvancedStateMachineEditorWindow.Instance.selectNode.isSelected)
                            {
                                AdvancedStateMachineEditorWindow.Instance.SelectNode(null);
                            }
                        }
                        return;
                    }
                }

                switch (e.type)
                {
                    case EventType.MouseDown:
                        {
                            if (e.button == 0&& !e.alt)
                            {
                                if (isConnectionStart)
                                    ClearConnectionSelection();

                                //## 최적화를 위해서 따로 뺴야한디
                                if (AdvancedStateMachineEditorWindow.Instance.selectNode != null)
                                {
                                    if (!AdvancedStateMachineEditorWindow.Instance.selectNode.isSelected)
                                    {
                                        AdvancedStateMachineEditorWindow.Instance.SelectNode(null);
                                    }
                                }
                            }

                            if (e.button == 0 && e.alt)
                            {
                                isDragStart = true;

                                if (isConnectionStart)
                                    ClearConnectionSelection();
                            }
                            else if (e.button == 1 && !e.alt)
                            {
                                if (isConnectionStart)
                                    ClearConnectionSelection();
                            }


                            GUI.changed = true;
                            e.Use();
                        }
                        break;
                    case EventType.MouseUp:
                        {
                            isDragStart = false;
                            drag = Vector2.zero;

                            GUI.changed = true;
                            e.Use();
                        }
                        break;
                    case EventType.MouseDrag:
                        if (isDragStart && e.button == 0 && e.alt)
                        {
                            Vector2 delta = Event.current.delta;
                            delta /= 1.0f;
                            zoomCoordsOrigin -= delta;

                            drag = delta;

                            e.Use();
                            GUI.changed = true;
                        }
                        break;
                    case EventType.ScrollWheel:
                        {
                            Vector2 screenCoordsMousePos = Event.current.mousePosition;
                            Vector2 delta = Event.current.delta;
                            Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
                            float zoomDelta = -delta.y / 150.0f;
                            float oldZoom = zoomScale;
                            zoomScale += zoomDelta;
                            zoomScale = Mathf.Clamp(zoomScale, minZoomScale, maxZoomScale);
                            zoomCoordsOrigin += (zoomCoordsMousePos - zoomCoordsOrigin) - (oldZoom / zoomScale) * (zoomCoordsMousePos - zoomCoordsOrigin);

                            e.Use();
                            GUI.changed = true;
                        }
                        break;
                        //======================================================
                    //## TODO
                    //======================================================
                    case EventType.KeyDown:
                        {
                            if (e.control && e.keyCode == KeyCode.S)
                            {
                                AdvancedStateMachineEditorWindow.Instance.SaveData();
                                e.Use();
                            }
                            else if (e.control && e.keyCode == KeyCode.D)
                            {
                                //Copy
                                AdvancedStateMachineEditorWindow.Instance.CopyNode();
                            }
                            else if (e.keyCode == KeyCode.Delete)
                            {
                                if (AdvancedStateMachineEditorWindow.Instance.selectNode != null)
                                {
                                    AdvancedStateMachineEditorWindow.Instance.DeleteNode(AdvancedStateMachineEditorWindow.Instance.selectNode);
                                    e.Use();
                                }
                            }
                        }
                        break;
                }//end switch
            }
            EditorZoomArea.End();
        }

        public override void UpdateView(Rect editorRect, Rect percentageRect)
        {
            base.UpdateView(editorRect, percentageRect);
        }

        public override void GUIView(Event e)
        {
            base.GUIView(e);
                   
            EditorZoomArea.Begin(zoomScale, viewRect);
            {
                //DrawGrid(20.0f , 0.5f, Color.gray);
                //DrawGrid(100.0f , 0.2f, Color.black);

                if (tempConnect != null)
                {
                    tempConnect.DrawByMouse(zoomCoordsOrigin, AdvancedStateMachineEditorWindow.Instance.selectNode.rect.center, zoomMousePos);
                    GUI.changed = true;
                }

                if(!AdvancedStateMachineEditorWindow.Instance.propertiesView.isHightLight)
                {
                    DrawNormalNode();
                }
                else
                {
                    DrawHightlightNode();
                }

            }
            EditorZoomArea.End();

            //GUI.Label(new Rect(245, 0, 300, 50), "viewRect" + viewRect.ToString());
            //GUI.Label(new Rect(245, 50, 300, 50), "zoom mousePos" + zoomMousePos.ToString());
            //GUI.Label(new Rect(245, 100, 300, 50), "origin mousePos" + originMousePos.ToString());
            //GUI.Label(new Rect(245, 150, 300, 50), "coord" + zoomCoordsOrigin.ToString());
            //GUI.Label(new Rect(245, 200, 300, 50), new Rect(originMousePos.x - viewRect.x - zoomCoordsOrigin.x, originMousePos.y - viewRect.y - zoomCoordsOrigin.y, viewRect.width, viewRect.height).ToString());
            //GUI.Label(new Rect(245, 250, 300, 50), "zoomScale" + zoomScale.ToString());
        }

        private void DrawNormalNode()
        {
            for (int i = 0; i < AdvancedStateMachineEditorWindow.EditorNodes.Count; i++)
            {
                AdvancedStateMachineEditorWindow.EditorNodes[i].DrawConnection(zoomCoordsOrigin);
            }

            for (int i = 0; i < AdvancedStateMachineEditorWindow.EditorNodes.Count; i++)
            {
                AdvancedStateMachineEditorWindow.EditorNodes[i].Draw(zoomCoordsOrigin);

                //if (AdvancedStateMachineEditorWindow.EditorNodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
                //{
                //    if (AdvancedStateMachineEditorWindow.EditorNodes[i].myData.transition.stateID != string.Empty)
                //    {
                //        GUI.Box(new Rect(AdvancedStateMachineEditorWindow.EditorNodes[i].rect.x - zoomCoordsOrigin.x, AdvancedStateMachineEditorWindow.EditorNodes[i].rect.y + AdvancedStateMachineEditorWindow.EditorNodes[i].rect.height - zoomCoordsOrigin.y
                //                     , AdvancedStateMachineEditorWindow.EditorNodes[i].rect.width, AdvancedStateMachineEditorWindow.EditorNodes[i].rect.height)
                //            , new GUIContent(AdvancedStateMachineEditorWindow.EditorNodes[i].myData.transition.stateID));
                //    }
                //}
            }
        }

        private void DrawHightlightNode()
        {
            if (AdvancedStateMachineEditorWindow.Instance.selectNode == null || AdvancedStateMachineEditorWindow.Instance.selectNode.myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
            {
                DrawNormalNode();
            }
            else
            {
                EditorNode<AdvancedStateMachineEditorWindow.NodeData> node = AdvancedStateMachineEditorWindow.Instance.selectNode;

                //Connection
                node.DrawConnection(zoomCoordsOrigin);
                if (node.childNodes != null)
                {
                    for (int i = 0; i < node.childNodes.Count; i++)
                    {
                        node.childNodes[i].DrawConnection(zoomCoordsOrigin);
                    }
                }

                if(node.parentNodes != null)
                {
                    for (int i = 0; i < node.parentNodes.Count; i++)
                    {
                        node.parentNodes[i].DrawConnection(zoomCoordsOrigin);
                    }
                }


                //Node
                node.Draw(zoomCoordsOrigin);
                if (node.childNodes != null)
                {
                    for (int i = 0; i < node.childNodes.Count; i++)
                    {
                        node.childNodes[i].Draw(zoomCoordsOrigin);

                        if (node.childNodes[i].childNodes == null)
                            continue;

                        for (int j = 0; j < node.childNodes[i].childNodes.Count; j++)
                        {
                            node.childNodes[i].childNodes[j].Draw(zoomCoordsOrigin);
                        }
                    }
                }

                if (node.parentNodes != null)
                {
                    for (int i = 0; i < node.parentNodes.Count; i++)
                    {
                        node.parentNodes[i].Draw(zoomCoordsOrigin);
                    }
                }
            }
        }

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return (screenCoords - viewRect.TopLeft()) / zoomScale + zoomCoordsOrigin;
        }

        private bool isDragStart;
        private Vector2 offset = Vector2.zero;
        private Vector2 drag = Vector2.zero;
        private float gridScale;
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            float widthDivs  = viewRect.width / gridSpacing;//Mathf.CeilToInt();
            float heightDivs = viewRect.height / gridSpacing;//Mathf.CeilToInt(viewRect.height / gridSpacing);

            widthDivs = 100;
            heightDivs = 100;

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += (drag / 10.0f);
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);    

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, viewRect.height + 850.0f, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(viewRect.width + 850.0f, gridSpacing * j, 0f) + newOffset);
            }

           // Handles.color = Color.white;
            Handles.EndGUI();
        }

        private bool ProcessNodeEvents(Event e)
        {
            if (AdvancedStateMachineEditorWindow.EditorNodes != null)
            {
                for (int i = AdvancedStateMachineEditorWindow.EditorNodes.Count - 1; i >= 0; i--)
                {
                    switch(e.type)
                    {
                        case EventType.MouseDown:
                            {
                                if (ProcessMouseDownEventByNode(AdvancedStateMachineEditorWindow.EditorNodes[i], e))
                                {
                                    e.Use();
                                    return GUI.changed = true;
                                }
                            }
                            break;
                        case EventType.MouseUp:
                            {
                                if (AdvancedStateMachineEditorWindow.EditorNodes[i].ProcessEventByEventType(zoomCoordsOrigin, e, EventType.MouseUp))
                                {
                                    AdvancedStateMachineEditorWindow.Instance.SelectNode(null);
                                    e.Use();
                                    return GUI.changed = true;
                                }
                            }
                            break;
                        case EventType.MouseDrag:
                            {
                                if (AdvancedStateMachineEditorWindow.EditorNodes[i].ProcessEventByEventType(zoomCoordsOrigin, e, EventType.MouseDrag))
                                {
                                    e.Use();
                                    return GUI.changed = true;
                                }
                            }
                            break;
                    }
                }
            }
            return false;
        }

        private bool ProcessMouseDownEventByNode(EditorNode<AdvancedStateMachineEditorWindow.NodeData> node, Event e)
        {
            if (node.ProcessEventByEventType(zoomCoordsOrigin, e, EventType.MouseDown))
            {
                if (!isConnectionStart)
                {
                    EditorNode<AdvancedStateMachineEditorWindow.NodeData> temp = node;
                    AdvancedStateMachineEditorWindow.EditorNodes.Remove(node);

                    AdvancedStateMachineEditorWindow.EditorNodes.Add(temp);

                    AdvancedStateMachineEditorWindow.Instance.SelectNode(temp);
                }
                else
                {
                    node.isSelected = false;
                    AdvancedStateMachineEditorWindow.Instance.selectNode.isSelected = true;

                    AdvancedStateMachineEditorWindow.Instance.CreateConnection(AdvancedStateMachineEditorWindow.Instance.selectNode, node);

                    ClearConnectionSelection();
                }
                return true;
            }

            return false;
        }

        private bool ProcessContextMenu(Event e)
        {
            if (e.button == 1 && !e.alt)
            {
                if (AdvancedStateMachineEditorWindow.Instance.selectNode == null)
                {
                    GenericMenu genericMenu = new GenericMenu();
                    genericMenu.AddItem(new GUIContent("Create/State")     , false, () => OnClickCreateState());
                    genericMenu.AddItem(new GUIContent("Create/Transition"), false, () => OnClickCreateTransition());
                    genericMenu.ShowAsContext();

                    e.Use();
                    GUI.changed = true;

                    return true;
                }
            }

            return false;
        }

        private void OnClickCreateState()
        {
            AdvancedStateMachineEditorWindow.Instance.CreateState(new Vector2(zoomMousePos.x + zoomCoordsOrigin.x, zoomMousePos.y + zoomCoordsOrigin.y));
        }

        private void OnClickCreateTransition()
        {
            AdvancedStateMachineEditorWindow.Instance.CreateTransition(new Vector2(zoomMousePos.x + zoomCoordsOrigin.x, zoomMousePos.y + zoomCoordsOrigin.y));
        }

        public void MakeTransition(EditorNode<AdvancedStateMachineEditorWindow.NodeData> node)
        {
            if(isConnectionStart)
            {
                ClearConnectionSelection();
                return;
            }

            isConnectionStart = true;
            tempConnect = new Connection();
        }

        private void ClearConnectionSelection()
        {
            isConnectionStart = false;
            tempConnect = null;

            GUI.changed = true;
        }
    }
}