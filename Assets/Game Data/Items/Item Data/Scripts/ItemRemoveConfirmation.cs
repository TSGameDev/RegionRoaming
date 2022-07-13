using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRemoveConfirmation : MonoBehaviour
{

    public GameObject uiItem;
    [SerializeField] private PlayerConnector playerConnector;
    [SerializeField] private Transform playerInventory;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private TextMeshProUGUI amountTxt;

    private ItemScriptableObject item;

    public void Open()
    {
        gameObject.SetActive(true);
        uiItem.transform.SetParent(playerInventory);
        uiItem.transform.SetAsLastSibling();

        item = uiItem.GetComponent<Interactions>().item;

        amountSlider.minValue = 1;
        amountSlider.maxValue = playerConnector.playerInventory[item];
    }

    public void UpdateText()
    {
        amountTxt.text = amountSlider.value.ToString();
    }

    public void CancelItemRemoval()
    {
        gameObject.SetActive(false);
        item = null;
    }

    public void RemoveItem()
    {
        playerConnector.removeItemFromInventory.Invoke(item, (int)amountSlider.value);
        CancelInvoke();
    }
}
