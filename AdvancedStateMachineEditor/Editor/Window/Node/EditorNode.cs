using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class EditorNode<T>
    {
        public T myData { private set; get; }

        public bool isEnable;
        public int index;
        public string title;
        public Rect rect;
        public List<EditorNode<T>> childNodes { private set; get; }
        public List<EditorNode<T>> parentNodes { private set; get; }

        public bool isSelected;
        private bool isDragged;
        private bool isToggle;

        private GUIStyle normalStyle;
        private GUIStyle selectStyle;
        private Action<EditorNode<T>> onGenericMenu;

        private List<Connection> connections;

        public EditorNode(T myData, int index, Rect rect, string title, GUIStyle normalStyle, GUIStyle selectStyle, Action<EditorNode<T>> onGenericMenu)
        {
            this.isEnable = true;
            this.index = index;
            this.myData = myData;
            this.rect = rect;

            this.title = title;
            this.normalStyle = normalStyle;
            this.selectStyle = selectStyle;

            this.onGenericMenu = onGenericMenu;
            isToggle = !isSelected;
        }

        public void Relese()
        {
            if(parentNodes != null)
            {
                for (int i = 0; i < parentNodes.Count; i++)
                    parentNodes[i].RemoveChildNode(this);
            }

            if(childNodes != null)
            {
                for (int i = 0; i < childNodes.Count; i++)
                    childNodes[i].RemoveParentNode(this);
            }

            ClearParents();
            ClearChilds();

            if(connections != null)
                connections.Clear();
        }

        public void SetData(T data)
        {
            this.myData = data;
        }

        public void AddParentNode(EditorNode<T> node)
        {
            if (node == null)
                return;

            if (parentNodes == null)
                parentNodes = new List<EditorNode<T>>();

            parentNodes.Add(node);
        }

        public void AddChildNode(EditorNode<T> node)
        {
            if (node == null)
                return;

            if (childNodes == null)
                childNodes = new List<EditorNode<T>>();

            childNodes.Add(node);

            CreateConnection(this, node);
        }

        public void RemoveParentNode(EditorNode<T> node)
        {
            if (node == null || parentNodes == null)
                return;

            parentNodes.Remove(node);
        }

        public void RemoveChildNode(EditorNode<T> node)
        {
            if (node == null || childNodes == null)
                return;

            childNodes.Remove(node);

            RemoveConnection();
        }

        public void ClearParents()
        {
            if (parentNodes != null)
                parentNodes.Clear();
        }

        public void ClearChilds()
        {
            if (childNodes != null)
                childNodes.Clear();
        }

        private void CreateConnection(EditorNode<T> startNode, EditorNode<T> endNode)
        {
            if (connections == null)
                connections = new List<Connection>();

            Connection connection = new Connection();
            connections.Add(connection);
        }

        private void RemoveConnection()
        {
            if (connections == null)
                return;

            if(connections.Count > 0)
                connections.RemoveAt(connections.Count - 1);
        }

        private void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(rect, "", isSelected ? selectStyle : normalStyle);

            GUI.Label(new Rect(rect.x + rect.width * 0.5f - (title.Length * 2.5f), rect.y + 15, 100, 20), title);
        }

        public void Draw(Vector2 zoomScale)
        {
            GUI.Box(new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height), "", isSelected ? selectStyle : normalStyle);

            GUI.Label(new Rect(rect.x - zoomScale.x + rect.width * 0.5f - (title.Length * 2.5f), rect.y - zoomScale.y + 15, 150, 20), title);

            //GUI.Label(new Rect(rect.x - zoomScale.x + rect.width * 0.5f - (title.Length * 2.5f), rect.y - zoomScale.y + 15, 150, 20), rect.ToString());
        }

        public void Draw(string text,Vector2 zoomScale)
        {
            GUI.Box(new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height), "", isSelected ? selectStyle : normalStyle);

            GUI.Label(new Rect(rect.x + rect.width * 0.5f - (text.Length * 3.5f), rect.y + 15, rect.width, 20), text);
        }

        public void DrawConnection(Vector2 zoomScale)
        {
            if (connections == null)
                return;

            if (childNodes != null)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    if (connections.Count <= i)
                        continue;

                    connections[i].Draw(zoomScale, rect.center, childNodes[i].rect.center);
                }
            }
        }

        public bool ProcessEventByEventType(Vector2 zoomScale, Event e, EventType eventType)
        {
            if (!isSelected && !(new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height).Contains(e.mousePosition)))
                return false;

            switch(eventType)
            {
                case EventType.MouseDown:
                    {
                        if (ProcessMouseLeftDown(zoomScale, e))
                            return true;

                        if (ProcessMouseRightDown(zoomScale, e))
                            return true;

                    }
                    break;
                case EventType.MouseUp:
                    {
                        if (ProcessMouseUp(zoomScale, e))
                            return true;
                    }
                    break;
                case EventType.MouseDrag:
                    {
                        if (e.button == 1)
                            return false;
                        
                        if (ProcessMouseDrag(zoomScale, e))
                            return true;
                    }
                    break;
            }

            return false;
        }

        private bool ProcessMouseLeftDown(Vector2 zoomScale, Event e)
        {
            if (e.button == 0)
            {
                if (new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height).Contains(e.mousePosition) && e.alt == false)
                {
                    isToggle = isSelected;
                    return isSelected = true;
                }
                else
                {
                    isSelected = false;
                    isToggle = !isSelected;
                }
            }

            return false;
        }

        private bool ProcessMouseRightDown(Vector2 zoomScale,Event e)
        {
            if (e.button == 1 && isSelected && new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height).Contains(e.mousePosition))
            {
                if (onGenericMenu != null)
                    onGenericMenu(this);
                
                return true;
            }
            return false;
        }

        private bool ProcessMouseUp(Vector2 zoomScale, Event e)
        {
            if (new Rect(rect.x - zoomScale.x, rect.y - zoomScale.y, rect.width, rect.height).Contains(e.mousePosition) && (isToggle == isSelected) && !isDragged)
            {
                isSelected = false;
                return true;
            }

            if (isToggle != isSelected)
                isToggle = isSelected;

            isDragged = false;

            return false;
        }

        private bool ProcessMouseDrag(Vector2 zoomScale, Event e)
        {
            if (isSelected)
            {
                Drag(e.delta);
                return isDragged = true;
            }

            return false;
        }
    }
}