using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Building/BuildingTypeSO")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;

    [Header("最小放置半径")] 
    public float minConstructionRadius;
    [Header("最大放置半径")] 
    public float maxConstructionRadius;
    [Header("建筑资源需要的成本条件集合")]
    public ResourceAmount[] constructionResourceCostArray;
    [Header("最大血量")]
    public int healthAmountMax;

    /// <summary>
    /// 获得成本string
    /// </summary>
    public string GetConstructionResourceCostString()
    {
        string str = "";
        
        foreach(ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.resourceType.colorHex + ">" +
                resourceAmount.resourceType.nameShort + resourceAmount.amount +
                "</color>  ";
        }

        return str;
    }
    
}
