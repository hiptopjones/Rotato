using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityBasicGameEventListener : MonoBehaviour, IGameEventListener
{
    [SerializeField]
    private SimpleGameEvent gameEvent;

    [SerializeField]
    private UnityEvent unityEvent;

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

    public void OnEventRaised()
    {
        if (unityEvent != null)
        {
            unityEvent.Invoke();
        }
    }
}
