using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //目标对象
    private Enemy targetEnemy;
    //炮弹生成位置
    private Vector3 projectileSpawnPosition;
    //查看敌人的间隔时间
    private float lookForTargetsTimer;
    [SerializeField] private float lookForTargetsTimerMax = 0.2f;
    //发射子弹的间隔时间
    private float shootTimer;
    [SerializeField] private float shootTimerMax = 0.3f;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Start()
    {
        //towerBuildingTypeOS = GetComponent<BuildingTypeHolder>().buildingType;
        //防止一堆敌人进入完全相同的帧
        lookForTargetsTimer = lookForTargetsTimerMax;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    /// <summary>
    /// 处理射击
    /// </summary>
    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer += shootTimerMax;

            if (targetEnemy != null)
            {
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
            }
        }
    }

    /// <summary>
    /// 间隔时间处理目标
    /// </summary>
    private void HandleTargeting()
    {
        lookForTargetsTimer -= Time.deltaTime;
        if (lookForTargetsTimer <= 0)
        {
            lookForTargetsTimer += lookForTargetsTimerMax;
            LookForTargets();
        }
    }

    /// <summary>
    /// 查看视野范围内的敌人
    /// </summary>
    private void LookForTargets()
    {
        //float lookRadius = towerBuildingTypeOS.lookRadius;
        float lookRadius = 20;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, lookRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    //判断最近Building敌人
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
