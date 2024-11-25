using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyTypeListSO")]
public class EnemyTypeListSO : ScriptableObject
{
    public List<EnemyTypeSO> list;
}
