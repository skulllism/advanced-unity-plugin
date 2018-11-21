using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow  
    {
        private AdvancedStateMachine selected;
        public static AdvancedStateMachine Target
        {
            get
            {
                if (Instance == null)
                    return null;
                else
                {
                    if (Instance.selected)
                    {
                        Instance.InitializePropertyData();
                        return Instance.selected;
                    }
                    else
                        return null;
                }
            }
        }

        private AdvancedStateMachine.AdvancedTransition[] advancedTransitions;
        public static AdvancedStateMachine.AdvancedTransition[] Transitions
        {
            get
            {
                if (Instance == null)
                    return null;
                else
                {
                    Instance.InitailizeTransition();
                    
                    return Instance.advancedTransitions;
                }
            }
        }

        private void InitailizeTransition()
        {
            advancedTransitions = FindObjectsOfType<AdvancedStateMachine.AdvancedTransition>();
        }

        private List<EditorNode<NodeData>> editorNodes;
        public static List<EditorNode<NodeData>> EditorNodes
        {
            get
            {
                if (Instance == null)
                    return null;
                else
                {
                    if (Instance.editorNodes == null)
                        Instance.InitializeNode();

                    return Instance.editorNodes;
                }
            }
        }
    }   
}