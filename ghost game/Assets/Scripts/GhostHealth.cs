using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GhostHealth : NetworkBehaviour
{

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxHealth(float health)
    {
        if(!isServer) {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    public void SetHealth(float health)
    {
        if(!isServer) {
            slider.value = health;
        }
    }
}
