using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

public class Level
{
    public string Name { get; private set; }
    public string Pack { get; private set; }
    public IList<Room> Rooms { get; private set; }
    public IDictionary<int, Tile> Tiles { get; private set; }
    public IList<Connection> Obstacles { get; private set; }
    public IList<Character> Characters { get; private set; }
    public IList<Weapon> Weapons { get; private set; }
    public IList<Connection> Shortcuts { get; private set; }

    public static Level FromFile(string filePath)
    {
        Level instance = new Level();

        string connection = "URI=file:" + filePath;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDictionary<string, string> levelMetadata = GetLevelMetadata(dbcon);
        instance.Pack = levelMetadata["pack"];
        instance.Name = levelMetadata["level_name"];

        instance.Rooms = GetRooms(dbcon);
        instance.Tiles = GetTiles(dbcon);
        instance.Obstacles = GetObstacles(dbcon);
        instance.Characters = GetCharacters(dbcon);
        instance.Weapons = GetWeapons(dbcon);
        instance.Shortcuts = GetShortcuts(dbcon);

        dbcon.Close();

        return instance;
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

    private static IDictionary<int, Tile> GetTiles(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT tile_id, x, y, room, tile FROM board_tiles";
        reader = cmnd_read.ExecuteReader();
        IDictionary<int, Tile> tiles = new Dictionary<int, Tile>();
        while (reader.Read())
        {
            int tile_id = reader.GetInt32(reader.GetOrdinal("tile_id"));
            tiles.Add(tile_id, new Tile(
                tile_id,
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
        IList<Character> characters = new List<Character>();

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
        public int Id;
        public string Name;
        public string Tile;
    }

    public class Weapon
    {
        public int Id;
        public string Name;
    }

    public class Tile
    {
        public int Id { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public string RoomName { get; private set; }
        public string TileName { get; private set; }

        public Tile(int id, int x, int y, string roomName, string tileName)
        {
            Id = id;
            this.x = x;
            this.y = y;
            RoomName = roomName;
            TileName = tileName;
        }
    }
}
