using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public enum Skills
{
    Alchemy,
    MortarandPestle,
    Juicer,
    Chopping,
    Imbue,
    BunsenBurner
}

[CreateAssetMenu(fileName = "NewPlayerConnector", menuName = "Scriptable Object/Player Connector", order = 0)]
public class PlayerConnector : SerializedScriptableObject
{
    #region Player States

    [TabGroup("base", "Player")]

    #region Alchemy Skill
    [FoldoutGroup("base/Player/Player Stats")]
    [FoldoutGroup("base/Player/Player Stats/Alchemy Skill")]
    [PropertyTooltip("The current level of the players alchemical level.")]
    public int alchemicalLevel;
    [FoldoutGroup("base/Player/Player Stats/Alchemy Skill")]
    [PropertyTooltip("The current XP of the players alchemical skill.")]
    public int alchemicalXP;
    [FoldoutGroup("base/Player/Player Stats/Alchemy Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the alchemical skill")]
    [DictionaryDrawerSettings(KeyLabel = "Alchemy Level", ValueLabel = "Xp To Gain Level")]
    public Dictionary<int, int> alchemicalXPToLevel = new Dictionary<int, int>();
    #endregion

    #region Mortar and Pestle Skill

    [FoldoutGroup("base/Player/Player Stats/Mortar and Pestle Skill")]
    [PropertyTooltip("The current level of the players mortar and pestle skill")]
    public int mortarAndPestleLevel;
    [FoldoutGroup("base/Player/Player Stats/Mortar and Pestle Skill")]
    [PropertyTooltip("The current XP of the players Mortar and Pestle Skill")]
    public int mortarAndPestleXP;
    [FoldoutGroup("base/Player/Player Stats/Mortar and Pestle Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the Mortar And Pestle skill.")]
    [DictionaryDrawerSettings(KeyLabel = "Mortar And Pestle Level", ValueLabel = "Xp to Gain Level")]
    public Dictionary<int, int> mortarAndPestleXPToLevel = new Dictionary<int, int>();

    #endregion

    #region Juicer Skill

    [FoldoutGroup("base/Player/Player Stats/Juicer Skill")]
    [PropertyTooltip("The current level of the players Juicer Skill")]
    public int juicerLevel;
    [FoldoutGroup("base/Player/Player Stats/Juicer Skill")]
    [PropertyTooltip("The current XP of the players Juicer Skill")]
    public int juicerXP;
    [FoldoutGroup("base/Player/Player Stats/Juicer Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the Juicer Skill.")]
    [DictionaryDrawerSettings(KeyLabel = "Juicer Level", ValueLabel = "Xp to Gain Level")]
    public Dictionary<int, int> juicerXPToLevel = new Dictionary<int, int>();

    #endregion

    #region Chopping Skill

    [FoldoutGroup("base/Player/Player Stats/Chopping Skill")]
    [PropertyTooltip("The current level of the players Chopping Skill")]
    public int choppingLevel;
    [FoldoutGroup("base/Player/Player Stats/Chopping Skill")]
    [PropertyTooltip("The current XP of the players Chopping Skill")]
    public int choppingXP;
    [FoldoutGroup("base/Player/Player Stats/Chopping Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the Chopping Skill.")]
    [DictionaryDrawerSettings(KeyLabel = "Chopping Level", ValueLabel = "Xp to Gain Level")]
    public Dictionary<int, int> choppingXPToLevel = new Dictionary<int, int>();

    #endregion

    #region Imbue Skill

    [FoldoutGroup("base/Player/Player Stats/Imbue Skill")]
    [PropertyTooltip("The current level of the players Imbue Skill")]
    public int imbueLevel;
    [FoldoutGroup("base/Player/Player Stats/Imbue Skill")]
    [PropertyTooltip("The current XP of the players Imbue Skill")]
    public int imbueXP;
    [FoldoutGroup("base/Player/Player Stats/Imbue Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the Imbue Skill.")]
    [DictionaryDrawerSettings(KeyLabel = "Imbue Level", ValueLabel = "Xp to Gain Level")]
    public Dictionary<int, int> imbueXPToLevel = new Dictionary<int, int>();

    #endregion

    #region Bunsen Burner Skill

    [FoldoutGroup("base/Player/Player Stats/Bunsen Burner Skill")]
    [PropertyTooltip("The current level of the players Bunsen Burner Skill")]
    public int bunsenBurnerLevel;
    [FoldoutGroup("base/Player/Player Stats/Bunsen Burner Skill")]
    [PropertyTooltip("The current XP of the players Bunsen Burner Skill")]
    public int bunsenBurnerXP;
    [FoldoutGroup("base/Player/Player Stats/Bunsen Burner Skill")]
    [PropertyTooltip("A dictionary of the XP required to gain each level in the Bunsen Burner Skill.")]
    [DictionaryDrawerSettings(KeyLabel = "Bunsen Burner Level", ValueLabel = "Xp to Gain Level")]
    public Dictionary<int, int> bunsenBurnerXPToLevel = new Dictionary<int, int>();

    #endregion

    #endregion

    #region Movement Variables

    [FoldoutGroup("base/Player/Movement Variables")]
    [PropertyTooltip("The speed of the player when walking.")]
    public float walkSpeed;
    [FoldoutGroup("base/Player/Movement Variables")]
    [PropertyTooltip("The speed of the player when running.")]
    public float runSpeed;
    [FoldoutGroup("base/Player/Movement Variables")]
    [PropertyTooltip("The distance from the movement order the player needs to be to make the player run.")]
    public float walkThreshold;

    #endregion

    #region Player Inventory

    [FoldoutGroup("base/Player/Player Stats/Inventory")]
    public GameObject itemInventoryImage;
    [FoldoutGroup("base/Player/Player Stats/Inventory")]
    public GameObject itemUIToolTip;
    [FoldoutGroup("base/Player/Player Stats/Inventory")]
    public Dictionary<IngredientScriptableObject, int> playerInventory = new Dictionary<IngredientScriptableObject, int>();
    [HideInInspector]
    public Dictionary<IngredientScriptableObject, GameObject> playerInventoryUI = new Dictionary<IngredientScriptableObject, GameObject>();

    #endregion

    #region Player Delegate

    [HideInInspector]
    public delegate void Interaction();
    [HideInInspector]
    public Interaction playerInteraction;

    #endregion

    #region Anim Hashes

    [HideInInspector]
    public readonly int animWalkHash = Animator.StringToHash("Walk");
    [HideInInspector]
    public readonly int animRunHash = Animator.StringToHash("Run");

    #endregion

    private void OnEnable()
    {
        playerInteraction = new Interaction(() => Debug.Log("Interaction Called."));
    }

    /// <summary>
    /// Function to improve a players skill through gaining xp
    /// </summary>
    /// <param name="skillToImprove">The skill to improve</param>
    /// <param name="XPGain">The amount of exp the player gains</param>
    public void GainSkillXp(Skills skillToImprove, int XPGain)
    {
        switch (skillToImprove)
        {
            case Skills.Alchemy:
                alchemicalXP += XPGain;
                if(alchemicalXP >= alchemicalXPToLevel[alchemicalLevel]) { alchemicalLevel++; }
                break;
            case Skills.MortarandPestle:
                mortarAndPestleXP += XPGain;
                if (mortarAndPestleXP >= mortarAndPestleXPToLevel[mortarAndPestleLevel]) { mortarAndPestleLevel++; }
                break;
            case Skills.Juicer:
                juicerXP += XPGain;
                if (juicerXP >= juicerXPToLevel[juicerLevel]) { juicerLevel++; }
                break;
            case Skills.Chopping:
                choppingXP += XPGain;
                if (choppingXP >= choppingXPToLevel[choppingLevel]) { choppingLevel++; }
                break;
            case Skills.Imbue:
                imbueXP += XPGain;
                if(imbueXP >= imbueXPToLevel[imbueLevel]) { imbueLevel++; }
                break;
            case Skills.BunsenBurner:
                bunsenBurnerXP += XPGain;
                if (bunsenBurnerXP >= bunsenBurnerXPToLevel[bunsenBurnerLevel]) { bunsenBurnerLevel++; }
                break;
        }
    }
}
