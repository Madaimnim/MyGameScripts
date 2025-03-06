using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, Delegate> eventDictionary = new Dictionary<Type, Delegate>();

    /// <summary> ��ť�ƥ� </summary>
    public static void Listen<T>(Action<T> listener) {
        if (!eventDictionary.ContainsKey(typeof(T)))
        {
            eventDictionary[typeof(T)] = null;
        }
        eventDictionary[typeof(T)] = (Action<T>)eventDictionary[typeof(T)] + listener;
    }

    /// <summary> �����ť�ƥ� </summary>
    public static void StopListen<T>(Action<T> listener) {
        if (eventDictionary.ContainsKey(typeof(T)))
        {
            eventDictionary[typeof(T)] = (Action<T>)eventDictionary[typeof(T)] - listener;
        }
    }

    /// <summary> Ĳ�o�]�o�G�^�ƥ�A�q���Ҧ���ť�� </summary>
    public static void Trigger<T>(T eventData) {
        if (eventDictionary.ContainsKey(typeof(T)) && eventDictionary[typeof(T)] != null)
        {
            ((Action<T>)eventDictionary[typeof(T)]).Invoke(eventData);
        }
    }
}
