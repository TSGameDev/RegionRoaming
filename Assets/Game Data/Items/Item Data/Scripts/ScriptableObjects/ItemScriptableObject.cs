using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum IngredientType
{
    Base,
    Solute,
    Potion,
    Junk
}

public enum AlchemicalTraits
{
    Energize,
    Heal,
    Cure,
    Poison,
    Antidote,
}

public struct BunsenBurnerProcess
{
    public BunsenBurnerProcess(int minTemp, int maxTemp, ItemScriptableObject result)
    {
        this.minTemp = minTemp;
        this.maxTemp = maxTemp;
        this.result = result;
    }

    public int minTemp;
    public int maxTemp;
    public ItemScriptableObject result;
}

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Scriptable Object/New Ingredient", order = 4)]
public class ItemScriptableObject : SerializedScriptableObject
{
    [BoxGroup("General Information")]
    [HorizontalGroup("General Information/Split", Width = 80)]
    [VerticalGroup("General Information/Split/Left")]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [HideLabel]
    public Sprite ingredientImage;
    [VerticalGroup("General Information/Split/Right")]
    public int ID;
    [VerticalGroup("General Information/Split/Right")]
    public string ingredientName;
    [VerticalGroup("General Information/Split/Right")]
    public int ingredientTier;
    [VerticalGroup("General Information/Split/Right")]
    public GameObject ingredientPrefab;
    [VerticalGroup("General Information/Split/Right")]
    public IngredientType ingredientType;
    [VerticalGroup("General Information/Split/Right")]
    public int minValue;
    [VerticalGroup("General Information/Split/Right")]
    public int maxValue;

    [BoxGroup("General Information")]
    [Multiline]
    public string itemDescription;

    [BoxGroup("Alchemical Properties")]
    public List<AlchemicalTraits> alchemicalTratis;

    [BoxGroup("Alchemical Properties")]
    public List<ItemScriptableObject> alchemyCraftingRecipe = new List<ItemScriptableObject>();

    [FoldoutGroup("Alchemical Properties/Processes")]
    public bool canMortarPestle;
    [HorizontalGroup("Alchemical Properties/Processes/MortarPestle")]
    [HideIf("@!canMortarPestle")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject mortarPestleResult;
    [HorizontalGroup("Alchemical Properties/Processes/MortarPestle")]
    [HideIf("@!canMortarPestle")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject mortarPestleResult2;

    [FoldoutGroup("Alchemical Properties/Processes")]
    public bool canJuicer;
    [HorizontalGroup("Alchemical Properties/Processes/Juicer")]
    [HideIf("@!canJuicer")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject juicerResult;
    [HorizontalGroup("Alchemical Properties/Processes/Juicer")]
    [HideIf("@!canJuicer")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject juicerResult2;

    [FoldoutGroup("Alchemical Properties/Processes")]
    public bool canChopping;
    [HorizontalGroup("Alchemical Properties/Processes/Chopping")]
    [HideIf("@!canChopping")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject choppingResult;
    [HorizontalGroup("Alchemical Properties/Processes/Chopping")]
    [HideIf("@!canChopping")]
    [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    [HideLabel]
    public ItemScriptableObject choppingResult2;

    [FoldoutGroup("Alchemical Properties/Processes")]
    public bool canImbue;
    [FoldoutGroup("Alchemical Properties/Processes")]
    [HideIf("@!canImbue")]
    public List<ItemScriptableObject> ImbueRecipe = new List<ItemScriptableObject>();

    [FoldoutGroup("Alchemical Properties/Processes")]
    public bool canBunsenBurner;
    [FoldoutGroup("Alchemical Properties/Processes")]
    [HideIf("@!canBunsenBurner")]
    public List<BunsenBurnerProcess> bunsenProcesses = new List<BunsenBurnerProcess>();
}
