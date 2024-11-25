using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;

    private void Awake()
    {
        //�����Ӧ
        MsgManager.AddMessage<GameObject>(EventTypes.OnDead, OnDead);
    }

    private void OnDestroy()
    {
        //ɾ����Ӧ
        MsgManager.RemoveMessage<GameObject>(EventTypes.OnDead, OnDead);

    }

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        //�������Ѫ��
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

    }

    private void OnDead(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
