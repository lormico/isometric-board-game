using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements.Experimental;

public class Board : MonoBehaviour
{
    public Tilemap boardTilemap;
    public Tilemap overlayTilemap;
    public TextAsset level;
    public TileProvider tileProvider;

    // Start is called before the first frame update
    void Start()
    {
        LoadBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadBoard()
    {
        foreach(string row in level.text.Split('\n'))
        {
            string[] values = row.Split(',');
            boardTilemap.SetTile(
                new Vector3Int(int.Parse(values[0]), int.Parse(values[1]), 0),
                tileProvider.Get(values[2])
                );
        }
    }
}
