
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPlayer : MonoBehaviour
{
    public GameObject player;
    private PlayerHealth player_health;
    // Start is called before the first frame update
    void Start()
    {
        player_health = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col) 
    {
        if (col.CompareTag("ghost"))
        {
            Debug.Log("collision detected");
            player_health.health = player_health.health - 1;
        }
    }
}