using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamage;
    public event EventHandler OnDead;

    //最大健康值
    [SerializeField]
    [Header("最大血量")]
    private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        MsgManager.Send(EventTypes.OnDamage, this.gameObject);

        if (IsDead())
        {
            MsgManager.Send(EventTypes.OnDead, this.gameObject);
        }
    }

    /// <summary>
    /// 是否死亡
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return healthAmount == 0;
    }

    /// <summary>
    /// 是否满血状态
    /// </summary>
    /// <returns></returns>
    public bool IsFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    /// <summary>
    /// 当前健康值占比
    /// </summary>
    /// <returns></returns>
    public float GetHealthAmountNormalized()
    {
        if(healthAmountMax == 0)
        {
            Debug.Log("提示：healthAmountMax最大血量为0！！！");
            return 0;
        }
        return (float)healthAmount / healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public void SetHealthAmountMax(int healthAmountMax, bool isUpdateHealthAmount)
    {
        this.healthAmountMax = healthAmountMax;

        //是否更新当前血量
        if (isUpdateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
    }
}
