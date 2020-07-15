using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

public class Level
{
    public string Name { get; private set; }
    public string Pack { get; private set; }
    public IList<Room> Rooms { get; private set; }
    public IList<Tile> Tiles { get; private set; }
    public IList<Character> Characters { get; private set; }
    public IList<Weapon> Weapons { get; private set; }

    public static Level FromFile(string filePath)
    {
        Level instance = new Level();

        string connection = "URI=file:" + filePath;
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDictionary<string, string> levelMetadata = GetLevelMetadata(dbcon);
        instance.Pack = levelMetadata["pack"];
        instance.Name = levelMetadata["level_name"];

        instance.Rooms = GetLevelRooms(dbcon);
        instance.Tiles = GetLevelTiles(dbcon);
        instance.Characters = GetLevelCharacters(dbcon);
        instance.Weapons = GetLevelWeapons(dbcon);

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

    private static IList<Room> GetLevelRooms(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT name, door_x, door_y, door_opening FROM rooms";
        reader = cmnd_read.ExecuteReader();
        IDictionary<string, IList<Door>> doorDict = new Dictionary<string, IList<Door>>();
        while (reader.Read())
        {
            string name = reader.GetString(reader.GetOrdinal("name"));
            if (!doorDict.ContainsKey(name))
            {
                doorDict[name] = new List<Door>();
            }

            Door door = null;
            if (!reader.IsDBNull(reader.GetOrdinal("door_x")) &&
                !reader.IsDBNull(reader.GetOrdinal("door_y")) &&
                !reader.IsDBNull(reader.GetOrdinal("door_opening")))
            {
                door = new Door(
                    reader.GetInt32(reader.GetOrdinal("door_x")),
                    reader.GetInt32(reader.GetOrdinal("door_y")),
                    reader.GetInt32(reader.GetOrdinal("door_opening")));
            }
            if (door != null)
            {
                doorDict[name].Add(door);
            }
        }

        IList<Room> rooms = new List<Room>();
        foreach (KeyValuePair<string, IList<Door>> item in doorDict)
        {
            rooms.Add(new Room(item.Key, item.Value));
        }

        return rooms;
    }

    private static IList<Tile> GetLevelTiles(IDbConnection dbcon)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        cmnd_read.CommandText = "SELECT x, y, room, tile FROM board_tiles";
        reader = cmnd_read.ExecuteReader();
        IList<Tile> tiles = new List<Tile>();
        while (reader.Read())
        {
            tiles.Add(new Tile(
                reader.GetInt32(reader.GetOrdinal("x")),
                reader.GetInt32(reader.GetOrdinal("y")),
                reader.GetString(reader.GetOrdinal("room")),
                reader.GetString(reader.GetOrdinal("tile"))
                ));
        }

        return tiles;
    }

    private static IList<Character> GetLevelCharacters(IDbConnection dbcon)
    {
        IList<Character> characters = new List<Character>();

        return characters;
    }

    private static IList<Weapon> GetLevelWeapons(IDbConnection dbcon)
    {
        IList<Weapon> weapons = new List<Weapon>();

        return weapons;
    }

    public class Room
    {
        public string Name { get; private set; }
        public IList<Door> Doors { get; private set; }

        public Room(string name, IList<Door> doors)
        {
            Name = name;
            Doors = doors;
        }
    }

    public class Door
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int Opening { get; private set; }

        public Door(int x, int y, int opening)
        {
            this.x = x;
            this.y = y;
            this.Opening = opening;
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
        public int x { get; private set; }
        public int y { get; private set; }
        public string RoomName { get; private set; }
        public string TileName { get; private set; }

        public Tile(int x, int y, string roomName, string tileName)
        {
            this.x = x;
            this.y = y;
            RoomName = roomName;
            TileName = tileName;
        }
    }
}
