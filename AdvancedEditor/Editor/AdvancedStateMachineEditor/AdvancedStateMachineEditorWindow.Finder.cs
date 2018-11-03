using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin.Editor
{
    public partial class AdvancedStateMachineEditorWindow 
    {
        public int FindStateIndexByNode(EditorNode<NodeData> node)
        {
            if (node == null)
                return -1;
            
            for (int i = 0; i < selected.advancedStates.Count; i++)
            {
                if (selected.advancedStates[i].ID == node.myData.state.ID)
                {
                    return i;
                }
            }

            return -1;
        }

        public int FindTransitionIndexByNode(EditorNode<NodeData> node)
        {
            if (node == null)
                return -1;
            
            for (int i = 0; i < selected.advancedTransitions.Count; i++)
            {
                if (selected.advancedTransitions[i].ID == node.myData.transition.ID)
                {
                    return i;
                }
            }
            return -1;
        }

        public AdvancedStateMachine.AdvancedTransition FindTransitionByNode(EditorNode<NodeData> node)
        {
            if (node == null)
                return null;
            
            for (int i = 0; i < selected.advancedTransitions.Count; i++)
            {
                if (selected.advancedTransitions[i].ID == node.myData.transition.ID)
                {
                    return selected.advancedTransitions[i];
                }
            }
            return null;
        }

        public EditorNode<NodeData> FindNodeByTransition(AdvancedStateMachine.AdvancedTransition transition)
        {
            if (transition == null)
                return null;

            for (int i = 0; i < editorNodes.Count; i++)
            {
                if (editorNodes[i].myData.type != NodeType.TRANSITION)
                    continue;

                if (editorNodes[i].myData.transition.ID == transition.ID)
                    return editorNodes[i];
            }

            return null;
        }

        public EditorNode<NodeData> FindNodeByState(AdvancedStateMachine.AdvancedState state)
        {
            if (state == null)
                return null;

            if (state.ID == null)
                return null;

            for (int i = 0; i < editorNodes.Count; i++)
            {
                if (editorNodes[i].myData.type != NodeType.STATE)
                    continue;

                if (editorNodes[i].myData.state.ID == state.ID)
                    return editorNodes[i];
            }

            return null;
        }

        public EditorNode<NodeData> FindNodeByStateID(string stateID)
        {
            if (stateID == string.Empty)
                return null;

            for (int i = 0; i < editorNodes.Count; i++)
            {
                if (editorNodes[i].myData.type != NodeType.STATE)
                    continue;

                if (editorNodes[i].myData.state.ID == stateID)
                    return editorNodes[i];
            }

            return null;
        }
    }
}


