using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    public EnergyBar energyBar;
    public float maxDuration = 5f; // The maximum duration the flashlight can stay on
    public float rechargeTime = 3f; // The time it takes for the flashlight to recharge

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
            if (isFlashlightOn)
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
        energyBar.SetMaxEnergy(maxDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cam.rotation;
        Debug.Log(currentDuration);
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
        /*if (Input.GetMouseButtonDown(0)) {
			lights.enabled = !lights.enabled;
        }
        transform.rotation = cam.rotation;*/
    }
}
