using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<IGameEventListener> listeners =
        new List<IGameEventListener>();

    public void Raise()
    {
        for (int i = 0; i < listeners.Count; ++i)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(IGameEventListener eventListener)
    {
        listeners.Add(eventListener);
    }

    public void UnregisterListener(IGameEventListener eventListener)
    {
        listeners.Remove(eventListener);
    }
}

public interface IGameEventListener
{
    void OnEventRaised();
}
