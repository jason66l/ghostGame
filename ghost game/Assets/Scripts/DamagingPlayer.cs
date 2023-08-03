using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject cube;
    private PlayerHealth player_health;
    private Collider colld;
    // Start is called before the first frame update
    void Start()
    {
        player_health = player.GetComponent<PlayerHealth>();
        colld = cube.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        onCollisionEnter(colld);
    }

    void onCollisionEnter(Collider col) 
    {
        if (col.gameObject.tag == "ghost")
        {
            player_health.health -= 1;
        }
    }
}
