using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private Color trueColor;
    [SerializeField] private Color errorColor;
    [SerializeField]private ResourceNearbyOverlay resourceNearbyOverlay;
    private GameObject spriteGameObject;
    private GameObject rangeSpriteGameObject;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        //��Ϣ�¼�
        MsgManager.AddMessage<BuildingTypeSO>(EventTypes.ShowBuildingTypeGhost, Show);
        MsgManager.AddMessage(EventTypes.HideBuildingTypeGhost, Hide);

        spriteGameObject = transform.Find("sprite").gameObject;
        rangeSpriteGameObject = transform.Find("rangeSprite").gameObject;
        

        Hide();
    }

    private void OnDestroy()
    {
        //ɾ����Ӧ
        MsgManager.RemoveMessage<BuildingTypeSO>(EventTypes.ShowBuildingTypeGhost, Show);
        MsgManager.RemoveMessage(EventTypes.HideBuildingTypeGhost, Hide);
    }

    private void Update()
    {
        activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        transform.position = UtilsClass.GetMouseWorldPosition();
        UpdateActiveBuildingTypeColor();
        UpdateRangeSprite();
    }

    /// <summary>
    /// �����Ƿ�����ʾ
    /// </summary>
    private void UpdateActiveBuildingTypeColor()
    {
        if (activeBuildingType == null) return;

        //�Ƿ���Խ���
        bool canSpawnBuilding = BuildingManager.Instance.CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage);
        //�Ƿ��ܹ��е��ɱ�
        bool canAfford = ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray);

        if (canSpawnBuilding && canAfford)
        {
            spriteGameObject.GetComponent<SpriteRenderer>().color = trueColor;
        }
        else
        {
            spriteGameObject.GetComponent<SpriteRenderer>().color = errorColor;
        }

        Color spriteColor = spriteGameObject.GetComponent<SpriteRenderer>().color;
        Color rangeSpriteColor = rangeSpriteGameObject.GetComponent<SpriteRenderer>().color;
        Color color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, rangeSpriteColor.a);
        rangeSpriteGameObject.GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// ���·�Χ��ʾͼ���С
    /// </summary>
    private void UpdateRangeSprite()
    {
        if (activeBuildingType == null) return;

        float rangeSize = activeBuildingType.resourceGeneratorData.resourceDataRadius * 2;
        Debug.Log("mmm: " + rangeSize+"  nnn: "+ activeBuildingType.minConstructionRadius);
        rangeSpriteGameObject.transform.localScale = new Vector3(rangeSize, rangeSize, 1);
    }

    private void Show(BuildingTypeSO resourceType)
    {
        spriteGameObject.SetActive(true);
        rangeSpriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = resourceType.sprite;

        //�ٷֱ���ʾ
        if(resourceType.resourceGeneratorData.resourceType != null)
        {
            resourceNearbyOverlay.Show(resourceType.resourceGeneratorData);
        }
        else
        {
            resourceNearbyOverlay.Hide();
        }
    }

    private void Hide()
    {
        spriteGameObject.SetActive(false);
        rangeSpriteGameObject.SetActive(false);
        resourceNearbyOverlay.Hide();
    }
}
