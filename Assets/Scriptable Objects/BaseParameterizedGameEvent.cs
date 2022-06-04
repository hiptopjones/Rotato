using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parameterized GameEvent")]
public class BaseParameterizedGameEvent<T> : ScriptableObject
{
    private List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

    public void Raise(T item)
    {
        for (int i = 0; i < listeners.Count; ++i)
        {
            listeners[i].OnEventRaised(item);
        }
    }

    public void RegisterListener(IGameEventListener<T> eventListener)
    {
        listeners.Add(eventListener);
    }

    public void UnregisterListener(IGameEventListener<T> eventListener)
    {
        listeners.Remove(eventListener);
    }
}

public interface IGameEventListener<T>
{
    void OnEventRaised(T item);
}
