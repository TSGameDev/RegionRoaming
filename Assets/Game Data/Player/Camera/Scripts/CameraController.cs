using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CameraConnector cameraConnector;
    public Transform cameraTranform;
    public Transform playerTransform;

    private void Start()
    {
        cameraConnector.newPosition = transform.position;
        cameraConnector.newRotation = transform.rotation;
        cameraConnector.newZoom = cameraTranform.localPosition;
    }

    private void Update()
    {
        HandleMovementInput();
        HandleRotation();
        HandleZoom();
    }

    /// <summary>
    /// Function that handles the movement of the camera via the WASD keys
    /// </summary>
    void HandleMovementInput()
    {
        if(cameraConnector.lockCamera)
        {
            cameraConnector.newPosition = playerTransform.position;
        }

        //Use fast speed if the shift key is down
        if (cameraConnector.fastCamera)
            cameraConnector.movementSpeed = cameraConnector.fastSpeed;
        else
            cameraConnector.movementSpeed = cameraConnector.normalSpeed;

        //Right
        if(cameraConnector.cameraInput.x >= 0.5f)
        {
            cameraConnector.newPosition += (transform.right * cameraConnector.movementSpeed);
        }

        //Left
        else if (cameraConnector.cameraInput.x <= -0.5f)
        {
            cameraConnector.newPosition += (transform.right * -cameraConnector.movementSpeed);
        }

        //Forward
        if (cameraConnector.cameraInput.y >= 0.5f)
        {
            cameraConnector.newPosition += (transform.forward * cameraConnector.movementSpeed);
        }

        //Backward
        else if (cameraConnector.cameraInput.y <= -0.5f)
        {
            cameraConnector.newPosition += (transform.forward * -cameraConnector.movementSpeed);
        }

        transform.position = Vector3.Lerp(transform.position, cameraConnector.newPosition, Time.deltaTime * cameraConnector.movementTime);
    }

    /// <summary>
    /// Function that handles the rotation of the camera via the QE keys
    /// </summary>
    void HandleRotation()
    {
        //Rotate Clockwise
        if (cameraConnector.cameraRotateRight)
        {
            cameraConnector.newRotation *= Quaternion.Euler(Vector3.up * cameraConnector.rotationAmount);
        }
        //Rotate anti-Clockwise
        else if (cameraConnector.cameraRotateLeft)
        {
            cameraConnector.newRotation *= Quaternion.Euler(Vector3.up * -cameraConnector.rotationAmount);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraConnector.newRotation, Time.deltaTime * cameraConnector.movementTime);
    }

    /// <summary>
    /// Function that handles the zooming of the camera via the mouse scroll wheel
    /// </summary>
    void HandleZoom()
    {
        if (cameraConnector.cameraZoomIn)
        {
            cameraConnector.newZoom -= cameraConnector.zoomAmount;
        }
        else if (cameraConnector.cameraZoomOut)
        {
            cameraConnector.newZoom += cameraConnector.zoomAmount;
        }
        cameraTranform.localPosition = Vector3.Lerp(cameraTranform.localPosition, cameraConnector.newZoom, Time.deltaTime * cameraConnector.movementTime);
    }
    
}
