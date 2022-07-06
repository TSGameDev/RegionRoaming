using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewPlayerConnector", menuName = "Scriptable Object/Player Connector", order = 0)]
public class PlayerConnector : ScriptableObject
{
    #region Movement Variables

    [TabGroup("base", "Player")]
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

    #region Player Delegate

    public delegate void Interaction();
    public Interaction playerInteraction;

    #endregion

    #region Anim Hashes

    public readonly int animWalkHash = Animator.StringToHash("Walk");
    public readonly int animRunHash = Animator.StringToHash("Run");

    #endregion

    private void OnEnable()
    {
        playerInteraction = new PlayerConnector.Interaction(() => Debug.Log("Interaction Called."));
    }
}
