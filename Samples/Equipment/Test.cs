using UnityEngine.Events;
using UnityEngine;
using AdvancedUnityPlugin;
using System;

public class Test : MonoBehaviour, GameEvent<Equipable>.Listener
{
    [Serializable]
    public class Event : UnityEvent<Equipable> { }

    public EquipmentEvent gameEvent;
    public Event response;

    public Equipment equipment;
    public EquipmentSlot slot;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Equipable[] args)
    {
        response.Invoke(args[0]);
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            equipment.Select("testType", slot);
            equipment.EquipByItemID("testID");
        }
    }
}
