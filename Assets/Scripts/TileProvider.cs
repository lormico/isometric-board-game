using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileProvider : MonoBehaviour
{
    public TextAsset tilesConfig;
    private Dictionary<string, Tile> tiles;

    private void Awake()
    {
        // Is this necessary? How many times should I expect Awake to be called?
        if (tiles == null)
        {
            LoadTiles();
        }
    }

    private void LoadTiles()
    {
        tiles = new Dictionary<string, Tile>();
        TileJsonData[] tileJsons = JsonHelper.FromJson<TileJsonData>(tilesConfig.text);
        foreach (TileJsonData tileJsonData in tileJsons)
        {
            tiles.Add(
                tileJsonData.type,
                (Tile)Resources.Load("Tiles/Board/" + tileJsonData.tile, typeof(Tile)));
        }
    }

    public Tile Get(string type)
    {
        if (tiles.ContainsKey(type))
        {
            return tiles[type];
        }
        return null;
    }

    [Serializable]
    private class TileJsonData
    {
        public string type;
        public string tile;
    }
}
