using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ��������Դ����
/// </summary>
public class ResourceNearbyOverlay : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        UpdateResourceAmountPercent();
    }

    /// <summary>
    /// ������ʾ������Դ�ĸ���
    /// </summary>
    private void UpdateResourceAmountPercent()
    {
        //��ȡ��������Դ��
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        //��ȡ������Դ�ĸ��� = ��������Դ�� / ����ȡ����Դ�� * 100f
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount * 100f);
        transform.Find("text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
