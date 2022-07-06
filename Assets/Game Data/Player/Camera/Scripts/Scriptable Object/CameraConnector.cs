using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewCameraConnector", menuName = "Scriptable Object/Camera Connector", order = 1)]
public class CameraConnector : ScriptableObject
{
    #region Camera Movement Variables
    [TabGroup("base", "Camera Movement")]

    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [MinValue(0), MaxValue(10)]
    [PropertyTooltip("The normal speed of the camera, when shift isn't pressed")]
    public float normalSpeed;
    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [MinValue("@normalSpeed"), MaxValue(20)]
    [PropertyTooltip("The fast speed of the camera, when shift is pressed")]
    public float fastSpeed;
    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [PropertyTooltip("The current speed of the camera")]
    [ReadOnly]
    public float movementSpeed;
    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [PropertyTooltip("The lerp time for the movement. smaller values will make the movement more snappy, longer times more fluid")]
    public float movementTime;
    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [PropertyTooltip("The amount to rotate the camera per tick of the rotate function. Smaller values makes a slower rotation, higher values make a faster rotation.")]
    public float rotationAmount;
    [FoldoutGroup("base/Camera Movement/Camera Stats")]
    [PropertyTooltip("The amount to zoom the camera per tick of the zoom function. Smaller values makes a slower zoom, hgher vallues makes a faster zoom.")]
    public Vector3 zoomAmount;

    [HideInInspector]
    public Vector3 newPosition;
    [HideInInspector]
    public Quaternion newRotation;
    [HideInInspector]
    public Vector3 newZoom;

    #endregion

    #region Camera Inputs/States
    [HideInInspector]
    public Vector2 cameraInput;

    [TabGroup("base", "Camera Stats")]

    [HorizontalGroup("base/Camera Stats/Hoz1",LabelWidth = 75)]
    [LabelText("Fast Cam")]
    [PropertyTooltip("Bool for is the camera is to use fast speed aka shift is pressed.")]
    public bool fastCamera;
    [HorizontalGroup("base/Camera Stats/Hoz1", LabelWidth = 75)]
    [LabelText("Rotate Left")]
    [PropertyTooltip("Bool for if the camera should rotate left.")]
    public bool cameraRotateLeft;
    [HorizontalGroup("base/Camera Stats/Hoz1", LabelWidth = 75)]
    [LabelText("Rotate Right")]
    [PropertyTooltip("Bool for if the camera should rotate right.")]
    public bool cameraRotateRight;
    [HorizontalGroup("base/Camera Stats/Hoz2", LabelWidth = 75)]
    [LabelText("Zoom In")]
    [PropertyTooltip("Bool for if the camera should zoom in.")]
    public bool cameraZoomIn;
    [HorizontalGroup("base/Camera Stats/Hoz2", LabelWidth = 75)]
    [LabelText("Zoom Out")]
    [PropertyTooltip("Bool for if the camera should zoom out.")]
    public bool cameraZoomOut;
    [HorizontalGroup("base/Camera Stats/Hoz2", LabelWidth = 75)]
    [LabelText("Lock On")]
    [PropertyTooltip("Bool for if the camera should lock on/follow the player transform.")]
    public bool lockCamera;

    #endregion
}
