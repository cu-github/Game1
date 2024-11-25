using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    [Header("基地")]
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
                //是否可以建筑
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                {
                    //是否能够承担建造成本
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("无法承担 " + 
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
    /// 设置建筑默认类型
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
    /// 是否可以可以生成建筑
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

        //有重叠的对象则不能建造
        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) 
        {
            errorMessage = "区域不清楚！";
            return false;
        }

        //获取半径内的资源对象
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if(buildingTypeHolder != null)
            {
                //范围内已经有相同建筑物
                if(buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = "离同类型的建筑太近了！";
                    return false;
                }
            }
        }

        //获取半径内的资源对象
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.maxConstructionRadius);
        //判断建筑物距离的最大范围
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "离其他建筑太远了！";
        return false;
    }


    public Building GetHQBuilding()
    {
        return hqBuilding;
    }
}
