using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : UnitySingleton<ResourceManager>
{
    //������Դ
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    //��ʼ����Դ���б�
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

        //��ӳ�ʼ��Դ
        foreach (ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }


    /// <summary>
    /// �����Դ
    /// </summary>
    /// <param name="resourceType"></param>
    /// <param name="amount"></param>
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
    }

    /// <summary>
    /// ��ȡ��Դ����
    /// </summary>
    /// <param name="resourceType"></param>
    /// <returns></returns>
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    /// <summary>
    /// �Ƿ��ܹ����������ɱ�
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
    /// ������Դ
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
