using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePack
{
    private readonly IDictionary<string, Type> tileTypes;

    public TilePack(string name)
    {
        tileTypes = new Dictionary<string, Type>();
        string packPath = string.Join(Path.DirectorySeparatorChar.ToString(), Application.dataPath, "Resources", "Packs", name);
        string tilesPath = packPath + Path.DirectorySeparatorChar + "Tiles";
        string boardPath = tilesPath + Path.DirectorySeparatorChar + "Board";
        string playersPath = tilesPath + Path.DirectorySeparatorChar + "Players";
        foreach (string dirPath in Directory.GetDirectories(boardPath))
        {
            Type type = new Type(Directory.GetFiles(dirPath));
            tileTypes.Add(Path.GetFileName(dirPath), type);
        }
    }

    public Tile GetTile(string type, string name)
    {
        if (tileTypes.ContainsKey(type))
        {
            return tileTypes[type].GetTile(name);
        }
        return null;
    }

    private class Type
    {
        private readonly IDictionary<string, Tile> tiles;

        public Type(IList<string> paths)
        {
            tiles = new Dictionary<string, Tile>();
            foreach (string path in paths)
            {
                string tileName = Path.GetFileNameWithoutExtension(path);
                string resourcePath = (new FileInfo(path).Directory.FullName + Path.DirectorySeparatorChar + tileName)
                    .Remove(0, (Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar).Length)
                    .Replace('\\', '/');

                tiles.Add(tileName, Resources.Load<Tile>(resourcePath));
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
}
