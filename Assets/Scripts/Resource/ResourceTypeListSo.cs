using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource/ResourceTypeListSo")]
public class ResourceTypeListSo : ScriptableObject
{
    public List<ResourceTypeSO> list;
}
