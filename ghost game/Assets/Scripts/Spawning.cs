using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Spawning : NetworkBehaviour
{
    public GameObject cubePrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer) {
            float [] arr_x = {-5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f};
            float [] arr_z = {-5f, -4f, -3f, -2f, -1f, 0f, 1f, 2f, 3f, 4f};
            for (int i = 0; i < 5; i++) {
                int randomSpawnPositionx = Random.Range(0, 9);
                int randomSpawnPositionz = Random.Range(0, 9);
                    Vector3 randomSpawnPosition = new Vector3(arr_x[randomSpawnPositionx], -0.30f, arr_z[randomSpawnPositionz]);
                    Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}