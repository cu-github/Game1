using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    //���ԵĽ������ͼ���
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;
    private Transform arrowBtn;

    private void Awake()
    {
        //��Ϣ�¼�
        MsgManager.AddMessage(EventTypes.UpdateActiveBuildingTypeButton, UpdateActiveBuildingTypeButton);

        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(
            "ScriptableObjects/" + typeof(BuildingTypeListSO).Name);


        int index = 0;

        //======���ð�ť=====
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);
        //λ��
        float offsetAmount = 170f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
        //����ͼ��
        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);
        //����л���������
        arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });
        //====�������˳��¼�====
        MouseEnterExitEvents mouseEnterExitEvents = arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("��ͷ");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };
        index++;

        //���ݽ������ͼ�������UIѡ��ť
        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            //λ��
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            //����ͼ��
            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            //����л���������
            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            //====�������˳��¼�====
            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.nameString + "\n�ɱ���" + buildingType.GetConstructionResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void OnDestroy()
    {
        //ɾ����Ӧ
        MsgManager.RemoveMessage(EventTypes.UpdateActiveBuildingTypeButton, UpdateActiveBuildingTypeButton);

    }
    private void Start()
    {
        UpdateActiveBuildingTypeButton();
    }

    /// <summary>
    /// ����ѡ��İ�ť��ʾ
    /// </summary>
    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);

            BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
            
            if(activeBuildingType == null)
            {
                arrowBtn.Find("selected").gameObject.SetActive(true);
            }
            else
            {
                btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
            }

        }
    }
}
