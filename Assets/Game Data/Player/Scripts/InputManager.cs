using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraConnector cameraConnector;
    [SerializeField] private PlayerConnector playerConnector;

    private PlayerControls playerControls;
    private Player player;
    private UIManager uiManager;

    private void OnEnable()
    {
        playerControls = new PlayerControls();
        player = GetComponent<Player>();
        uiManager = FindObjectOfType<UIManager>();

        playerControls.Enable();

        // Camera Controls
        playerControls.Game.CameraMovement.performed += ctx => cameraConnector.cameraInput = ctx.ReadValue<Vector2>();
        playerControls.Game.CameraMovement.canceled += ctx => cameraConnector.cameraInput = new Vector2();

        playerControls.Game.CameraFastToggle.performed += ctx => cameraConnector.fastCamera = !cameraConnector.fastCamera;

        playerControls.Game.CamerRotation.performed += ctx =>
        {
            float rotationValue = ctx.ReadValue<float>();
            if (rotationValue >= 0.5f)
            {
                cameraConnector.cameraRotateRight = true;
                cameraConnector.cameraRotateLeft = false;
            }
            else if (rotationValue <= -0.5f)
            {
                cameraConnector.cameraRotateLeft = true;
                cameraConnector.cameraRotateRight = false;
            }
        };
        playerControls.Game.CamerRotation.canceled += ctx => 
        {
            cameraConnector.cameraRotateLeft = false;
            cameraConnector.cameraRotateRight = false;
        };

        playerControls.Game.CameraZoom.performed += ctx =>
        {
            float zoomValue = ctx.ReadValue<float>();
            if(zoomValue > 1)
            {
                cameraConnector.cameraZoomIn = true;
                cameraConnector.cameraZoomOut = false;
            }

            else if(zoomValue < -1)
            {
                cameraConnector.cameraZoomIn = false;
                cameraConnector.cameraZoomOut = true;
            }
        };
        playerControls.Game.CameraZoom.canceled += ctx =>
        {
            cameraConnector.cameraZoomIn = false;
            cameraConnector.cameraZoomOut = false;
        };

        playerControls.Game.CameraLock.performed += ctx => cameraConnector.lockCamera = !cameraConnector.lockCamera;

        //Player Controls
        playerControls.Game.PlayerMove.performed += ctx => player.PlayerMove();

        playerControls.Game.PlayerInteraction.performed += ctx => playerConnector.playerInteraction();
        playerControls.Game.PlayerInventory.performed += ctx => uiManager.ShowInventory();
    }

    private void OnDisable()
    {
        playerControls.Disable();

        // Camera Controls
        playerControls.Game.CameraMovement.performed -= ctx => cameraConnector.cameraInput = ctx.ReadValue<Vector2>();
        playerControls.Game.CameraMovement.canceled -= ctx => cameraConnector.cameraInput = new Vector2();

        playerControls.Game.CameraFastToggle.performed -= ctx => cameraConnector.fastCamera = !cameraConnector.fastCamera;

        playerControls.Game.CamerRotation.performed -= ctx =>
        {
            float rotationValue = ctx.ReadValue<float>();
            if (rotationValue >= 0.5f)
            {
                cameraConnector.cameraRotateRight = true;
                cameraConnector.cameraRotateLeft = false;
            }
            else if (rotationValue <= -0.5f)
            {
                cameraConnector.cameraRotateLeft = true;
                cameraConnector.cameraRotateRight = false;
            }
        };
        playerControls.Game.CamerRotation.canceled -= ctx =>
        {
            cameraConnector.cameraRotateLeft = false;
            cameraConnector.cameraRotateRight = false;
        };

        playerControls.Game.CameraZoom.performed -= ctx =>
        {
            float zoomValue = ctx.ReadValue<float>();
            if (zoomValue > 1)
            {
                cameraConnector.cameraZoomIn = true;
                cameraConnector.cameraZoomOut = false;
            }

            else if (zoomValue < -1)
            {
                cameraConnector.cameraZoomIn = false;
                cameraConnector.cameraZoomOut = true;
            }
        };
        playerControls.Game.CameraZoom.canceled -= ctx =>
        {
            cameraConnector.cameraZoomIn = false;
            cameraConnector.cameraZoomOut = false;
        };

        playerControls.Game.CameraLock.performed -= ctx => cameraConnector.lockCamera = !cameraConnector.lockCamera;

        //Player Controls
        playerControls.Game.PlayerMove.performed -= ctx => player.PlayerMove();
    }
}
