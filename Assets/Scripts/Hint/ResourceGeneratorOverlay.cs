using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 资源加载提示
/// </summary>
public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    private Transform barTransform;

    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        barTransform = transform.Find("bar").GetComponent<Transform>();
        
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        if(resourceGenerator.enabled == true)
        {
            transform.Find("text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        }

    }

    private void Update()
    {
        if (resourceGenerator.enabled == true)
        {
            barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
        }
    }
}
