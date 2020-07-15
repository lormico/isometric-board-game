using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Settings settings;
    public string Name { get; private set; }
    public string Pack { get; private set; }
    public IList<Room> Rooms { get; private set; }
    public IList<Tile> Tiles { get; private set; }
    public IList<Connection> Obstacles { get; private set; }
    public IList<Character> Characters { get; private set; }
    public IList<Weapon> Weapons { get; private set; }
    public IList<Connection> Shortcuts { get; private set; }

    private IDictionary<int, int> TileIdIndex;
    private IDictionary<Vector2Int, int> TilePositionIndex;

    private void Start()
    {
        string connection = "URI=file:" + Application.dataPath + "/Levels/" + settings.Level;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDictionary<string, string> levelMetadata = GetLevelMetadata(dbcon);
        Pack = levelMetadata["pack"];
        Name = levelMetadata["level_name"];

        Rooms = GetRooms(dbcon);
        Tiles = GetTiles(dbcon);
        Obstacles = GetObstacles(dbcon);
        Characters = GetCharacters(dbcon);
        Weapons = GetWeapons(dbcon);
        Shortcuts = GetShortcuts(dbcon);

        dbcon.Close();

        CreateTileIndices();
    }

    private static IDictionary<string, string> GetLevelMetadata(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT key, value FROM meta";
        reader = cmnd_read.ExecuteReader();
        IDictionary<string, string> levelMetadata = new Dictionary<string, string>();
        while (reader.Read())
        {
            levelMetadata.Add(reader[0].ToString(), reader[1].ToString());
        }

        return levelMetadata;
    }

    private static IList<Room> GetRooms(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT name FROM rooms";
        reader = cmnd_read.ExecuteReader();
        IList<Room> rooms = new List<Room>();
        while (reader.Read())
        {
            rooms.Add(new Room(reader.GetString(reader.GetOrdinal("name"))));
        }

        return rooms;
    }

    private static IList<Tile> GetTiles(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT tile_id, x, y, room, tile FROM board_tiles";
        reader = cmnd_read.ExecuteReader();
        IList<Tile> tiles = new List<Tile>();
        while (reader.Read())
        {
            tiles.Add(new Tile(
                reader.GetInt32(reader.GetOrdinal("tile_id")),
                reader.GetInt32(reader.GetOrdinal("x")),
                reader.GetInt32(reader.GetOrdinal("y")),
                reader.GetString(reader.GetOrdinal("room")),
                reader.GetString(reader.GetOrdinal("tile"))
                ));
        }

        return tiles;
    }

    private static IList<Connection> GetObstacles(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT tile_id1, tile_id2 FROM obstacles";
        reader = cmnd_read.ExecuteReader();
        IList<Connection> obstacles = new List<Connection>();
        while (reader.Read())
        {
            obstacles.Add(new Connection(
                reader.GetInt32(reader.GetOrdinal("tile_id1")),
                reader.GetInt32(reader.GetOrdinal("tile_id2"))
            ));
        }

        return obstacles;
    }

    private static IList<Character> GetCharacters(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT id, name, tile, start_tile_id FROM characters";
        reader = cmnd_read.ExecuteReader();
        IList<Character> characters = new List<Character>();
        while (reader.Read())
        {
            characters.Add(new Character(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("name")),
                reader.GetString(reader.GetOrdinal("tile")),
                reader.GetInt32(reader.GetOrdinal("start_tile_id"))
            ));
        }

        return characters;
    }

    private static IList<Weapon> GetWeapons(IDbConnection dbcon)
    {
        IList<Weapon> weapons = new List<Weapon>();

        return weapons;
    }

    private static IList<Connection> GetShortcuts(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT room1, room2 FROM shortcuts";
        reader = cmnd_read.ExecuteReader();
        IList<Connection> shortcuts = new List<Connection>();
        while (reader.Read())
        {
            shortcuts.Add(new Connection(
                reader.GetInt32(reader.GetOrdinal("room1")),
                reader.GetInt32(reader.GetOrdinal("room2"))
            ));
        }

        return shortcuts;
    }

    private void CreateTileIndices()
    {
        TileIdIndex = new Dictionary<int, int>();
        TilePositionIndex = new Dictionary<Vector2Int, int>();
        foreach (Tile tile in Tiles)
        {
            TileIdIndex.Add(tile.Id, Tiles.IndexOf(tile));
            TilePositionIndex.Add(tile.Position, Tiles.IndexOf(tile));
        }
    }

    public Tile GetTile(int id)
    {
        if (!TileIdIndex.ContainsKey(id))
        {
            return null;
        }
        return Tiles[TileIdIndex[id]];
    }

    public Tile GetTile(Vector2Int position)
    {
        if (!TilePositionIndex.ContainsKey(position)) {
            return null;
        }
        return Tiles[TilePositionIndex[position]];

    }

    public class Room
    {
        public string Name { get; private set; }

        public Room(string name)
        {
            Name = name;
        }
    }

    public class Connection
    {
        public int Origin { get; private set; }
        public int Destination { get; private set; }

        public Connection(int origin, int destination)
        {
            Origin = origin;
            Destination = destination;
        }
    }

    public class Character
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Tile { get; private set; }
        public int StartTileId { get; private set; }

        public Character(int id, string name, string tile, int startTileId)
        {
            Id = id;
            Name = name;
            Tile = tile;
            StartTileId = startTileId;
        }
    }

    public class Weapon
    {
        public int Id;
        public string Name;
    }

    public class Tile
    {
        public int Id { get; private set; }
        public Vector2Int Position { get; private set; }
        public string RoomName { get; private set; }
        public string TileName { get; private set; }

        public Tile(int id, int x, int y, string roomName, string tileName)
        {
            Id = id;
            Position = new Vector2Int(x, y);
            RoomName = roomName;
            TileName = tileName;
        }
    }
}
