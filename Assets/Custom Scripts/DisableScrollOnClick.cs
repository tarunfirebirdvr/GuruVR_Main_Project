using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisableScrollOnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.enabled = true;
    }
}
