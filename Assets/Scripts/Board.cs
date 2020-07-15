using UnityEngine;
using UnityEngine.Tilemaps;

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

    private void LoadBoard()
    {
        Level levelData = Level.FromFile(Application.dataPath + "/Levels/Example.db");
        foreach (Level.Tile tile in levelData.Tiles)
        {
            boardTilemap.SetTile(
                new Vector3Int(tile.x, tile.y, 0),
                tileProvider.Get(levelData.Pack, tile.RoomName, tile.TileName)
            );
        }
    }

    public class Room
    {

    }
}
