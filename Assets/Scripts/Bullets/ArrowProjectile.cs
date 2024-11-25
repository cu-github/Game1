using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class ArrowProjectile : MonoBehaviour
{
    //Ŀ�����
    private Enemy targetEnemy;
    //�ƶ��ٶ�
    [SerializeField]
    [Header("�ƶ��ٶ�")]
    private float moveSpeed = 20f;
    //������
    [SerializeField]
    private int damageAmount = 10;
    //��¼��һ��Ŀ��λ��
    private Vector3 lastMoveDir;
    //���ʱ��
    private float timeToDie = 2f;

    /// <summary>
    /// ��������
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
            //ʧȥĿ�������ƶ�
            moveDir = lastMoveDir;
        }
        
        //��Ŀ���ƶ�
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        //ת��Ŀ��
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        //����ʱ
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
    /// ��ײ���
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            //��������
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
        }
    }
}
