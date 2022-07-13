using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    [SerializeField] private PlayerConnector playerConnector;
    [SerializeField] private ItemScriptableObject harvestItem;
    [SerializeField] private int minHarvestAmount;
    [SerializeField] private int maxHarvestAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.interactionText.text = $"Press F to Pick Up {harvestItem.ingredientName}";
            player.interactionText.gameObject.SetActive(true);
            playerConnector.playerInteraction = new PlayerConnector.Interaction(() =>
            {
                int harvestAmount = Random.Range(minHarvestAmount, maxHarvestAmount);
                player.AddItemToInventory(harvestItem, harvestAmount);
                OnTriggerExit(other);
                Destroy(gameObject);
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.interactionText.text = $"";
            player.interactionText.gameObject.SetActive(false);
            playerConnector.playerInteraction = new PlayerConnector.Interaction(() => { Debug.Log("No Interaction Set"); });
        }
    }
}
