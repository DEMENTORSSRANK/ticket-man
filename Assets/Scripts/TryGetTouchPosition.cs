using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TryGetTouchPosition : MonoBehaviour
{
    private static GraphicRaycaster _graphicRaycaster;
    
    private void Awake()
    {
        _graphicRaycaster = FindObjectOfType<Canvas>().GetComponent<GraphicRaycaster>();
    }

    public static bool Try(out Vector2 touchPosition)
    {
        if (Input.GetMouseButton(0))
        {
            if (!IsPointerOverUIObject())
            {
                touchPosition = Input.mousePosition;
                return true;
            }
        }

        touchPosition = default;
        return false;
    }    
    
    public static bool TryRight(out Vector2 touchPosition)
    {
        if (Input.GetMouseButton(1))
        {
            if (!IsPointerOverUIObject())
            {
                touchPosition = Input.mousePosition;
                return true;
            }
        }

        touchPosition = default;
        return false;
    }

    public static bool TryOnce(out Vector2 touchPosition)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                touchPosition = Input.mousePosition;
                return true;
            }
        }

        touchPosition = default;
        return false;
    }

    private static bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}