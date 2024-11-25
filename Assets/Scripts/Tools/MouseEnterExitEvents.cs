using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 鼠标进入退出事件
/// </summary>
public class MouseEnterExitEvents : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public event EventHandler OnMouseEnter;
    public event EventHandler OnMouseExit;

    /// <summary>
    /// 进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// 退出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(this, EventArgs.Empty);
    }
}
