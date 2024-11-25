using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;

    private void Awake()
    {
        //添加响应
        MsgManager.AddMessage<GameObject>(EventTypes.OnDead, OnDead);
    }

    private void OnDestroy()
    {
        //删除响应
        MsgManager.RemoveMessage<GameObject>(EventTypes.OnDead, OnDead);

    }

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        //设置最大血量
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

    }

    private void OnDead(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
