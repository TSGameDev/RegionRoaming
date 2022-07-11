using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    #region Dependices
    [SerializeField] PlayerConnector playerConnector;
    [SerializeField] UIManagerConnector uiManagerConnector;

    NavMeshAgent agent;
    Animator animController;

    #endregion

    #region Movement Variables

    Vector3 movePos;
    int terrainLayer = 1 << 6;
    float closeEnough = 0.2f;

    #endregion

    #region Player Inventory

    public GameObject inventoryUI;

    #endregion

    #region Player Interaction

    public TextMeshProUGUI interactionText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Dependience collection
        agent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the distance between the movement order and current position is less than 0.2, stop walking and runing animations.
        if (agent.hasPath)
        {
            if (agent.remainingDistance < closeEnough)
            {
                animController.SetBool(playerConnector.animRunHash, false);
                animController.SetBool(playerConnector.animWalkHash, false);
            }
        }
    }

    /// <summary>
    /// Function that controls issuing a movement command to the player.
    /// </summary>
    public void PlayerMove()
    {
        //Get a terrain positon from a click on the screen.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, terrainLayer))
        {
            //Assigned terrain poition to the movePos variable
            movePos = hit.point;
            agent.SetDestination(movePos);
        }

        //If the distance between the issued movement order position is greater than the walk threshol, makes the player run to the location.
        if (agent.remainingDistance >= playerConnector.walkThreshold)
        {
            agent.speed = playerConnector.runSpeed;
            animController.SetBool(playerConnector.animRunHash, true);
            animController.SetBool(playerConnector.animWalkHash, false);
        }
        //Else the player will walk to the location
        else if(agent.remainingDistance < playerConnector.walkThreshold)
        {
            agent.speed = playerConnector.walkSpeed;
            animController.SetBool(playerConnector.animWalkHash, true);
            animController.SetBool(playerConnector.animRunHash, false);
        }
    }

    /// <summary>
    /// Function to add items to the players inventory. Returns a bool for if the addition was successful.
    /// </summary>
    /// <param name="ingredient">The item to add to the players inventory.</param>
    /// <param name="amount">The amoutn of the item to add to the players inventory.</param>
    /// <returns>A bool, True if the item was added to the players inventory. False if the item was not added to the players inventory.</returns>
    public bool AddItemToInventory(ItemScriptableObject ingredient, int amount)
    {
        if (playerConnector.playerInventory.Count == 30)
        {
            return false;
        }

        Sprite itemInventoryImage = ingredient.ingredientImage;

        if (playerConnector.playerInventory.ContainsKey(ingredient))
        {
            playerConnector.playerInventory[ingredient] += amount;
            GameObject itemUI = playerConnector.playerInventoryUI[ingredient];
            string itemAmount = playerConnector.playerInventory[ingredient].ToString();
            TextMeshProUGUI itemUITxt = itemUI.GetComponentInChildren<TextMeshProUGUI>();
            itemUITxt.text = itemAmount;

        }
        else
        {
            playerConnector.playerInventory.Add(ingredient, amount);
            GameObject itemUI = Instantiate(playerConnector.itemInventoryImage, inventoryUI.transform);
            itemUI.GetComponentInChildren<Image>().sprite = itemInventoryImage;
            itemUI.GetComponentInChildren<TextMeshProUGUI>().text = playerConnector.playerInventory[ingredient].ToString();
            
            EventTrigger trigger = itemUI.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { uiManagerConnector.onItemHover.Invoke(ingredient); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => { uiManagerConnector.onItemHoverExit.Invoke(); });
            trigger.triggers.Add(entry);

            playerConnector.playerInventoryUI.Add(ingredient, itemUI);
        }
        return true;
    }

    /// <summary>
    /// Function that removes an item from the player inventory.
    /// </summary>
    /// <param name="ingredient">the ingredient to remove.</param>
    /// <param name="amount">the amount of that ingredient to remove.</param>
    /// <returns>A bool, True if the item is removed from the inventory. False if the item was not removed from the inventory</returns>
    public bool RemoveItemFromInventory(ItemScriptableObject ingredient, int amount)
    {
        if(playerConnector.playerInventory[ingredient] == 1)
        {
            playerConnector.playerInventory.Remove(ingredient);
            GameObject ItemUI = playerConnector.playerInventoryUI[ingredient];
            Destroy(ItemUI);
            playerConnector.playerInventoryUI.Remove(ingredient);
            return true;
        }
        else
        {
            playerConnector.playerInventory[ingredient] -= amount;
            playerConnector.playerInventoryUI[ingredient].GetComponentInChildren<TextMeshProUGUI>().text = playerConnector.playerInventory[ingredient].ToString();
            return true;
        }
    }
}
