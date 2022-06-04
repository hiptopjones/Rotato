using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CodedBasicGameEventListener : IGameEventListener
{
    [SerializeField]
    private SimpleGameEvent gameEvent;

    [SerializeField]
    private Action eventCallback;

    public void OnEnable(Action eventCallback)
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

    public void OnEventRaised()
    {
        if (eventCallback != null)
        {
            eventCallback.Invoke();
        }
    }
}
