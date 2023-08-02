using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Wall_State
{
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8,
    VISITED = 128 // 1000 0000
}

public struct Position
{
    public int X;
    public int Y;
}

public struct Neighbor
{
    public Position Position;
    public Wall_State SharedWall;
}

public static class Maze_Generator
{
    // Recursive backtracker
    private static Wall_State GetOppositeWall(Wall_State wall)
    {
        switch (wall)
        {
            case Wall_State.RIGHT: return Wall_State.LEFT;
            case Wall_State.LEFT: return Wall_State.RIGHT;
            case Wall_State.UP: return Wall_State.DOWN;
            case Wall_State.DOWN: return Wall_State.UP;
            default: return Wall_State.LEFT;
        }
    }

    private static Wall_State[] ApplyRecursiveBacktracker(Wall_State[] maze, int width, int height)
    {
        var rng = new System.Random();
        var positionStack = new Stack<Position>();
        var position = new Position { X = rng.Next(0, width), Y = rng.Next(0, height) };
       maze[GetIndex(position.X, position.Y, width)] |= Wall_State.VISITED;

        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Peek();
            var neighbors = GetUnvisitedNeighbors(current, maze, width, height);

            if (neighbors.Count > 0)
            {
                var randomNeighbor = neighbors[rng.Next(0, neighbors.Count)];
                var nPosition = randomNeighbor.Position;

                // Remove the wall between the current cell and the selected neighbor
                maze[GetIndex(current.X, current.Y, width)] &= ~randomNeighbor.SharedWall;
                maze[GetIndex(nPosition.X, nPosition.Y, width)] &= ~GetOppositeWall(randomNeighbor.SharedWall);

                // Mark the neighbor as visited and add it to the stack
                maze[GetIndex(nPosition.X, nPosition.Y, width)] |= Wall_State.VISITED;
                positionStack.Push(nPosition);
            }
            else
            {
                // Backtrack to the previous cell
                positionStack.Pop();
            }
        }

        return maze;
    }

    private static List<Neighbor> GetUnvisitedNeighbors(Position p, Wall_State[] maze, int width, int height)
    {
        var list = new List<Neighbor>();
        if (p.X > 0) // Left
        {
            if (!maze[GetIndex(p.X - 1, p.Y, width)].HasFlag(Wall_State.VISITED))
            {
                list.Add(new Neighbor
                {
                    Position = new Position { X = p.X - 1, Y = p.Y },
                    SharedWall = Wall_State.LEFT
                });
            }
        }

        if (p.Y > 0) // Down
        {
            if (!maze[GetIndex(p.X, p.Y - 1, width)].HasFlag(Wall_State.VISITED))
            {
                list.Add(new Neighbor
                {
                    Position = new Position { X = p.X, Y = p.Y - 1 },
                    SharedWall = Wall_State.DOWN
                });
            }
        }

        if (p.Y < height - 1) // Up
        {
            if (!maze[GetIndex(p.X, p.Y + 1, width)].HasFlag(Wall_State.VISITED))
            {
                list.Add(new Neighbor
                {
                    Position = new Position { X = p.X, Y = p.Y + 1 },
                    SharedWall = Wall_State.UP
                });
            }
        }

        if (p.X < width - 1) // Right
        {
            if (!maze[GetIndex(p.X + 1, p.Y, width)].HasFlag(Wall_State.VISITED))
            {
                list.Add(new Neighbor
                {
                    Position = new Position { X = p.X + 1, Y = p.Y },
                    SharedWall = Wall_State.RIGHT
                });
            }
        }

        return list;
    }

    private static int GetIndex(int x, int y, int width)
    {
        return y * width + x;
    }

    // Update the method signature to use Wall_State[] instead of Wall_State[,]
    public static Wall_State[] Generate(int width, int height)
    {
        Wall_State[] maze = new Wall_State[width * height];
        Wall_State initial = Wall_State.RIGHT | Wall_State.LEFT | Wall_State.UP | Wall_State.DOWN;

        for (int i = 0; i < maze.Length; i++)
        {
            maze[i] = initial; // 1111
        }

        return ApplyRecursiveBacktracker(maze, width, height);
    }
}
