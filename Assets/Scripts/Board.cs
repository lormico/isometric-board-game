using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap boardTilemap;
    public TileProvider tileProvider;
    public Level level;

    // Start is called before the first frame update
    void Start()
    {
        LoadBoard();
    }

    private void LoadBoard()
    {
        foreach (Level.Tile tile in level.Tiles.Values)
        {
            boardTilemap.SetTile(
                new Vector3Int(tile.x, tile.y, 0),
                tileProvider.GetBoardTile(level.Pack, tile.RoomName, tile.TileName)
            );
        }
    }

}
