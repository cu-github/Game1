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

    [Header("��С���ð뾶")] 
    public float minConstructionRadius;
    [Header("�����ð뾶")] 
    public float maxConstructionRadius;
    [Header("������Դ��Ҫ�ĳɱ���������")]
    public ResourceAmount[] constructionResourceCostArray;
    [Header("���Ѫ��")]
    public int healthAmountMax;

    /// <summary>
    /// ��óɱ�string
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
