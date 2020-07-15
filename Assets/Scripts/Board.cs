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
        foreach (Level.Tile tile in level.Tiles)
        {
            boardTilemap.SetTile(
                (Vector3Int)tile.Position,
                tileProvider.GetBoardTile(level.Pack, tile.RoomName, tile.TileName)
            );
        }
    }

}
