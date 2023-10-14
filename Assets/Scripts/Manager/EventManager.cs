
//提示：过度使用装箱和拆箱会导致性能消耗过大的问题，因此使用object为变量的事件总线系统需慎用


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

enum EventType   //如果需要添加什么事件 可以直接在这里添加枚举
{
    BossFindTarget,
}

public class SingleValueUnityEvent : UnityEvent<float>{}
public class DoubleValueUnityEvent : UnityEvent<float, float>{}
public class ObjectValueEvent : UnityEvent<object> { }

public class EventManager : Singleton<EventManager>
{
    //public Dictionary<EventType, UnityEvent> dictionary = new Dictionary<EventType, UnityEvent>();
    public Dictionary<string, UnityEvent> strDictionary = new Dictionary<string, UnityEvent>();
    public Dictionary<string, UnityEvent<float>> singleValueDic = new Dictionary<string, UnityEvent<float>>();
    public Dictionary<string, UnityEvent<float, float>> doubleValueDic = new Dictionary<string, UnityEvent<float, float>>();

    //使用对象进行信息传递 Use object to Send message.
    public Dictionary<string, UnityEvent<object>> objectValueDic = new Dictionary<string, UnityEvent<object>>();

    protected override void Awake() //让scene切换的时候不要将该物体删除
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    #region 使用内部Enum进行分类的事件系统
/*    public void AddListener(EventType eventType, UnityAction unityAction)
    {
        if(dictionary.ContainsKey(eventType))
        {
            dictionary[eventType].AddListener(unityAction);
        }
        else
        {
            UnityEvent currentEvent = new UnityEvent();
            currentEvent.AddListener(unityAction);
            dictionary.Add(eventType, currentEvent);
        }
    }

    public void RemoveListener(EventType eventType, UnityAction unityAction)
    {
        if(dictionary.ContainsKey(eventType))
        {
            dictionary[eventType].RemoveListener(unityAction);
        }
        else
        {
            throw new System.Exception("删除事件出现错误");
        }
    }

    public void InvokeEvent(EventType eventType)   //执行所有事件
    {
        if(dictionary.ContainsKey(eventType))
        {
            dictionary[eventType].Invoke();
        }
        else
        {
            throw new System.Exception("调用事件出现错误");
        }
    }*/
    #endregion //使用内部Enum进行分类的事件系统

    #region 不使用参数的事件系统
    public void AddListener<T>(T eventType, UnityAction unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        Debug.Log(eventName);
        if (strDictionary.ContainsKey(eventName))
        {
            strDictionary[eventName].AddListener(unityAction);
        }
        else
        {
            UnityEvent currentEvent = new UnityEvent();
            currentEvent.AddListener(unityAction);
            strDictionary.Add(eventName, currentEvent);
        }
    }

    public void RemoveListener<T>(T eventType, UnityAction unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if (strDictionary.ContainsKey(GetType() + eventName))
        {
            strDictionary[eventName].RemoveListener(unityAction);
        }
        else
        {
            Debug.Log("删除事件出现错误");
        }
    }

    public void InvokeEvent<T>(T eventType) where T : Enum //执行所有事件
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if(strDictionary.ContainsKey(eventName))
        {
            strDictionary[eventName].Invoke();
        }
        else
        {
            Debug.Log("调用事件出现错误");
        }
    }
    #endregion //不使用参数的事件系统

    #region 使用一个float类型作为参数的事件系统
/*    public void AddListener<T>(T eventType, UnityAction<float> unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        Debug.Log(eventName);
        if (singleValueDic.ContainsKey(eventName))
        {
            singleValueDic[eventName].AddListener(unityAction);
        }
        else
        {
            UnityEvent<float> currentEvent = new SingleValueUnityEvent();
            currentEvent.AddListener(unityAction);
            singleValueDic.Add(eventName, currentEvent);
        }
    }*/

    public void RemoveListener<T>(T eventType, UnityAction<float> unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if (singleValueDic.ContainsKey(eventName))
        {
            singleValueDic[eventName].RemoveListener(unityAction);
        }
        else
        {
            //throw new System.Exception("删除事件出现错误");
            Debug.Log("删除事件出现错误");
        }
    }

    public void InvokeEvent<T>(T eventType, float value) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if(singleValueDic.ContainsKey(eventName))
        {
            singleValueDic[eventName].Invoke(value);
        }
        else
        {
            //throw new System.Exception("调用事件出现错误");
            Debug.Log("调用事件出现错误");
        }
    }
    #endregion //使用一个float类型作为参数的事件系统

    #region 使用两个float类型作为参数的事件系统
    public void AddListener<T>(T eventType, UnityAction<float, float> unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        Debug.Log(eventName);
        if (doubleValueDic.ContainsKey(eventName))
        {
            doubleValueDic[eventName].AddListener(unityAction);
        }
        else
        {
            UnityEvent<float, float> currentEvent = new DoubleValueUnityEvent();
            currentEvent.AddListener(unityAction);
            doubleValueDic.Add(eventName, currentEvent);
        }
    }

    public void RemoveListener<T>(T eventType, UnityAction<float, float> unityAction) where T : Enum
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if (doubleValueDic.ContainsKey(eventName))
        {
            doubleValueDic[eventName].RemoveListener(unityAction);
        }
        else
        {
            Debug.Log("删除事件出现错误");
        }
    }

    public void InvokeEvent<T>(T eventType, float first, float second) where T : Enum //执行所有事件
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if(doubleValueDic.ContainsKey(eventName))
        {
            doubleValueDic[eventName].Invoke(first, second);
        }
        else
        {
            Debug.Log("调用事件出现错误");
        }
    }
    #endregion //使用两个float类型作为参数的事件系统

    #region 使用object作为参数的事件系统
    public void AddListener<T>(T eventType, UnityAction<object> unityAction)
    {
        string eventName = eventType.GetType().Name + "_" + eventType.ToString();
        if (objectValueDic.ContainsKey(eventName))
        {
            objectValueDic[eventName].AddListener(unityAction);
        }
        else
        {
            UnityEvent<object> currentEvent = new ObjectValueEvent();
            currentEvent.AddListener(unityAction);
            objectValueDic.Add(eventName, currentEvent);
        }
    }
    public void RemoveListener<T>(T eventType, UnityAction<object> unityAction) where T : Enum
    {
        string eventName = eventType.GetType() + "_" + eventType.ToString();
        if (objectValueDic.ContainsKey(eventName))
        {
            objectValueDic[eventName].RemoveListener(unityAction);
        }
        else
        {
            Debug.Log("删除事件错误");
        }
    }

    public void InvokeEvent<T>(T eventType, object objectData) where T : Enum
    {
        string eventName = eventType.GetType() + "_" + eventType.ToString();
        if (objectValueDic.ContainsKey(eventName))
        {
            objectValueDic[eventName].Invoke(objectData);
        }
        else
        {
            Debug.Log("调用事件出现错误");
        }
    }
    #endregion //使用object作为参数的事件系统
}
