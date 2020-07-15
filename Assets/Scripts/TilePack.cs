using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePack
{
    private readonly IDictionary<string, Room> boardRooms;
    private readonly IDictionary<string, Tile> playerTiles;
    private readonly IDictionary<string, Tile> overlayTiles;

    public TilePack(string folder)
    {
        boardRooms = new Dictionary<string, Room>();
        playerTiles = new Dictionary<string, Tile>();
        overlayTiles = new Dictionary<string, Tile>();
        string packPath = string.Join(Path.DirectorySeparatorChar.ToString(), Application.dataPath, "Resources", "Packs", folder);
        string tilesPath = packPath + Path.DirectorySeparatorChar + "Tiles";
        string boardPath = tilesPath + Path.DirectorySeparatorChar + "Board";
        string playersPath = tilesPath + Path.DirectorySeparatorChar + "Players";
        string overlayPath = tilesPath + Path.DirectorySeparatorChar + "Overlay";

        foreach (string dirPath in Directory.GetDirectories(boardPath))
        {
            Room room = new Room(Directory.GetFiles(dirPath));
            boardRooms.Add(Path.GetFileName(dirPath), room);
        }

        foreach (string path in Directory.GetFiles(playersPath))
        {
            playerTiles.Add(Path.GetFileNameWithoutExtension(path), Resources.Load<Tile>(GetResourcePath(path)));
        }

        foreach (string path in Directory.GetFiles(overlayPath))
        {
            overlayTiles.Add(Path.GetFileNameWithoutExtension(path), Resources.Load<Tile>(GetResourcePath(path)));
        }
    }

    public Tile GetBoardTile(string room, string name)
    {
        if (boardRooms.ContainsKey(room))
        {
            return boardRooms[room].GetTile(name);
        }
        return null;
    }

    public Tile GetPlayerTile(string name)
    {
        if (playerTiles.ContainsKey(name))
        {
            return playerTiles[name];
        }
        return null;
    }

    public Tile GetOverlayTile(string name)
    {
        if (overlayTiles.ContainsKey(name))
        {
            return overlayTiles[name];
        }
        return null;
    }

    private class Room
    {
        private readonly IDictionary<string, Tile> tiles;

        public Room(IList<string> paths)
        {
            tiles = new Dictionary<string, Tile>();
            foreach (string path in paths)
            {
                tiles.Add(Path.GetFileNameWithoutExtension(path), Resources.Load<Tile>(GetResourcePath(path)));
            }
        }

        public Tile GetTile(string name)
        {
            if (tiles.ContainsKey(name))
            {
                return tiles[name];
            }
            return null;
        }
    }

    private static string GetResourcePath(string absolutePath)
    {
        string tileName = Path.GetFileNameWithoutExtension(absolutePath);
        return (new FileInfo(absolutePath).Directory.FullName + Path.DirectorySeparatorChar + tileName)
            .Remove(0, (Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar).Length)
            .Replace('\\', '/');
    }
}
