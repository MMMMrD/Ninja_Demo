using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemInfoText;
    RectTransform rectTransform;

    void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();    
    }

    public void SetUpTooltip(ItemData_SO itemData)
    {
        itemNameText.text = itemData.itemName;
        itemInfoText.text = itemData.description;
    }

    void OnEnable() 
    {
        UpdatePosition();    
    }

    void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        //获取图片四个角落的坐标
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].x - corners[0].x;

        if(mousePos.y < height)
        {
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        }
        else if(Screen.width - mousePos.x > width)
        {
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else
        {
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
        }
    }
}
