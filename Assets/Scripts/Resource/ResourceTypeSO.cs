using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    public string nameString;
    [Header("���Ƽ�д")]
    public string nameShort;
    public Sprite sprite;
    [Header("16������ɫ")]
    public string colorHex;

}
