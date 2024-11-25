using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// �������˳��¼�
/// </summary>
public class MouseEnterExitEvents : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public event EventHandler OnMouseEnter;
    public event EventHandler OnMouseExit;

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// �˳�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(this, EventArgs.Empty);
    }
}
