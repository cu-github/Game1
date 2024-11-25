using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyTypeSO")]
public class EnemyTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;

    [Header("攻击")]
    public int attack;
    [Header("最大血量")]
    public int healthAmountMax;
    [Header("移动速度")]
    public float moveSpeed;
    [Header("看见的范围")]
    public float lookRadius;

}
