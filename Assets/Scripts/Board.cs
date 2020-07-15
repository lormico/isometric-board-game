using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap boardTilemap;
    public TileProvider tileProvider;

    // Start is called before the first frame update
    void Start()
    {
        LoadBoard();
    }

    private void LoadBoard()
    {
        Level levelData = Level.FromFile(Application.dataPath + "/Levels/Example.db");
        foreach (Level.Tile tile in levelData.Tiles.Values)
        {
            boardTilemap.SetTile(
                new Vector3Int(tile.x, tile.y, 0),
                tileProvider.GetBoardTile(levelData.Pack, tile.RoomName, tile.TileName)
            );
        }
    }

}
