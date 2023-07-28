using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags]
public enum Wall_State {
    LEFT = 1,
    RIGHT = 2, 
    UP = 4, 
    DOWN = 8, 
    VISITED = 128,  //1000 0000
}

public struct Position {
    public int X;
    public int Y;
}
public struct Neighbor {
    public Position Position;
    public Wall_State SharedWall;
}

public static class Maze_Generator
{
    //recursive backtracker
    private static Wall_State GetOppositeWall(Wall_State wall) {
        switch(wall) {
            case Wall_State.RIGHT: return Wall_State.LEFT;
            case Wall_State.LEFT: return Wall_State.RIGHT;
            case Wall_State.UP: return Wall_State.DOWN;
            case Wall_State.DOWN: return Wall_State.UP;
            default: return Wall_State.LEFT;
        }
    }
    private static Wall_State[,] ApplyRecursiveBacktracker (Wall_State[,] maze, int width, int height) {
        var rng = new System.Random();
        var positionStack = new Stack<Position> ();
        var position = new Position { X = rng.Next(0, width), Y = rng.Next(0, height)};
        maze[position.X, position.Y] |= Wall_State.VISITED;
        positionStack.Push(position);

        while (positionStack.Count > 0) {
            var current  = positionStack.Pop();
            var neighbors = GetUnvisitedNeighbors(current, maze,width, height);
            if (neighbors.Count > 0) {
                positionStack.Push(current);
                var randIndex = rng.Next(0, neighbors.Count);
                var randomNeighbor = neighbors[randIndex];
                var nPosition = randomNeighbor.Position;
                maze[current.X, current.Y] &=  ~randomNeighbor.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbor.SharedWall);
                maze[nPosition.X, nPosition.Y] |= Wall_State.VISITED;
                positionStack.Push(nPosition);
            }
            //positionStack.pop();
            //getUnvisitedNeightbors(position, width, height);
        }
        return maze;
    }

    private static List<Neighbor> GetUnvisitedNeighbors(Position p, Wall_State[,] maze, int width, int height) {
        var list = new List <Neighbor>();
        if (p.X > 0) //left 
        {
            if (!maze[p.X-1,p.Y].HasFlag(Wall_State.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X-1,
                        Y = p.Y
                    },
                    SharedWall = Wall_State.LEFT
                });
            }
        }

        if (p.Y > 0) //down 
        {
            if (!maze[p.X,p.Y-1].HasFlag(Wall_State.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X,
                        Y = p.Y-1
                    },
                    SharedWall = Wall_State.DOWN
                });
            }
        }

        if (p.Y < height - 1) //UP 
        {
            if (!maze[p.X,p.Y+1].HasFlag(Wall_State.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X,
                        Y = p.Y+1
                    },
                    SharedWall = Wall_State.UP
                });
            }
        }

        if (p.X < width - 1) //RIGHT 
        {
            if (!maze[p.X+1,p.Y].HasFlag(Wall_State.VISITED)) {
                list.Add(new Neighbor {
                    Position = new Position {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = Wall_State.RIGHT
                });
            }
        }

        return list;
    }

    public static Wall_State[,] Generate(int width, int height) {

        Wall_State[,] maze = new Wall_State[width,height];
        Wall_State initial = Wall_State.RIGHT | Wall_State.LEFT | Wall_State.UP | Wall_State.DOWN;
        for (int i = 0; i < width; ++i) {
            for (int j = 0; j < height; ++j) {
                maze[i,j] = initial; //1111
                //maze[i,j].HasFlag(WallState.RIGHT)
            }
        }
        //return maze;
        return ApplyRecursiveBacktracker(maze, width, height);
    }
    
    
    // Start is called before the first frame update
    /*void Start()
    {
        /*WallState wallState = WallState.LEFT | WallState.RIGHT;
        wallState |= WallState.UP;
        wallState &= WallState.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
