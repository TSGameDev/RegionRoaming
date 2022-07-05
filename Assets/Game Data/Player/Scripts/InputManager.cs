using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] private CameraController cameraController;

    private void OnEnable()
    {
        playerControls = new PlayerControls();
        cameraController = FindObjectOfType<CameraController>();

        playerControls.Enable();

        // Camera Controls
        playerControls.Game.CameraMovement.performed += ctx => cameraController.cameraInput = ctx.ReadValue<Vector2>();
        playerControls.Game.CameraMovement.canceled += ctx => cameraController.cameraInput = new Vector2();
        playerControls.Game.CameraFastToggle.performed += ctx => cameraController.fastCamera = !cameraController.fastCamera;
        playerControls.Game.CamerRotation.performed += ctx => cameraController.cameraRotation = ctx.ReadValue<float>();
        playerControls.Game.CamerRotation.canceled += ctx => cameraController.cameraRotation = 0f;
        playerControls.Game.CameraZoom.performed += ctx => cameraController.cameraZoom = ctx.ReadValue<float>();
        playerControls.Game.CameraZoom.canceled += ctx => cameraController.cameraZoom = 0f;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
