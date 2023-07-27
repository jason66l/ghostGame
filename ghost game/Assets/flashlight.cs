using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{

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
        if (Input.GetMouseButtonDown(0)) {
			lights.enabled = !lights.enabled;
        }
        transform.rotation = cam.rotation;
    }
}
