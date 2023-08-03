using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GhostHealthManagement : NetworkBehaviour
{
    // Start is called before the first frame update

    public flashlight flashlight;
    public GhostHealth healthBar;
    void Start()
    {
        healthBar.SetMaxHealth(flashlight.health);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(flashlight.health);
    }
}
