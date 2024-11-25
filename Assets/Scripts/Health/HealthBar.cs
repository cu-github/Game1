using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生命条
/// </summary>
public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;

    private void Awake()
    {
        MsgManager.AddMessage<GameObject>(EventTypes.OnDamage, OnDamage);
        barTransform = transform.Find("bar");
    }

    private void OnDestroy()
    {
        //删除响应
        MsgManager.RemoveMessage<GameObject>(EventTypes.OnDamage, OnDamage);
    }

    private void Start()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void OnDamage(GameObject gameObject)
    {
        if (gameObject != healthSystem.gameObject) return;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    /// <summary>
    /// 更新血条
    /// </summary>
    private void UpdateBar()
    {
        float v = healthSystem.GetHealthAmountNormalized();
        barTransform.localScale = new Vector3(v, 1, 1);
    }

    /// <summary>
    /// 血量条是否可见
    /// </summary>
    private void UpdateHealthBarVisible()
    {
        //如果健康则隐藏
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
