using UnityEngine;
using Mirror;

public class pp : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float cameraSensitivity = 3.5f;
    private float cameraRotation = 0f;

    public Transform camOrientation;

    private CharacterController characterController;
    private Transform orientation;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(playerCamera.gameObject);
            return;
        }

        characterController = GetComponent<CharacterController>();
        orientation = transform.GetChild(0); // Assuming the camera is the first child of the player

        // Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        HandleMovement();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Send player position and rotation updates to the server
        CmdSyncTransform(transform.position, transform.rotation, playerCamera?.transform.localRotation ?? Quaternion.identity);
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        cameraRotation -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);
            orientation.transform.localRotation = Quaternion.Euler(0f, mouseX, 0f);
        }

        // Send camera rotation updates to the server
        CmdSyncCameraRotation(playerCamera?.transform.localRotation ?? Quaternion.identity);
    }

    [Command]
    void CmdSyncTransform(Vector3 position, Quaternion rotation, Quaternion cameraRotation)
    {
        // Update the player's position and rotation on the server
        RpcSyncTransform(position, rotation, cameraRotation);
    }

    [ClientRpc]
    void RpcSyncTransform(Vector3 position, Quaternion rotation, Quaternion cameraRotation)
    {
        // Update the player's position and rotation on the clients
        if (!isLocalPlayer)
        {
            transform.position = position;
            transform.rotation = rotation;
            if (playerCamera != null)
            {
                playerCamera.transform.localRotation = cameraRotation;
            }
        }
    }

    [Command]
    void CmdSyncCameraRotation(Quaternion cameraRotation)
    {
        
        // Update the player's camera rotation on the server
        RpcSyncCameraRotation(cameraRotation);
    }

    [ClientRpc]
    void RpcSyncCameraRotation(Quaternion cameraRotation)
    {   
        // Update the player's camera rotation on the clients
        if (!isLocalPlayer)
        {
            //Debug.Log("hauidhaiodjasda");
            //orientation.transform.localRotation = cameraRotation;
        }
    }

    public override void OnStartLocalPlayer()
    {
        if (isLocalPlayer)
        {
            // Enable player controls, camera, or any client-side logic for the local player.
            // For example, you might enable player movement scripts, camera follow, etc.

            // In this case, you can lock and hide the cursor for the local player.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update() 
    {
        transform.rotation = orientation.rotation;
    }
}

