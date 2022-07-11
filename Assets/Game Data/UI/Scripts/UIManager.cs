using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Player Inventory

    public GameObject inventoryUI;
    public GameObject toolTipUI;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemTier;
    public TextMeshProUGUI itemDescription;

    private bool inventoryOpen;

    #endregion

    [SerializeField] private UIManagerConnector uIManagerConnector;

    private void Start()
    {
        uIManagerConnector.onItemHover.AddListener(ShowToolTip);
        uIManagerConnector.onItemHoverExit.AddListener(HideToolTip);
    }

    /// <summary>
    /// Function to tween in or out the inventory
    /// </summary>
    public void ShowInventory()
    {
        inventoryOpen = !inventoryOpen;
        if (inventoryOpen)
        {
            inventoryUI.GetComponent<Tween>().BeginTween();
        }
        else
        {
            inventoryUI.GetComponent<Tween>().ReturnTween();
        }

    }

    /// <summary>
    /// function used to display the hovered items tooltip, also performs enterance tween
    /// </summary>
    /// <param name="item">Item to showcase in tooltip</param>
    public void ShowToolTip(ItemScriptableObject item)
    {
        itemImage.sprite = item.ingredientImage;
        itemName.text = item.name;
        itemTier.text = item.ingredientTier.ToString();
        itemDescription.text = item.itemDescription;
        toolTipUI.SetActive(true);

    }

    /// <summary>
    /// Function to hide the tool tip when the mouse is no longer hovering item
    /// </summary>
    public void HideToolTip()
    {
        toolTipUI.SetActive(false);
        itemImage.sprite = null;
        itemName.text = null;
        itemTier.text = null;
        itemDescription.text = null;
    }
}
