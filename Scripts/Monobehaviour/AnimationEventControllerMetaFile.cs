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
            public DateTime creationTime;
            public DateTime latelyModifiedTime;

            public string guid;
            public string sceneName;
            public string objectName;
            public string path;

        }
        [SerializeField]
        private MetaData current;

#if UNITY_EDITOR

        public void Initialize(string path, string sceneName, string objectName)
        {
            current.creationTime = DateTime.Now;
            ModifyDate(current.creationTime);

            current.sceneName = sceneName;
            current.objectName = objectName;
            current.path = path;
            current.guid = AssetDatabase.AssetPathToGUID(current.path);
        }

        private void ModifyDate(DateTime time)
        {
            current.latelyModifiedTime = time;
        }

#endif
        //GUID는 파일로 존재해야 생긴다 
        //메타파일의 guid와 비교하여 틀리면 적용하기를 눌러야하고 아니면 오픈 에디터가 바로 뜬다
        //같은 오브젝트이름 가능성도 존재하고 
        //메타파일의 guid와 비교하여 틀리면 적용하기를 눌러야하고 아니면 오픈 에디터가 바로 뜬다
        //public bool Compare
    }
}