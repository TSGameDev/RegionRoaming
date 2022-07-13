using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New UI Manager Connector", menuName = "Scriptable Object/UI Manager Connector", order = 3)]
public class UIManagerConnector : SerializedScriptableObject
{
    public UnityEvent<ItemScriptableObject> onItemHover;
    public UnityEvent onItemHoverExit;

    private void OnEnable()
    {
        onItemHover = new UnityEvent<ItemScriptableObject>();
        onItemHoverExit = new UnityEvent();
    }
}
