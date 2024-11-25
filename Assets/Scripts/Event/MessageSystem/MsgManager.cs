using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgManager
{
    private static Dictionary<EventTypes, Delegate> m_EventTable = new Dictionary<EventTypes, Delegate>();

    private static void OnListenerAdding(EventTypes EventTypes, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(EventTypes))
        {
            m_EventTable.Add(EventTypes, null);
        }
        Delegate d = m_EventTable[EventTypes];
        if (d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", EventTypes, d.GetType(), callBack.GetType()));
        }
    }
    private static void OnListenerRemoving(EventTypes EventTypes, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(EventTypes))
        {
            Delegate d = m_EventTable[EventTypes];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", EventTypes));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", EventTypes, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", EventTypes));
        }
    }
    private static void OnListenerRemoved(EventTypes EventTypes)
    {
        if (m_EventTable[EventTypes] == null)
        {
            m_EventTable.Remove(EventTypes);
        }
    }
    //no parameters
    public static void AddMessage(EventTypes EventTypes, CallBack callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack)m_EventTable[EventTypes] + callBack;
    }
    //Single parameters
    public static void AddMessage<T>(EventTypes EventTypes, CallBack<T> callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T>)m_EventTable[EventTypes] + callBack;
    }
    //two parameters
    public static void AddMessage<T, X>(EventTypes EventTypes, CallBack<T, X> callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X>)m_EventTable[EventTypes] + callBack;
    }
    //three parameters
    public static void AddMessage<T, X, Y>(EventTypes EventTypes, CallBack<T, X, Y> callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y>)m_EventTable[EventTypes] + callBack;
    }
    //four parameters
    public static void AddMessage<T, X, Y, Z>(EventTypes EventTypes, CallBack<T, X, Y, Z> callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y, Z>)m_EventTable[EventTypes] + callBack;
    }
    //five parameters
    public static void AddMessage<T, X, Y, Z, W>(EventTypes EventTypes, CallBack<T, X, Y, Z, W> callBack)
    {
        OnListenerAdding(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y, Z, W>)m_EventTable[EventTypes] + callBack;
    }

    //no parameters
    public static void RemoveMessage(EventTypes EventTypes, CallBack callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }
    //single parameters
    public static void RemoveMessage<T>(EventTypes EventTypes, CallBack<T> callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T>)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }
    //two parameters
    public static void RemoveMessage<T, X>(EventTypes EventTypes, CallBack<T, X> callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X>)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }
    //three parameters
    public static void RemoveMessage<T, X, Y>(EventTypes EventTypes, CallBack<T, X, Y> callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y>)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }
    //four parameters
    public static void RemoveMessage<T, X, Y, Z>(EventTypes EventTypes, CallBack<T, X, Y, Z> callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y, Z>)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }
    //five parameters
    public static void RemoveMessage<T, X, Y, Z, W>(EventTypes EventTypes, CallBack<T, X, Y, Z, W> callBack)
    {
        OnListenerRemoving(EventTypes, callBack);
        m_EventTable[EventTypes] = (CallBack<T, X, Y, Z, W>)m_EventTable[EventTypes] - callBack;
        OnListenerRemoved(EventTypes);
    }


    //no parameters
    public static void Send(EventTypes EventTypes)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
    //single parameters
    public static void Send<T>(EventTypes EventTypes, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
    //two parameters
    public static void Send<T, X>(EventTypes EventTypes, T arg1, X arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack<T, X> callBack = d as CallBack<T, X>;
            if (callBack != null)
            {
                callBack(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
    //three parameters
    public static void Send<T, X, Y>(EventTypes EventTypes, T arg1, X arg2, Y arg3)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
    //four parameters
    public static void Send<T, X, Y, Z>(EventTypes EventTypes, T arg1, X arg2, Y arg3, Z arg4)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3, arg4);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
    //five parameters
    public static void Send<T, X, Y, Z, W>(EventTypes EventTypes, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(EventTypes, out d))
        {
            CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
            if (callBack != null)
            {
                callBack(arg1, arg2, arg3, arg4, arg5);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", EventTypes));
            }
        }
    }
}
