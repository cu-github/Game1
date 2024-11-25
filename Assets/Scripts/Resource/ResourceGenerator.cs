using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Դ����
/// </summary>
public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timeMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        //�жϸ����Ƿ������Դ
        if (nearbyResourceAmount == 0)
        {
            enabled = false;
        }
        else
        {
            timerMax = (resourceGeneratorData.timeMax / 2f) + resourceGeneratorData.timeMax * 
                (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    /// <summary>
    /// ��ȡ��������Դ��
    /// </summary>
    /// <param name="resourceGeneratorData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDataRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return timer / timerMax;
    }

    /// <summary>
    /// ��ȡÿ�����ɵ�����
    /// </summary>
    /// <returns></returns>
    public float GetAmountGeneratedPerSecond()
    {
        return 1 / timerMax;
    }
}
