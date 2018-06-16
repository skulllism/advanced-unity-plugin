using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class CommandQueue : MonoBehaviour
    {
        public Command[] commands;

        private Command[] clones;

        private Queue<Command> queue = new Queue<Command>();

        private void Start()
        {
            clones = PrototypeScriptableObject.SetClones(gameObject, commands);
        }

        private Command GetCommand(string commandName)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].name == commandName)
                    return commands[i];
            }

            return null;
        }

        public void Push(string commandName)
        {
            queue.Enqueue(GetCommand(commandName));
        }

        public void Execute()
        {
            queue.Dequeue().Execute(gameObject);
        }

        public void Execute(string commandName)
        {
            queue.Clear();

            GetCommand(commandName).Execute(gameObject);
        }
    }
}
