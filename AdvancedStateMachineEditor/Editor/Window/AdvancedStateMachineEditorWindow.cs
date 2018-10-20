using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow : EditorWindow
    {
        public static AdvancedStateMachineEditorWindow Instance;

        public enum NodeType { STATE, TRANSITION }
        public class NodeData
        {
            public NodeType type;
            public AdvancedStateMachine.AdvancedState state;
            public AdvancedStateMachine.AdvancedTransition transition;

            public NodeData(NodeType type, AdvancedStateMachine.AdvancedState state, AdvancedStateMachine.AdvancedTransition transition)
            {
                this.type = type;
                this.state = state;
                this.transition = transition;
            }
        }

        //======================
        //## Editor View
        //======================
        public PropertiesView propertiesView;
        public WorkView workView;
        public StateModifyView stateModifyView;
        public TransitionModifyView transitionModifyView;
        //======================
        //## Property Data
        //======================
        private SerializedObject   serializedObject;
        private SerializedProperty propertyStates;
        private SerializedProperty propertyTransitions;

        public const float NODE_WIDTH = 180.0f;
        public const float NODE_HEIGHT = 40.0f;

        public EditorNode<NodeData> selectNode;

        public static void OpenWindow(AdvancedStateMachine data)
        {
            Instance = GetWindow<AdvancedStateMachineEditorWindow>();
            Instance.titleContent = new GUIContent("AdvancedStateMachine");

            if (data != null)
            {
                Instance.InitializeView();

                Instance.selected = data;
                Instance.InitializePropertyData();
                Instance.InitializeNode();
                Instance.InitializeConnection();
            }
            else
                Debug.LogError("[Editor]]Not Found Advanced State Machine");

            Instance.Show();
        }

  

        private void InitializeView()
        {
            if (Instance != null)
            {
                workView = new WorkView();
                propertiesView = new PropertiesView();
                stateModifyView = new StateModifyView();
                transitionModifyView = new TransitionModifyView();
            }
            else
            {
                workView = new WorkView();
                propertiesView = new PropertiesView();
                stateModifyView = new StateModifyView();
                transitionModifyView = new TransitionModifyView();
            }

            workView.Initialize();
        }

        public void InitializePropertyData()
        {
            serializedObject = new SerializedObject(selected);
            propertyStates = serializedObject.FindProperty("advancedStates");
            propertyTransitions = serializedObject.FindProperty("advancedTransitions");
        }

        private void InitializeNode()
        {
            if (editorNodes == null)
                editorNodes = new List<EditorNode<NodeData>>();

            //================================================
            editorNodes.Clear(); // 노드 데이터 로드해서 사용 할 예정 
            //================================================
            if (selected == null)
                return;

            for (int i = 0; i < selected.advancedStates.Count; i++)
            {
                AdvancedStateMachine.AdvancedState state = selected.advancedStates[i];

                CreateStateNode(state, new Vector2(30.0f + (i * 10.0f), 30.0f + (i * 10.0f)), state.ID);
            }

            for (int i = 0; i < selected.advancedTransitions.Count; i++)
            {
                AdvancedStateMachine.AdvancedTransition transition = selected.advancedTransitions[i];

                CreateTransitionNode(transition, new Vector2(60.0f + (i * 10.0f), 30.0f + (i * 10.0f)), transition.ID);
            }
        }

        private void InitializeConnection()
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if (editorNodes[i].myData.type == NodeType.STATE)
                {
                    for (int j = 0; j < editorNodes[i].myData.state.transitions.Count; j++)
                    {
                        AttachChildNodeInParentNode(FindNodeByTransition(editorNodes[i].myData.state.transitions[j]), editorNodes[i]);
                    }
                }
                else if (editorNodes[i].myData.type == NodeType.TRANSITION)
                {
                    AttachChildNodeInParentNode(FindNodeByState(editorNodes[i].myData.transition.state), editorNodes[i]);
                }
            }
        }

        private void Update()
        {
            if (selected == null || workView == null)
                return;

            if(selectNode != null)
            {
                switch(selectNode.myData.type)
                {
                    case NodeType.STATE :   
                        {
                            stateModifyView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                                       , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
                        }
                        break;
                    case NodeType.TRANSITION:
                        {
                            transitionModifyView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                                       , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
                        }
                        break;
                }//end switch
            }
            else
            {
                propertiesView.UpdateView(new Rect(position.width, position.height, position.width, position.height)
                                        , new Rect(0.0f, 0.0f, 0.3f, 1.0f));
            }
            
            workView.UpdateView(  new Rect(position.width, position.height, position.width, position.height)
                                , new Rect(0.3f, 0.0f, 0.7f, 1.0f));
        }

        private void OnGUI()
        {
            if(selected == null || workView == null)
            {
                GUILayout.Label("No AdvancedStateMachine Selected");
                return;
            }

            GUILayout.BeginHorizontal();
            {
                if (selectNode != null)
                {
                    switch (selectNode.myData.type)
                    {
                        case NodeType.STATE:
                            {
                                stateModifyView.GUIView(Event.current);
                            }
                            break;
                        case NodeType.TRANSITION:
                            {
                                transitionModifyView.GUIView(Event.current);
                            }
                            break;
                    }//end switch
                }
                else
                {
                    propertiesView.GUIView(Event.current);    
                }

                workView.GUIView(Event.current);
            }
            GUILayout.EndHorizontal();


            if (GUI.changed)
                Repaint();
        }

        //TODO : 동일한 ID 입력에 대한 예외처리 해주
        public void CreateState(Vector2 position)
        {
            AdvancedStateMachine.AdvancedState state = new AdvancedStateMachine.AdvancedState();
            state.ID = "New State" + Target.advancedStates.Count;
            Target.advancedStates.Add(state);

            InitializePropertyData();

            EditorNode<NodeData> node = CreateStateNode(state, position, state.ID);
            SelectNode(node);
        }

        //TODO : 동일한 ID 입력에 대한 예외처리 해주
        public void CreateTransition(Vector2 position)
        {
            AdvancedStateMachine.AdvancedTransition transition = new AdvancedStateMachine.AdvancedTransition();
            transition.ID = "New Transition" + Target.advancedTransitions.Count;
            transition.state = null;
            Target.advancedTransitions.Add(transition);

            InitializePropertyData();

            EditorNode<NodeData> node = CreateTransitionNode(transition, position, transition.ID);
            SelectNode(node);
        }

        private void DeleteNode(EditorNode<AdvancedStateMachineEditorWindow.NodeData> node)
        {
            if (node == null)
                return;

            if(node.myData.type == NodeType.STATE)
            {
                if(node.parentNodes != null)
                {
                    for (int i = 0; i < node.parentNodes.Count; i++)
                    {
                        node.parentNodes[i].myData.transition.state = null;
                    }    
                }

                selected.advancedStates.Remove(node.myData.state);
            }
            else
            {
                if(node.parentNodes != null)
                {
                    for (int i = 0; i < node.parentNodes.Count; i++)
                    {
                        for (int j = 0; j < node.parentNodes[i].myData.state.transitions.Count; j++)
                        {
                            if (node.parentNodes[i].myData.state.transitions[j].ID == node.myData.transition.ID)
                            {
                                node.parentNodes[i].myData.state.transitions.Remove(node.myData.transition);
                                break;
                            }
                        }
                    }
                }

                selected.advancedTransitions.Remove(node.myData.transition);
            }
                
            node.Relese();

            editorNodes.Remove(node);

            SelectNode(null);

            InitializePropertyData();
        }

        private EditorNode<NodeData> CreateStateNode(AdvancedStateMachine.AdvancedState state, Vector2 position, string title)
        {
            EditorNode<NodeData> node = new EditorNode<NodeData>(new NodeData(NodeType.STATE, state, null), new Rect(position.x, position.y, NODE_WIDTH, NODE_HEIGHT), title ,(GUIStyle)"flow node hex 4", (GUIStyle)"flow node hex 4 on", OnStateNodeGenericMenu);          
            editorNodes.Add(node);

            return node;
        }

        private EditorNode<NodeData> CreateTransitionNode(AdvancedStateMachine.AdvancedTransition transition, Vector2 position, string title)
        {
            EditorNode<NodeData> node = new EditorNode<NodeData>(new NodeData(NodeType.TRANSITION, null, transition), new Rect(position.x, position.y, NODE_WIDTH, NODE_HEIGHT), title, (GUIStyle)"flow node 2", (GUIStyle)"flow node 2 on", OnTransitionNodeGenericMenu);
            editorNodes.Add(node);

            return node;
        }

        private void OnStateNodeGenericMenu(EditorNode<NodeData> node)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Make Transition"), false, () => MakeTransition(node));
            genericMenu.AddItem(new GUIContent("Delete"), false, () => DeleteNode(node));

            genericMenu.ShowAsContext();

            Event.current.Use();
        }

        private void OnTransitionNodeGenericMenu(EditorNode<NodeData> node)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Connect State"), false, null);
            genericMenu.AddItem(new GUIContent("Delete"), false, () => DeleteNode(node));

            genericMenu.ShowAsContext();

            Event.current.Use();
        }

        private void MakeTransition(EditorNode<NodeData> node)
        {
            if(workView != null)
                workView.MakeTransition(node);
        }

        public void CreateConnection(EditorNode<NodeData> startNode, EditorNode<NodeData> endNode)
        {
            if(AttachTransitionInState(startNode, endNode))
            {
                AttachChildNodeInParentNode(endNode, startNode);
            }
        }

        public void AttachChildNodeInParentNode(EditorNode<NodeData> childNode, EditorNode<NodeData> parentNode)
        {
            if(childNode != null && parentNode != null)
                childNode.AddParentNode(parentNode);

            if(parentNode != null && childNode != null)
                parentNode.AddChildNode(childNode);
        }

        private bool AttachTransitionInState(EditorNode<NodeData> stateNode, EditorNode<NodeData> transitionNode)
        {
            for (int i = 0; i < stateNode.myData.state.transitions.Count; i++)
            {
                if (stateNode.myData.state.transitions[i] == transitionNode.myData.transition)
                    return false;
            }

            stateNode.myData.state.transitions.Add(transitionNode.myData.transition);

            return true;
        }

        public void SelectNodeByState(AdvancedStateMachine.AdvancedState state)
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if(editorNodes[i].myData.type == NodeType.STATE)
                {
                    if(editorNodes[i].myData.state.ID == state.ID)
                    {
                        SelectNode(editorNodes[i]);
                        return;
                    }
                }
            }
        }

        public void SelectNodeByTransition(AdvancedStateMachine.AdvancedTransition transition)
        {
            for (int i = 0; i < editorNodes.Count; i++)
            {
                if(editorNodes[i].myData.type == NodeType.TRANSITION)
                {
                    if(editorNodes[i].myData.transition.ID == transition.ID)
                    {
                        SelectNode(editorNodes[i]);
                        return;
                    }
                }
            }
        }

        public void SelectNode(EditorNode<NodeData> node)
        {
            if (selectNode != null)
            {
                selectNode.isSelected = false;
            }

            selectNode = node;
            if(selectNode != null)
            {
                selectNode.isSelected = true;    

                switch (selectNode.myData.type)
                {
                    case NodeType.STATE: SetDataInStateModifyView(selectNode); break;
                    case NodeType.TRANSITION: SetDataInTransitionModifyView(selectNode); break;
                }
            }
        }

        private void SetDataInStateModifyView(EditorNode<NodeData> node)
        {
            if (node == null)
                return;
            
            stateModifyView.Initialize(propertyStates.GetArrayElementAtIndex(FindStateIndexByNode(node))
                                        , node);
        }

        private void SetDataInTransitionModifyView(EditorNode<NodeData> node)
        {
            if (node == null)
                return;
            
            transitionModifyView.Initialize(propertyTransitions.GetArrayElementAtIndex(FindTransitionIndexByNode(node))
                                             , node);
        }
    }
}