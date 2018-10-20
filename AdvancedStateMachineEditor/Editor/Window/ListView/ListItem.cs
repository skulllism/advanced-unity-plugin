using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin.Editor
{
    //나중에 다 바꿀꺼야 데이터도 노필
    public class ListItem<T>
    {
        public T data;

        public int index;
        public Rect rect;
        private Rect originRect;

        private bool isToggle;
        public bool isSelected;

       
        public ListItem(int index)
        {
            this.index = index;
        }

        public ListItem(T data, int index)
        {
            this.data = data;
            this.index = index;
        }

        public ListItem(T data, int index, Rect rect)
        {
            this.data = data;
            this.index = index;
            this.rect = rect;
            originRect = rect;
        }

        public void Initalize(T data, int index)
        {
            this.data = data;
            this.index = index;
        }

        public void SetData(T data)
        {
            this.data = data;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public void SetRect(Rect rect)
        {
            originRect = this.rect = rect;
        }

        public void Draw(string text)
        {
            if(!isSelected)
                GUI.Box(rect, new GUIContent(text));
            else
                GUI.Box(new Rect(rect.x, rect.y + draggedOffset, rect.width, rect.height), new GUIContent(text),(GUIStyle)"ControlHighlight");
            
        }

        private float draggedOffset;
        public bool ProcessEvents(Event e)
        {
            switch(e.type)
            {
                case EventType.MouseDown:
                    {
                        if(e.button == 0)
                        {
                            if(rect.Contains(e.mousePosition))
                            {
                                isToggle = isSelected;

                                e.Use();
                                return isSelected = true;
                            }
                            else
                            {
                                isSelected = false;
                                isToggle = !isSelected;
                            }
                        }
                    }
                    break;
                case EventType.MouseUp:
                    {
                        if (rect.Contains(e.mousePosition)&& (isToggle == isSelected))
                        {
                            isSelected = false;
                            e.Use();
                        }
                            
                        if (isToggle != isSelected)
                            isToggle = isSelected;
                        
                    }
                    break;
            }
            return false;
        }
    }
}