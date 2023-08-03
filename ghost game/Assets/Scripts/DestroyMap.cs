using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MiniMap : NetworkBehaviour
{
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer) 
        {
            Destroy(canvas);
        }
    }
}