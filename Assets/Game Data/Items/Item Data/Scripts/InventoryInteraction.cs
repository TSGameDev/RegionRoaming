using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryInteraction : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        ItemScriptableObject item = eventData.pointerDrag.GetComponent<Interactions>().item;
        if(eventData.pointerDrag != null & item != null)
        {
            Transform itemTransform = eventData.pointerDrag.GetComponent<Transform>();
            itemTransform.SetParent(transform);
            itemTransform.SetAsLastSibling();
        }
    }
}
