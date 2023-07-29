using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    float timer = 0;
    Light lights;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        lights = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && lights.enabled == false &&  timer >= 1) {
			lights.enabled = !lights.enabled;
        }
        transform.rotation = cam.rotation;

        if (timer < 10 && lights.enabled == false) {
            timer += Time.deltaTime;
        }
        else if (timer > 0 && lights.enabled == true) {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0) {
            lights.enabled = false;
            timer = 0;
            timer += Time.deltaTime;
        }
    } 
}
