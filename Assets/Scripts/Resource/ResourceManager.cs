using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : UnitySingleton<ResourceManager>
{
    //所有资源
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    //开始的资源量列表
    [SerializeField] private List<ResourceAmount> startingResourceAmountList;

    private void Awake()
    {
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSo resourceTypeList = Resources.Load<ResourceTypeListSo>(
            "ScriptableObjects/" + typeof(ResourceTypeListSo).Name);

        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        //添加初始资源
        foreach (ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }


    /// <summary>
    /// 添加资源
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
    }

    /// <summary>
    /// 获取资源数量
    /// </summary>
    /// <param name="resourceType"></param>
    /// <returns></returns>
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    /// <summary>
    /// 是否能够负担建筑成本
    /// </summary>
    /// <param name="resourceAmountArray"></param>
    /// <returns></returns>
    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        if (resourceAmountArray == null) return false;
        foreach(ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 消耗资源
    /// </summary>
    /// <param name="resourceAmountArray"></param>
    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach(ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
