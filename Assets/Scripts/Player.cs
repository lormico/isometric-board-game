using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Player
{
    public string id;
    public string tileName;

    public Tilemap playerTilemap;
    public Tile playerTile;
    private Vector3Int position;

    public Player(Tilemap playerTilemap, Tile playerTile, Vector3Int startPosition)
    {
        this.playerTilemap = playerTilemap;
        this.playerTile = playerTile;
        position = startPosition;
        playerTilemap.SetTile(position, playerTile);
    }

    public Vector3Int GetPosition()
    {
        return position;
    }

    public void MoveTo(Vector3Int cell)
    {
        playerTilemap.SetTile(position, null);
        position = cell;
        playerTilemap.SetTile(position, playerTile);
    }

}
