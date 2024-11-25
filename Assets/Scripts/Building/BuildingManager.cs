using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    [Header("����")]
    private Building hqBuilding;
    public static BuildingManager Instance { get; private set; }
    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;
    

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(
            "ScriptableObjects/" + typeof(BuildingTypeListSO).Name);

    }

    private void Start()
    {
        mainCamera = Camera.main;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(activeBuildingType != null)
            {
                //�Ƿ���Խ���
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                {
                    //�Ƿ��ܹ��е�����ɱ�
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("�޷��е� " + 
                            activeBuildingType.GetConstructionResourceCostString(), new TooltipUI.TooltipTimer(2f));
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer(2f));
                }
            }
        }

    }

    /// <summary>
    /// ���ý���Ĭ������
    /// </summary>
    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;

        MsgManager.Send(EventTypes.UpdateActiveBuildingTypeButton);
        if (buildingType != null)
            MsgManager.Send(EventTypes.ShowBuildingTypeGhost, buildingType);
        else
            MsgManager.Send(EventTypes.HideBuildingTypeGhost);
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    /// <summary>
    /// �Ƿ���Կ������ɽ���
    /// </summary>
    /// <param name="buildingType"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        if (buildingType == null)
        {
            errorMessage = "";
            return false;
        }

        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        //���ص��Ķ������ܽ���
        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) 
        {
            errorMessage = "���������";
            return false;
        }

        //��ȡ�뾶�ڵ���Դ����
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                //��Χ���Ѿ�����ͬ������
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = "��ͬ���͵Ľ���̫���ˣ�";
                    return false;
                }
            }
        }

        //��ȡ�뾶�ڵ���Դ����
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.maxConstructionRadius);
        //�жϽ������������Χ
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "����������̫Զ�ˣ�";
        return false;
    }


    public Building GetHQBuilding()
    {
        return hqBuilding;
    }
}
