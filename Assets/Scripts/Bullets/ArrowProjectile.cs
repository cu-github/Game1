using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 箭弹
/// </summary>
public class ArrowProjectile : MonoBehaviour
{
    //目标敌人
    private Enemy targetEnemy;
    //移动速度
    [SerializeField]
    [Header("移动速度")]
    private float moveSpeed = 20f;
    //攻击力
    [SerializeField]
    private int damageAmount = 10;
    //记录上一次目标位置
    private Vector3 lastMoveDir;
    //存活时间
    private float timeToDie = 2f;

    /// <summary>
    /// 创建箭弹
    /// </summary>
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>("Bullets/pfArrowProjectile");
        Transform prefab = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        if (prefab != null)
        {
            ArrowProjectile arrowProjectile = prefab.GetComponent<ArrowProjectile>();
            arrowProjectile.SetTargetEnemy(enemy);

            return arrowProjectile;
        }
        else
        {
            return null;
        }
    }

    void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            //失去目标后继续移动
            moveDir = lastMoveDir;
        }
        
        //向目标移动
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        //转向目标
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        //存活倒计时
        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTargetEnemy(Enemy enemy)
    {
        targetEnemy = enemy;
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //敌人受伤
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
        }
    }
}
