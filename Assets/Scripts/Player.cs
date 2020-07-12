using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Tile playerTile;
    public Tilemap playerTilemap;
    private Vector3Int position;

    void Start()
    {
        position = new Vector3Int(0, 0, 0);
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
