using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
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
        //ɾ����Ӧ
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
    /// ����Ѫ��
    /// </summary>
    private void UpdateBar()
    {
        float v = healthSystem.GetHealthAmountNormalized();
        barTransform.localScale = new Vector3(v, 1, 1);
    }

    /// <summary>
    /// Ѫ�����Ƿ�ɼ�
    /// </summary>
    private void UpdateHealthBarVisible()
    {
        //�������������
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
