using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactions : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private UIManagerConnector uiManagerConnector;
    [SerializeField] private PlayerConnector playerConnector;

    public ItemScriptableObject item;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        uiManagerConnector.onItemHover.Invoke(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiManagerConnector.onItemHoverExit.Invoke();
    }
}
