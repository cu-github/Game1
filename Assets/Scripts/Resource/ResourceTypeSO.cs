using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    public string nameString;
    [Header("名称简写")]
    public string nameShort;
    public Sprite sprite;
    [Header("16进制颜色")]
    public string colorHex;

}
