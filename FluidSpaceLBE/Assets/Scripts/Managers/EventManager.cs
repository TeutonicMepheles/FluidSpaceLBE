using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
    private Dictionary<string, object> eventDictionaryWithParams = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEvent(string eventName, UnityAction listener)
    {
        if (!eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent = new UnityEvent();
            eventDictionary.Add(eventName, thisEvent);
        }
        thisEvent.AddListener(listener);
    }

    public void RemoveEvent(string eventName, UnityAction listener)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public void AddEvent<T>(string eventName, UnityAction<T> listener)
    {
        if (!eventDictionaryWithParams.TryGetValue(eventName, out object thisEvent))
        {
            thisEvent = new UnityEvent<T>();
            eventDictionaryWithParams.Add(eventName, thisEvent);
        }
        ((UnityEvent<T>)thisEvent).AddListener(listener);
    }

    public void RemoveEvent<T>(string eventName, UnityAction<T> listener)
    {
        if (eventDictionaryWithParams.TryGetValue(eventName, out object thisEvent))
        {
            ((UnityEvent<T>)thisEvent).RemoveListener(listener);
        }
    }

    public void TriggerEvent<T>(string eventName, T param)
    {
        if (eventDictionaryWithParams.TryGetValue(eventName, out object thisEvent))
        {
            ((UnityEvent<T>)thisEvent).Invoke(param);
        }
    }
}
