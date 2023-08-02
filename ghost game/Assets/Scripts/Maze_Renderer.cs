using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Maze_Renderer : NetworkBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private int width = 10;
    [SerializeField]
    [Range(1, 50)]
    private int height = 10;
    [SerializeField]
    private float size = 1f;
    [SerializeField]
    private Transform wallPrefab = null;

    private bool isMazeGenerated = false;

    void Start()
    {
        Debug.Log("its working lmfao");

        if (isServer)
        {
            GenerateAndDrawMaze();
        }
    }

    // Call this method to generate and draw the maze on the server
    [Server]
    public void GenerateAndDrawMaze()
    {
        if (isMazeGenerated)
        {
            // Don't generate the maze again if it's already generated
            return;
        }
        
        Wall_State[] mazeData = Maze_Generator.Generate(width, height);

        // Draw the maze on the server (host) side
        DrawMazeOnServer(mazeData);

        // Notify the clients to draw the maze
        RpcDrawMazeOnClients(mazeData);

        // Set the flag to true indicating that the maze is generated
        isMazeGenerated = true;
    }

    // Method to draw the maze on the server (host) side
    private void DrawMazeOnServer(Wall_State[] mazeData)
    {
        for (int i = 0; i < mazeData.Length; i++)
        {
            var cell = mazeData[i];
            var x = i % width;
            var y = i / width;

            var position = new Vector3(-width / 2 + x, 0, -height / 2 + y);

            if (cell.HasFlag(Wall_State.UP))
            {
                var topWall = Instantiate(wallPrefab, position + new Vector3(0, 0, size / 2), Quaternion.identity);
                NetworkServer.Spawn(topWall.gameObject);
            }
            if (cell.HasFlag(Wall_State.LEFT))
            {
                var leftWall = Instantiate(wallPrefab, position + new Vector3(-size / 2, 0, 0), Quaternion.Euler(0, 90, 0));
                NetworkServer.Spawn(leftWall.gameObject);
            }
            if (x == width - 1 && cell.HasFlag(Wall_State.RIGHT))
            {
                var rightWall = Instantiate(wallPrefab, position + new Vector3(size / 2, 0, 0), Quaternion.Euler(0, 90, 0));
                NetworkServer.Spawn(rightWall.gameObject);
            }
            if (y == 0 && cell.HasFlag(Wall_State.DOWN))
            {
                var bottomWall = Instantiate(wallPrefab, position + new Vector3(0, 0, -size / 2), Quaternion.identity);
                NetworkServer.Spawn(bottomWall.gameObject);
            }
        }
    }

    // This method will be called on all clients to draw the maze
    [ClientRpc]
    private void RpcDrawMazeOnClients(Wall_State[] mazeData)
    {
        if (isServer)
        {
            // Don't execute this method on the server
            return;
        }

        for (int i = 0; i < mazeData.Length; i++)
        {
            var cell = mazeData[i];
            var x = i % width;
            var y = i / width;

            var position = new Vector3(-width / 2 + x, 0, -height / 2 + y);

            if (cell.HasFlag(Wall_State.UP))
            {
                var topWall = Instantiate(wallPrefab, position + new Vector3(0, 0, size / 2), Quaternion.identity);
            }
            if (cell.HasFlag(Wall_State.LEFT))
            {
                var leftWall = Instantiate(wallPrefab, position + new Vector3(-size / 2, 0, 0), Quaternion.Euler(0, 90, 0));
            }
            if (x == width - 1 && cell.HasFlag(Wall_State.RIGHT))
            {
                var rightWall = Instantiate(wallPrefab, position + new Vector3(size / 2, 0, 0), Quaternion.Euler(0, 90, 0));
            }
            if (y == 0 && cell.HasFlag(Wall_State.DOWN))
            {
                var bottomWall = Instantiate(wallPrefab, position + new Vector3(0, 0, -size / 2), Quaternion.identity);
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("something joined");

        // Generate and draw the maze only when a player (host or client) joins the server
        GenerateAndDrawMaze();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Clients should not call GenerateAndDrawMaze here
        // The maze data will be received through RpcDrawMazeOnClients
    }

    private int GetIndex(int x, int y, int width)
    {
        return y * width + x;
    }
}
