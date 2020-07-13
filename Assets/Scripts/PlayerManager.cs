using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public Tilemap playerTilemap;
    public TextAsset playersConfig;
    private List<Player> players;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadPlayers()
    {
        players = new List<Player>();
        PlayerJsonData[] playerJsons = JsonHelper.FromJson<PlayerJsonData>(playersConfig.text);
        foreach (PlayerJsonData playerJsonData in playerJsons)
        {
            players.Add(new Player(
                playerTilemap,
                (Tile)Resources.Load("Tiles/Players/" + playerJsonData.tile, typeof(Tile)),
                playerJsonData.startPosition));
        }
    }

    public Player GetPlayer(int id)
    {
        return players[id];
    }

    public bool CanMoveTo(Vector3Int cell)
    {
        return playerTilemap.GetTile(cell) == null;
    }

    [Serializable]
    public class PlayerJsonData
    {
        public Vector3Int startPosition;
        public string tile;
    }

}
