using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player
{
    public string id;
    public string tileName;

    private Tilemap playerTilemap;
    private Tile playerTile;
    private PathFinder pathFinder;

    public Vector3Int Position { get; private set; }

    public List<Vector3Int> ReachableCells { get; private set; }

    public Player(Tilemap playerTilemap, Tile playerTile, Vector3Int startPosition)
    {
        this.playerTilemap = playerTilemap;
        this.playerTile = playerTile;
        Position = startPosition;
        pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
        playerTilemap.SetTile(Position, playerTile);
    }

    public void MoveTo(Vector3Int cell)
    {
        playerTilemap.SetTile(Position, null);
        Position = cell;
        playerTilemap.SetTile(Position, playerTile);
    }

    public void SetWalkableDistance(int distance)
    {
        ReachableCells = pathFinder.GetReachableCells(Position, distance);
    }
}
