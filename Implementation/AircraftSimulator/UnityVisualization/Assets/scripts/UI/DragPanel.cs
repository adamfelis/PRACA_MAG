using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragPanel1 : MonoBehaviour , IPointerDownHandler, IDragHandler
{

    private Vector2 pointerOffset;
    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;

    void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;
            panelRectTransform = transform.parent as RectTransform;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        //bring panel to the front
        //panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
    }

    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null)
            return;
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, data.position,
            data.pressEventCamera, out localPointerPosition))
        {
            Vector2 newLocalPosition = localPointerPosition - pointerOffset;
            newLocalPosition.x = Mathf.Clamp(newLocalPosition.x, -canvasRectTransform.rect.width / 2, canvasRectTransform.rect.width / 2 - panelRectTransform.rect.width);
            newLocalPosition.y = Mathf.Clamp(newLocalPosition.y, -canvasRectTransform.rect.height / 2 + panelRectTransform.rect.height, canvasRectTransform.rect.height / 2);
            panelRectTransform.localPosition = newLocalPosition;
        }
    }
}
