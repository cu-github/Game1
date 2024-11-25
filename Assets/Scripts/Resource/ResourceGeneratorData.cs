using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData
{
    [Header("�ռ��ٶ�")]
    public float timeMax;

    public ResourceTypeSO resourceType;

    [Header("��Դ�ռ���Χ")]
    public float resourceDataRadius;

    [Header("����ռ���Դ��")]
    public int maxResourceAmount;

}
