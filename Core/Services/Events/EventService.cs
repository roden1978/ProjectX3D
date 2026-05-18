using System;
using System.Collections.Generic;

public static class EventService
{
    private static readonly Dictionary<Type, Action<GameEvent>> Events = [];

    private static readonly Dictionary<Delegate, Action<GameEvent>> EventLookups = [];

    public static void Subscribe<T>(Action<T> evt) where T : GameEvent
    {
        if (false == EventLookups.ContainsKey(evt))
        {
            void NewAction(GameEvent e) => evt((T) e);
                
            EventLookups[evt] = NewAction;

            if (Events.ContainsKey(typeof(T)))
                Events[typeof(T)] += NewAction;
            else
                Events[typeof(T)] = NewAction;
        }
    }

    public static void UnSubscribe<T>(Action<T> evt) where T : GameEvent
    {
        if (EventLookups.TryGetValue(evt, out Action<GameEvent> action))
        {
            if (Events.TryGetValue(typeof(T), out Action<GameEvent> tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    Events.Remove(typeof(T));
                else
                    Events[typeof(T)] = tempAction;
            }

            EventLookups.Remove(evt);
        }
    }

    public static void Broadcast(GameEvent evt)
    {
        if (Events.TryGetValue(evt.GetType(), out Action<GameEvent> action))
            action.Invoke(evt);
    }

    public static void Clear()
    {
        Events.Clear();
        EventLookups.Clear();
    }
}