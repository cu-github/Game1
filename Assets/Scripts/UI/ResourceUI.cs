using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    private float offsetAmount = -210f;
    private ResourceTypeListSo resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

    private void Awake()
    {
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        resourceTypeList = Resources.Load<ResourceTypeListSo>(
            "ScriptableObjects/" + typeof(ResourceTypeListSo).Name);

        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        //������Դ���͵ļ��ϴ���UI
        int index = 0;
        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            //����λ��
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            //����ͼ��
            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            
            resourceTypeTransformDictionary[resourceType] = resourceTransform;
            index++;
        }
    }

    private void Start()
    {
        UpdateResourceAmount();
    }

    private void Update()
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];

            //������Դ����
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
