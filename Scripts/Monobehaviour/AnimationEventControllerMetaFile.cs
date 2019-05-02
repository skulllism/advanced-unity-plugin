using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AdvancedUnityPlugin
{
    public class AnimationEventControllerMetaFile : ScriptableObject
    {
        [Serializable]
        public struct MetaData
        {
            public string creationTime;
            public string latelyModifiedTime;

            public string sceneName;
            public string objectName;
            public string path;
            public int instanceID;

        }
        [SerializeField]
        private MetaData current;

#if UNITY_EDITOR

        public void Initialize(string path, string sceneName, string objectName, int instanceID)
        {
            current.creationTime = DateTime.Now.ToString();
            ModifyDate(current.creationTime);

            current.sceneName = sceneName;
            current.objectName = objectName;
            current.path = path;
            current.instanceID = instanceID;
        }

        private void ModifyDate(string time)
        {
            current.latelyModifiedTime = time;
        }

        public string GetCreationTime()
        {
            return current.creationTime;
        }

        public string GetLatelyModifiedTime()
        {
            return current.latelyModifiedTime;
        }

#endif
        //GUID는 파일로 존재해야 생긴다 
        //메타파일의 guid와 비교하여 틀리면 적용하기를 눌러야하고 아니면 오픈 에디터가 바로 뜬다
        //같은 오브젝트이름 가능성도 존재하고 
        //메타파일의 guid와 비교하여 틀리면 적용하기를 눌러야하고 아니면 오픈 에디터가 바로 뜬다
        //public bool Compare
    }
}