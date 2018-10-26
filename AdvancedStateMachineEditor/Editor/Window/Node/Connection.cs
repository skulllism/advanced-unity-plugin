using UnityEngine;
using UnityEditor;
using System;

namespace AdvancedUnityPlugin.Editor
{
    public class Connection
    {
        public void Draw(Vector2 startPos, Vector2 endPos)
        {
            Handles.DrawBezier(startPos, endPos,
                               startPos, endPos,
                               Color.white,
                               null,
                               4.0f);
        }

        public void Draw(Vector2 zoomScale, Vector2 startPos, Vector2 endPos)
        {
            Handles.DrawBezier(  new Vector3(startPos.x - zoomScale.x, startPos.y - zoomScale.y, 0.0f)
                               , new Vector3(endPos.x - zoomScale.x, endPos.y - zoomScale.y , 0.0f)
                               , new Vector3(startPos.x - zoomScale.x, startPos.y - zoomScale.y, 0.0f)
                               , new Vector3(endPos.x - zoomScale.x, endPos.y - zoomScale.y, 0.0f)
                               , Color.white
                               , null
                               , 4.0f);



           // Handles.color = Color.red;
          //  Vector2 center = (new Vector2(startPos.x - zoomScale.x, startPos.y - zoomScale.y) + new Vector2(endPos.x - zoomScale.x ,endPos.y - zoomScale.y)) * 0.5f;

          //  float angle =  GetAngle(endPos, startPos);
           // Debug.Log(angle);
           // Handles.DrawLine(center, new Vector2(center.x, center.y + 50.0f * Mathf.Sin(angle)));
            //Handles.DrawPolyLine(new Vector3[]
            //{
            //    center,
            //    new Vector2( center.x  + 30.0f * Mathf.Cos(45 )  + 30.0f 
            //                ,center.y  + 30.0f * Mathf.Sin(45 ) + 30.0f) ,
            //    new Vector2( center.x  + 75.0f * Mathf.Cos(180 - 45 ) + 30.0f 
            //                ,center.y  + 30.0f * Mathf.Sin(45 ) + 30.0f) ,
            //    center
            //});
        }

        public void DrawByMouse(Vector2 zoomScale, Vector2 startPos, Vector2 endPos)
        {
            Handles.DrawBezier(new Vector3(startPos.x - zoomScale.x, startPos.y - zoomScale.y, 0.0f)
                      , new Vector3(endPos.x , endPos.y, 0.0f)
                      , new Vector3(startPos.x - zoomScale.x, startPos.y - zoomScale.y, 0.0f)
                      , new Vector3(endPos.x , endPos.y, 0.0f)
                      , Color.white
                      , null
                      , 4.0f);
        }

        public void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    {

                    }
                    break;
            }
        }

        private float GetAngle(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            float radian = Mathf.Atan2(dx, dy);

            float degree = (radian * 180.0f) / Mathf.PI;

            return degree;
        }
    }
}