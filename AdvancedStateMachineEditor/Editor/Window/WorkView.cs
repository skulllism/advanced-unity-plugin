using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class WorkView : ViewBase
    {
        public WorkView() : base("WorkView"){}

        private const float minZoomScale = 0.5f;
        private const float maxZoomScale = 1.5f;
        private float   zoomScale = 1.0f;
        private Vector2 zoomCoordsOrigin = Vector2.zero;
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
            ProcessContextMenu(e);
            originMousePos = e.mousePosition;

            EditorZoomArea.Begin(zoomScale, viewRect);
            {
                zoomMousePos = e.mousePosition;

                base.ProcessEvents(e);
                if(e.type == EventType.MouseDown || e.type == EventType.MouseUp || e.type == EventType.MouseDrag)
                    ProcessNodeEvents(e);

                switch (e.type)
                {
                    case EventType.MouseDown:
                        {
                            if (e.button == 0)
                            {
                                if (isConnectionStart)
                                    ClearConnectionSelection();
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
                        }
                        break;
                    case EventType.MouseUp:
                        {
                            isDragStart = false;
                            drag = Vector2.zero;
                            GUI.changed = true;
                        }
                        break;
                    case EventType.MouseDrag:
                        float lim = 0.1f;
                        if (isDragStart && e.button == 0 && e.alt)
                        {
                            Vector2 delta = Event.current.delta;
                            delta /= zoomScale;
                            zoomCoordsOrigin -= delta;

                            drag = delta;

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

                            Event.current.Use();
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

                if (AdvancedStateMachineEditorWindow.EditorNodes != null)
                {
                    for (int i = 0; i < AdvancedStateMachineEditorWindow.EditorNodes.Count; i++)
                    {
                        if(AdvancedStateMachineEditorWindow.EditorNodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.STATE)
                        {
                            AdvancedStateMachineEditorWindow.EditorNodes[i].DrawConnection(zoomCoordsOrigin);
                        }
                    }

                    for (int i = 0; i < AdvancedStateMachineEditorWindow.EditorNodes.Count  ; i++)
                    {    
                        AdvancedStateMachineEditorWindow.EditorNodes[i].Draw(zoomCoordsOrigin);

                        if (AdvancedStateMachineEditorWindow.EditorNodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
                        {
                            if(AdvancedStateMachineEditorWindow.EditorNodes[i].myData.transition.state != null && AdvancedStateMachineEditorWindow.EditorNodes[i].myData.transition.state.ID != string.Empty)
                            {
                                GUI.Box(new Rect(AdvancedStateMachineEditorWindow.EditorNodes[i].rect.x - zoomCoordsOrigin.x, AdvancedStateMachineEditorWindow.EditorNodes[i].rect.y + AdvancedStateMachineEditorWindow.EditorNodes[i].rect.height - zoomCoordsOrigin.y
                                             , AdvancedStateMachineEditorWindow.EditorNodes[i].rect.width, AdvancedStateMachineEditorWindow.EditorNodes[i].rect.height)
                                    , new GUIContent(AdvancedStateMachineEditorWindow.EditorNodes[i].myData.transition.state.ID));    
                            }
                        }
                    }
                }
            }
            EditorZoomArea.End();
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

            offset += (drag / 15.0f);
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

        private void ProcessNodeEvents(Event e)
        {
            GUI.Label(new Rect(0, 0, 300, 50), "viewRect" + viewRect.ToString());
            GUI.Label(new Rect(0, 50, 300, 50), "mousePos" + e.mousePosition.ToString());
            GUI.Label(new Rect(0, 100, 300, 50), "origin mousePos" + originMousePos.ToString());
            GUI.Label(new Rect(0, 150, 300, 50), "coord" + zoomCoordsOrigin.ToString());
            GUI.Label(new Rect(0, 200, 300, 50), new Rect(originMousePos.x - viewRect.x - zoomCoordsOrigin.x, originMousePos.y - viewRect.y - zoomCoordsOrigin.y, viewRect.width, viewRect.height).ToString());

            if(!viewRect.Contains(originMousePos))
                return;

            if (AdvancedStateMachineEditorWindow.EditorNodes != null)
            {
                for (int i = AdvancedStateMachineEditorWindow.EditorNodes.Count - 1; i >= 0; i--)
                {
                    if (AdvancedStateMachineEditorWindow.EditorNodes[i].ProcessEvents(zoomCoordsOrigin, e))
                    {
                        if(!isConnectionStart)
                        {
                             EditorNode<AdvancedStateMachineEditorWindow.NodeData> temp = AdvancedStateMachineEditorWindow.EditorNodes[i];
                            AdvancedStateMachineEditorWindow.EditorNodes.Remove(AdvancedStateMachineEditorWindow.EditorNodes[i]);

                            AdvancedStateMachineEditorWindow.EditorNodes.Add(temp);

                            AdvancedStateMachineEditorWindow.Instance.SelectNode(temp);
                        }
                        else
                        {
                            AdvancedStateMachineEditorWindow.EditorNodes[i].isSelected = false;
                            AdvancedStateMachineEditorWindow.Instance.selectNode.isSelected = true;

                            if (AdvancedStateMachineEditorWindow.EditorNodes[i].myData.type == AdvancedStateMachineEditorWindow.NodeType.TRANSITION)
                            {
                                AdvancedStateMachineEditorWindow.Instance.CreateConnection(AdvancedStateMachineEditorWindow.Instance.selectNode, AdvancedStateMachineEditorWindow.EditorNodes[i]);

                                ClearConnectionSelection();
                            }
                            else
                            {
                                ClearConnectionSelection();
                            }
                        }
                        break;
                    }
                }

                if(AdvancedStateMachineEditorWindow.Instance.selectNode != null)
                {
                    if (!AdvancedStateMachineEditorWindow.Instance.selectNode.isSelected)
                    {
                        AdvancedStateMachineEditorWindow.Instance.SelectNode(null);
                        //AdvancedStateMachineEditorWindow.Instance.selectNode = null;
                    }
                }
            }
        }

        private void ProcessContextMenu(Event e)
        {
            if (e.button == 1 && !e.alt)
            {
                if (!IsSelectedNode())
                {
                    GenericMenu genericMenu = new GenericMenu();
                    genericMenu.AddItem(new GUIContent("Create/State"), false, () => OnClickCreateState(e.mousePosition));
                    genericMenu.AddItem(new GUIContent("Create/Transition"), false, () => OnClickCreateTransition(e.mousePosition));
                    genericMenu.ShowAsContext();

                    e.Use();
                    GUI.changed = true;
                }
            }
        }

        private void OnClickCreateState(Vector2 mousePosition)
        {
            AdvancedStateMachineEditorWindow.Instance.CreateState(new Vector2(mousePosition.x - viewRect.x, mousePosition.y - viewRect.y));
        }

        private void OnClickCreateTransition(Vector2 mousePosition)
        {
            AdvancedStateMachineEditorWindow.Instance.CreateTransition(new Vector2(mousePosition.x - viewRect.x, mousePosition.y - viewRect.y));
        }

        private bool IsSelectedNode()
        {
            if (AdvancedStateMachineEditorWindow.EditorNodes != null)
            {
                for (int i = 0; i < AdvancedStateMachineEditorWindow.EditorNodes.Count; i++)
                {
                    if (AdvancedStateMachineEditorWindow.EditorNodes[i].isSelected)
                    {
                        return true;
                    }
                }
            }

            return false;
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