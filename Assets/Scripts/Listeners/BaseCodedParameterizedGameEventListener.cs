using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCodedParameterizedGameEventListener<T> : IGameEventListener<T>
{
    [SerializeField]
    private BaseParameterizedGameEvent<T> gameEvent;

    [SerializeField]
    private Action<T> eventCallback;

    public void OnEnable(Action<T> eventCallback)
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
            this.eventCallback = eventCallback;
        }
    }

    public void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
            eventCallback = null;
        }
    }

    public void OnEventRaised(T item)
    {
        if (eventCallback != null)
        {
            eventCallback.Invoke(item);
        }
    }
}
