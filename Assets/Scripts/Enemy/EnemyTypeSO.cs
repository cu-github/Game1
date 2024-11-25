using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyTypeSO")]
public class EnemyTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;

    [Header("����")]
    public int attack;
    [Header("���Ѫ��")]
    public int healthAmountMax;
    [Header("�ƶ��ٶ�")]
    public float moveSpeed;
    [Header("�����ķ�Χ")]
    public float lookRadius;

}
