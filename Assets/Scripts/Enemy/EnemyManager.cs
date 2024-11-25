using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人类型
/// </summary>
public enum EnemyTypes
{
    Dogface = 0,

}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private EnemyTypeListSO enemyTypeList;

    private void Awake()
    {
        Instance = this;
        enemyTypeList = Resources.Load<EnemyTypeListSO>(
            "ScriptableObjects/" + typeof(EnemyTypeListSO).Name);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 enemySpawnPosition = UtilsClass.GetMouseWorldPosition() + UtilsClass.GetRandomDir() * 5f;

            Create(EnemyTypes.Dogface, enemySpawnPosition);
        }

    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    public Enemy Create(EnemyTypes enemyType, Vector3 position)
    {
        Transform prefab = Instantiate(GetEnemyTypeSO(enemyType).prefab, position, Quaternion.identity);

        if (prefab != null)
        {
            Enemy enemy = prefab.GetComponent<Enemy>();

            return enemy;
        }
        else
        {
            return null;
        }
    }

    public EnemyTypeSO GetEnemyTypeSO(EnemyTypes enemyType)
    {
        return enemyTypeList.list[(int)enemyType];
    }

}
