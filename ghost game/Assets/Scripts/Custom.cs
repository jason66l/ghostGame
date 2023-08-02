using Mirror;
using UnityEngine;

public class Custom : NetworkManager
{
    public GameObject[] playerPrefabs; // An array to hold different player prefabs
    private int playerCount = 0; // Keep track of the number of players that have joined

    void Start()
    {
        Debug.Log("custom started");
    }

    
    
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Check if the connection already has a player associated with it
        if (conn.identity != null)
        {
            Debug.LogWarning("Player already exists for connection: " + conn);
            return;
        }

        // Determine which player prefab to spawn based on specific conditions
        GameObject playerPrefabToSpawn = SelectPlayerPrefab();

        // Spawn the selected player prefab on the server
        GameObject player = Instantiate(playerPrefabToSpawn);
        NetworkServer.AddPlayerForConnection(conn, player);

        // Increment the player count
        playerCount++;

        Debug.Log("Player spawned for connection: " + conn);
    }

    

    private GameObject SelectPlayerPrefab()
    {
        // Implement your logic here to choose the appropriate player prefab
        // For this example, we'll use the playerCount to index into the playerPrefabs array.
        int index = playerCount % playerPrefabs.Length;
        Debug.Log("instantiating prefab index: " + index);

        return playerPrefabs[index];
    }



}