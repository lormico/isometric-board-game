using UnityEngine.Tilemaps;
using UnityEngine;

namespace Game
{
    public class Tiles
    {
        public static readonly Tile PEACH = (Tile)Resources.Load("Tiles/Board/peach", typeof(Tile));
        public static readonly Tile RED = (Tile)Resources.Load("Tiles/Board/red", typeof(Tile));
        public static readonly Tile GREEN = (Tile)Resources.Load("Tiles/Board/green", typeof(Tile));
        public static readonly Tile YELLOW = (Tile)Resources.Load("Tiles/Board/yellow", typeof(Tile));
        public static readonly Tile CERULEAN = (Tile)Resources.Load("Tiles/Board/cerulean", typeof(Tile));
    }
}
