using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow
    {
        //TODO : 에디터에서 오브젝트의 인스턴스 아이디를 기억하고 캐시데이터를 저장하고 있자, 인스턴스의 파일명이 바뀌면 삭제하고 새로생성하고 ㅇㅋ ?
        //TODO : 타겟의 이름이 바뀌면 캐시 삭제
        //ISSUE : 인스턴스 ID가 경로에 포함되기 때문에, 프리팹으로 이동시 문제가 될 수 있다. 하지만 인스턴스 id를 안쓰면 같은 이름을 가진 프리팹에 대해서 문제가 발생할 수 있다 ?

        private const string PATH = "Assets/Editor Defalult Resources/AdvancedStateMachineEditor/cache/";
        public void SaveData()
        {
            if (editorNodes == null || editorNodes.Count <= 0)
                return;


            DirectoryInfo info = new DirectoryInfo(PATH);
            if(!info.Exists)
            {
                info.Create();
            }

            NodeAsset asset = AssetDatabase.LoadAssetAtPath<NodeAsset>(PATH + Target.transform.root.name + Target.transform.root.GetInstanceID() + ".asset");
            if(asset == null)
            {
                asset = ScriptableObject.CreateInstance<NodeAsset>();
                AssetDatabase.CreateAsset(asset, PATH + Target.transform.root.name + Target.transform.root.GetInstanceID() + ".asset");
            }
             
            asset.Save(editorNodes);

            AssetDatabase.Refresh();
        }

        private void LoadData()
        {
            NodeAsset asset = AssetDatabase.LoadAssetAtPath<NodeAsset>(PATH + Target.transform.root.name + Target.transform.root.GetInstanceID() + ".asset");
            if (asset == null)
                return;

            asset.Load(editorNodes);
        }
    }
}