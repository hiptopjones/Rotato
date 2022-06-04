using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityParameterizedGameEventListener<T> : MonoBehaviour, IGameEventListener<T>
{
    [SerializeField]
    private BaseParameterizedGameEvent<T> gameEvent;

    [SerializeField]
    private UnityEvent<T> unityEvent;

    public void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    public void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    public void OnEventRaised(T item)
    {
        if (unityEvent != null)
        {
            unityEvent.Invoke(item);
        }
    }
}
