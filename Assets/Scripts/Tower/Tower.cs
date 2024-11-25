using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //Ŀ�����
    private Enemy targetEnemy;
    //�ڵ�����λ��
    private Vector3 projectileSpawnPosition;
    //�鿴���˵ļ��ʱ��
    private float lookForTargetsTimer;
    [SerializeField] private float lookForTargetsTimerMax = 0.2f;
    //�����ӵ��ļ��ʱ��
    private float shootTimer;
    [SerializeField] private float shootTimerMax = 0.3f;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Start()
    {
        //towerBuildingTypeOS = GetComponent<BuildingTypeHolder>().buildingType;
        //��ֹһ�ѵ��˽�����ȫ��ͬ��֡
        lookForTargetsTimer = lookForTargetsTimerMax;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    /// <summary>
    /// �������
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
    /// ���ʱ�䴦��Ŀ��
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
    /// �鿴��Ұ��Χ�ڵĵ���
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
                    //�ж����Building����
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
