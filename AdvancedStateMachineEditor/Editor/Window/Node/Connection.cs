using UnityEngine;
using UnityEditor;
using System;

namespace AdvancedUnityPlugin.Editor
{
    public class Connection
    {
        private Vector2 startPos;
        private Vector2 endNode;

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



            //Handles.color = Color.red;
           // Vector2 center = (startNode.rect.center +endPos.center) * 0.5f;

            //.Draw

            //float angle =  GetAngle(stateNode.rect.center, transitionNode.rect.center);
            //Debug.Log(angle);
            //Handles.DrawPolyLine(new Vector3[]
            //{
            //    center,
            //    new Vector2(center.x + 30.0f * Mathf.Cos(45 ) + 30.0f * Mathf.Cos(180 - angle) 
            //                , center.y + 30.0f * Mathf.Sin(45 )+ 30.0f * Mathf.Sin(180 - angle)),
            //    new Vector2(center.x + 30.0f * Mathf.Cos(45 ) + 30.0f * Mathf.Cos(180 - angle)
            //                , center.y + 30.0f * Mathf.Sin(45 )+ 30.0f * Mathf.Sin(180 - angle)),
            //    center
            //});
            //Handles.ArrowHandleCap(0, (stateNode.rect.center + transitionNode.rect.center) * 0.5f, Quaternion.identity, 3.0f,EventType.ContextClick);
            //Handles.DrawSolidRectangleWithOutline(new Rect((stateNode.rect.center.x + transitionNode.rect.center.x) * 0.5f, (stateNode.rect.center.y + transitionNode.rect.center.y) * 0.5f, 30, 30), Color.black, Color.cyan);
            //Handles.DrawLine((stateNode.rect.center + transitionNode.rect.center) * 0.5f, (stateNode.rect.center + transitionNode.rect.center) * Mathf.Sin(45.0f) * 0.5f);
            //Handles.DrawLine((stateNode.rect.center + transitionNode.rect.center) * 0.5f, (new Vector2(-stateNode.rect.center.x,stateNode.rect.center.y) + new Vector2(-transitionNode.rect.center.x,transitionNode.rect.center.y)) * Mathf.Sin(45.0f) * 0.5f);
            //Handles.DrawLine((stateNode.rect.center + transitionNode.rect.center) * 0.5f, -(stateNode.rect.center + transitionNode.rect.center) * Mathf.Cos(45.0f));
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