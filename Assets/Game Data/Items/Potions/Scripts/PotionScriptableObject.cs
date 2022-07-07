using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Potion", menuName = "Scriptable Object/New Potion", order = 3)]
public class PotionScriptableObject : SerializedScriptableObject
{
    [BoxGroup("Potion Information")]
    [HorizontalGroup("Potion Information/Split", 80)]
    [VerticalGroup("Potion Information/Split/Left")]
    [PreviewField(75, ObjectFieldAlignment.Center)]
    [HideLabel]
    public Image potionImage;
    [VerticalGroup("Potion Information/Split/Right")]
    public string potionName;
    [VerticalGroup("Potion Information/Split/Right")]
    public int potionTier;
    [VerticalGroup("Potion Information/Split/Right")]
    public int minValue;
    [VerticalGroup("Potion Information/Split/Right")]
    public int maxValue;
    [VerticalGroup("Potion Information/Split/Right")]
    public List<AlchemicalTraits> potionTraits = new List<AlchemicalTraits>();
    [VerticalGroup("Potion Information/Split/Right")]
    public List<IngredientScriptableObject> craftingRecipe;
}
