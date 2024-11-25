using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人
/// </summary>
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    //目标位置
    private Transform targetTransform;
    //敌人类型脚本对象
    private EnemyTypeSO enemyTypeOS;
    //查看敌人的间隔时间
    private float lookForTargetsTimer;
    private float lookForTargetsTimerMax = .2f;
    //健康系统
    private HealthSystem healthSystem;

    private void Start()
    {
        enemyTypeOS = EnemyManager.Instance.GetEnemyTypeSO(EnemyTypes.Dogface);
        rigidbody2D = GetComponent<Rigidbody2D>();
        //目标位置为基地位置
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;

        //防止一堆敌人进入完全相同的帧
        lookForTargetsTimer = Random.Range(0f, lookForTargetsTimerMax);

        //设置最大血量
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(enemyTypeOS.healthAmountMax, true);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();

    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            //防御塔受伤
            healthSystem.Damage(enemyTypeOS.attack);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 处理运动
    /// </summary>
    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = enemyTypeOS.moveSpeed;
            rigidbody2D.velocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 处理目标
    /// </summary>
    private void HandleTargeting()
    {
        lookForTargetsTimer -= Time.deltaTime;
        if (lookForTargetsTimer < 0)
        {
            lookForTargetsTimer += lookForTargetsTimerMax;
            LookForTargets();
        }
    }

    /// <summary>
    /// 跟随视野范围内的敌人
    /// </summary>
    private void LookForTargets()
    {
        float lookRadius = enemyTypeOS.lookRadius;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, lookRadius);

        foreach(Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if(building != null)
            {
                if(targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    //判断最近Building敌人
                    if (Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, targetTransform.position)){
                        targetTransform = building.transform;
                    }
                }
            }
        }


        if(targetTransform == null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
