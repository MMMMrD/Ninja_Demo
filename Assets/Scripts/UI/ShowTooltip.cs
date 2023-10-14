using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GetComponent<ItemUI>().currentItemData != null)
        {
            QuestUIManager.Instance.tooltip.gameObject.SetActive(true);
            QuestUIManager.Instance.tooltip.SetUpTooltip(GetComponent<ItemUI>().currentItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUIManager.Instance.tooltip.gameObject.SetActive(false);
    }
}
