using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    //Ŀ��λ��
    private Transform targetTransform;
    //�������ͽű�����
    private EnemyTypeSO enemyTypeOS;
    //�鿴���˵ļ��ʱ��
    private float lookForTargetsTimer;
    private float lookForTargetsTimerMax = .2f;
    //����ϵͳ
    private HealthSystem healthSystem;

    private void Start()
    {
        enemyTypeOS = EnemyManager.Instance.GetEnemyTypeSO(EnemyTypes.Dogface);
        rigidbody2D = GetComponent<Rigidbody2D>();
        //Ŀ��λ��Ϊ����λ��
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;

        //��ֹһ�ѵ��˽�����ȫ��ͬ��֡
        lookForTargetsTimer = Random.Range(0f, lookForTargetsTimerMax);

        //�������Ѫ��
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(enemyTypeOS.healthAmountMax, true);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();

    }

    /// <summary>
    /// ��ײ���
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();

        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            //����������
            healthSystem.Damage(enemyTypeOS.attack);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �����˶�
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
    /// ����Ŀ��
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
    /// ������Ұ��Χ�ڵĵ���
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
                    //�ж����Building����
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
