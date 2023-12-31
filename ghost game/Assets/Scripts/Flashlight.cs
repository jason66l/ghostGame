using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class flashlight : NetworkBehaviour
{
    //booleans for ghost
    Vector3 angleUp;
    Vector3 angleSide;
    public float health;
    public LayerMask ghost;
    bool ghosted;
    bool ghosted0;
    bool ghosted1;
    bool ghosted2;
    bool ghosted3;
    bool ghosted4;
    bool ghosted5;
    bool ghosted6;
    bool ghosted7;
    //public GameObject player1;
    public EnergyBar energyBar;
    public float maxDuration = 5f; // The maximum duration the flashlight can stay on
    public float rechargeTime = 3f; // The time it takes for the flashlight to recharge
    
    [SyncVar(hook = nameof(OnFlashlightStateChange))]
    private bool isFlashlightOn = true;
    private float currentDuration = 5f;

    public Light flashlightLight;
    public Transform cam;
    private bool cocurrent = false;

    private IEnumerator RechargeFlashlight()
    {
        cocurrent = true;
        isFlashlightOn = false;
        flashlightLight.enabled = isFlashlightOn;
        yield return new WaitForSeconds(rechargeTime);
        currentDuration = maxDuration;
        energyBar.SetEnergy(currentDuration);
        isFlashlightOn = !isFlashlightOn;
        flashlightLight.enabled = isFlashlightOn;
        cocurrent = false;
    }
    public void ToggleFlashlight()
    {
        if (currentDuration > 0f)
        {
            isFlashlightOn = !isFlashlightOn;
            if (isFlashlightOn) //possible exploit = pressing f when energy bar fully done autorecharge
            {
                flashlightLight.enabled = isFlashlightOn;
                //lights = GetComponent<Light>();
                // Turn on the flashlight
                // You can activate the light component and any other visual effects here
            }
            else {
                flashlightLight.enabled = isFlashlightOn;
            }
            /*else
            {
                lights.enabled = !lights.enabled;
                // Turn off the flashlight
                // Deactivate the light component and any other visual effects here
            }*/
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        energyBar.SetMaxEnergy(maxDuration);
        angleUp = new Vector3(0, 0.25F, 0);
        angleSide = new Vector3(0.25F, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer) {
            if (isFlashlightOn)
            {
                currentDuration -= Time.deltaTime;
                energyBar.SetEnergy(currentDuration);
                if (currentDuration <= 0f)
                {
                    // The flashlight has run out of power, initiate recharge
                    StartCoroutine(RechargeFlashlight());
                }
            }
            else {
                if (currentDuration < maxDuration) {
                    currentDuration += Time.deltaTime;
                    if (!cocurrent)
                        energyBar.SetEnergy(currentDuration);
                }
            }
            // Toggle flashlight on/off when the player presses the designated key (e.g., F key)
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleFlashlight();
            }

            // Rotate the flashlight to match the camera rotation (if needed)
            transform.rotation = cam.rotation;
            
            if (flashlightLight.enabled) {
                ghosted4 = Physics.Raycast(transform.position, transform.forward + angleUp, flashlightLight.range, ghost);
                ghosted5 = Physics.Raycast(transform.position, transform.forward + angleSide, flashlightLight.range, ghost);
                ghosted6 = Physics.Raycast(transform.position, transform.forward - angleUp, flashlightLight.range, ghost);
                ghosted7 = Physics.Raycast(transform.position, transform.forward - angleSide, flashlightLight.range, ghost);

                for (float i = 0; i <= 78.5F; i = i + 0.1F)
                {
                    for (float j = 0.1F; j <= 1; j = j + 0.1F)
                    {
                        ghosted = Physics.Raycast(transform.position, transform.forward + j * angleUp * Mathf.Sin(i/100) + angleSide * Mathf.Cos(i/100), flashlightLight.range + 0.05F, ghost);
                        ghosted0 = Physics.Raycast(transform.position, transform.forward + j * angleUp * -Mathf.Sin(i/100) + angleSide * Mathf.Cos(i/100), flashlightLight.range + 0.05F, ghost);
                        ghosted1 = Physics.Raycast(transform.position, transform.forward + j * angleUp * Mathf.Sin(i/100) + angleSide * -Mathf.Cos(i/100), flashlightLight.range + 0.05F, ghost);
                        ghosted2 = Physics.Raycast(transform.position, transform.forward + j * angleUp * -Mathf.Sin(i/100) + angleSide * -Mathf.Cos(i/100), flashlightLight.range + 0.05F, ghost);
                        ghosted3 = Physics.Raycast(transform.position, transform.forward, flashlightLight.range, ghost);

                        if (ghosted || ghosted0 || ghosted1 || ghosted2 || ghosted3 || ghosted4 || ghosted5 || ghosted6 || ghosted7)
                        {
                            health -= 0.001f;
                            ghosted = false; 
                        }
                        break;
                    }
                }
            }
        }
    }
     private void OnFlashlightStateChange(bool oldValue, bool newValue)
    {
        flashlightLight.enabled = newValue;
    }
}