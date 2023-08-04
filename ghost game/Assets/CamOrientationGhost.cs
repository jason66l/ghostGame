using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GhostCamOrientation : NetworkBehaviour
{
    public float sensX;
    public float sensY;
    [SyncVar]
    float xRotation;
    [SyncVar]
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        if (!isServer) 
        {
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * -1 *  sensY;

                yRotation += mouseX;
                
                xRotation += mouseY; 
                xRotation = Mathf.Clamp(xRotation, -15f, 15f);  

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
}