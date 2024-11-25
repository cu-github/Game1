using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamage;
    public event EventHandler OnDead;

    //��󽡿�ֵ
    [SerializeField]
    [Header("���Ѫ��")]
    private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    /// <summary>
    /// ����
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
    /// �Ƿ�����
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return healthAmount == 0;
    }

    /// <summary>
    /// �Ƿ���Ѫ״̬
    /// </summary>
    /// <returns></returns>
    public bool IsFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    /// <summary>
    /// ��ǰ����ֵռ��
    /// </summary>
    /// <returns></returns>
    public float GetHealthAmountNormalized()
    {
        if(healthAmountMax == 0)
        {
            Debug.Log("��ʾ��healthAmountMax���Ѫ��Ϊ0������");
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

        //�Ƿ���µ�ǰѪ��
        if (isUpdateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
    }
}
