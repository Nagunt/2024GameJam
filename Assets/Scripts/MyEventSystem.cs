using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public enum EventType
    {
        None = 0,
        Arrive,
        StageStart,
        StageClear,
        NextStage,
        StageRestart,
        HPChanged,
        GameOver,
        PlatformDrop
    }

    public class MyEventSystem : MonoBehaviour
    {
        class EventDelegateClass
        {
            public Action callback;
        }

        class EventDelegateClass<T> : EventDelegateClass
        {
            public new Action<T> callback;
        }

        class EventDelegateClass<T, K> : EventDelegateClass
        {
            public new Action<T, K> callback;
        }

        public static MyEventSystem Instance { get; private set; } = null;

        Dictionary<EventType, EventDelegateClass> eventData_0;
        Dictionary<EventType, EventDelegateClass> eventData_1;
        Dictionary<EventType, EventDelegateClass> eventData_2;

        private void Awake()
        {
            Instance = this;
            eventData_0 = new Dictionary<EventType, EventDelegateClass>();
            eventData_1 = new Dictionary<EventType, EventDelegateClass>();
            eventData_2 = new Dictionary<EventType, EventDelegateClass>();
        }

        public void Register(EventType eventType, Action action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_0;
            if(dic.TryGetValue(eventType, out EventDelegateClass value)) {
                value.callback += action;
            }
            else {
                EventDelegateClass newEvent = new EventDelegateClass();
                newEvent.callback = action;
                dic.Add(eventType, newEvent);
            }
        }

        public void Register<T>(EventType eventType, Action<T> action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_1;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                (value as EventDelegateClass<T>).callback += action;
            }
            else {
                EventDelegateClass<T> newEvent = new EventDelegateClass<T>();
                newEvent.callback += action;
                dic.Add(eventType, newEvent);
            }
        }

        public void Register<T, K>(EventType eventType, Action<T, K> action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_2;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                (value as EventDelegateClass<T, K>).callback += action;
            }
            else {
                Debug.Log("Register Event At");
                EventDelegateClass<T, K> newEvent = new EventDelegateClass<T, K>();
                newEvent.callback += action;
                dic.Add(eventType, newEvent);
            }
        }

        public void UnRegister(EventType eventType, Action action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_0;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                value.callback -= action;
            }
        }

        public void UnRegister<T>(EventType eventType, Action<T> action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_1;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                (value as EventDelegateClass<T>).callback -= action;
            }
        }

        public void UnRegister<T, K>(EventType eventType, Action<T, K> action)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_2;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                (value as EventDelegateClass<T, K>).callback -= action;
            }
        }


        public void Call(EventType eventType)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_0;
            if(dic.TryGetValue(eventType, out EventDelegateClass value)) {
                value.callback?.Invoke();
            }
        }

        public void Call<T>(EventType eventType, T t)
        {
            Dictionary<EventType, EventDelegateClass> dic = eventData_1;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                (value as EventDelegateClass<T>).callback?.Invoke(t);
            }
        }

        public void Call<T, K>(EventType eventType, T t, K k)
        {
            Debug.Log("Call Event At");
            Dictionary<EventType, EventDelegateClass> dic = eventData_2;
            if (dic.TryGetValue(eventType, out EventDelegateClass value)) {
                Debug.Log("¡∏¿Á«‘");
                (value as EventDelegateClass<T, K>).callback?.Invoke(t, k);
            }
        }
    }
}