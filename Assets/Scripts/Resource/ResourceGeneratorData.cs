using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData
{
    [Header("收集速度")]
    public float timeMax;

    public ResourceTypeSO resourceType;

    [Header("资源收集范围")]
    public float resourceDataRadius;

    [Header("最大收集资源数")]
    public int maxResourceAmount;

}
