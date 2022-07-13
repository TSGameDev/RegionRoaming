using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Scriptable Object/New Item Database", order = 5)]
public class ItemDatabase : SerializedScriptableObject
{
    [BoxGroup("Databases")]
    [PropertyTooltip("A list of items that are added to the ingredient database automatically, given an id as well. " +
    "The list should contain all ingredients that are not created by recipes. (raw ingredients, Processed ingredients other than inbued).")]
    public List<ItemScriptableObject> ingredientItemDatabase = new List<ItemScriptableObject>();
    [BoxGroup("Databases")]
    [PropertyTooltip("A list of items that are added to the ingredient database automatically, given an id as well. " +
    "The list should contain only items created from the inbue process,")]
    public List<ItemScriptableObject> inbueItemDatabase = new List<ItemScriptableObject>();
    [BoxGroup("Databases")]
    [PropertyTooltip("A list of items that are added to the ingredient database automatically, given an id as well. " +
    "The list should contain only potion items.")]
    public List<ItemScriptableObject> potionItemDatabase = new List<ItemScriptableObject>();

    private Dictionary<string, ItemScriptableObject> ingredientDatabase = new Dictionary<string, ItemScriptableObject>();
    private Dictionary<string, ItemScriptableObject> inbueDatabase = new Dictionary<string, ItemScriptableObject>();
    private Dictionary<string, ItemScriptableObject> potionDatabase = new Dictionary<string, ItemScriptableObject>();

    public ItemScriptableObject FindIngredientByID(string ID)
    {
        ingredientDatabase.TryGetValue(ID, out var item);

        if(item == null)
            return null;
        else
            return item;
    }

    public ItemScriptableObject FindInbueItemByID(string ID)
    {
        inbueDatabase.TryGetValue(ID, out var item);

        if (item == null)
            return null;
        else
            return item;
    }

    public ItemScriptableObject FindPotionByID(string ID)
    {
        potionDatabase.TryGetValue(ID, out var item);

        if (item == null)
            return null;
        else
            return item;
    }

    [Button("Databases", Name = "Populate Databases")]
    public void PopulateDatabases()
    {
        for(int i = 0; i < ingredientItemDatabase.Count - 1; i++)
        {
            string itemKey = $"Ingredient_{i}";

            ingredientDatabase.Add(itemKey, ingredientItemDatabase[i]);
        }

        for (int i = 0; i < inbueItemDatabase.Count - 1; i++)
        {
            string itemKey = $"Inbue_{i}";

            inbueDatabase.Add(itemKey, inbueItemDatabase[i]);
        }

        for (int i = 0; i < potionItemDatabase.Count - 1; i++)
        {
            string itemKey = $"Potion_{i}";

            potionDatabase.Add(itemKey, potionItemDatabase[i]);
        }
    }
}
