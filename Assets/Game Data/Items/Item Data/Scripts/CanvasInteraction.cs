using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasInteraction : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemRemoveConfirmation itemRemoval;

    public void OnDrop(PointerEventData eventData)
    {
        itemRemoval.uiItem = eventData.pointerDrag;
        itemRemoval.GetComponent<ItemRemoveConfirmation>().Open();
    }
}
