﻿using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public class ViewBase
    {
        public string     viewTitle;
        public Rect       viewRect;
        protected GUISkin viewSkin;

        public ViewBase()
        {
            viewTitle = string.Empty;
            viewRect = new Rect();
            GetEditorSkin();
        }

        public ViewBase(string title)
        {
            viewTitle = title;
            viewRect = new Rect();
            GetEditorSkin();
        }

        public ViewBase(string title, Rect rect)
        {
            viewTitle = title;
            viewRect = rect;
            GetEditorSkin();
        }

        protected void GetEditorSkin()
        {
            if (EditorGUIUtility.isProSkin)
                viewSkin = EditorGUIUtility.Load("Assets/AdvancedUnityPlugin/AdvancedEditor/Editor/GUISkin/EditorDarkSkin.guiskin") as GUISkin;
            else
                viewSkin = EditorGUIUtility.Load("Assets/AdvancedUnityPlugin/AdvancedEditor/Editor/GUISkin/EditorLightSkin.guiskin") as GUISkin;
        }

        public virtual void UpdateView(Rect editorRect, Rect percentageRect)
        {
            viewRect = new Rect(editorRect.x      * percentageRect.x,
                                editorRect.y      * percentageRect.y,
                                editorRect.width  * percentageRect.width,
                                editorRect.height * percentageRect.height
                               );
        }

        public virtual void GUIView(Event e)
        {
            //if (viewSkin == null)
            //{
            //    GetEditorSkin();
            //    return;
            //}
        }

        public virtual void ProcessEvents(Event e) 
        {  
        }
    }
}