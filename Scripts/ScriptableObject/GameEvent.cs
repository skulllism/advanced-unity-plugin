using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public interface Listener
    {
        void OnEventRaised();
    }

    private List<Listener> listeners = new List<Listener>();

    public void Raise()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(Listener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(Listener listener)
    {
        listeners.Remove(listener);
    }
}
