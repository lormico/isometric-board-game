using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public Tilemap overlayTilemap;
    public Tilemap playerTilemap;
    public TileProvider tileProvider;
    public TextAsset playersConfig;

    private List<Player> players;
    private Player currentPlayer;

    public string Mode { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayers();
        currentPlayer = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Mode == null)
            {
                return;
            }
            else if (Mode == "moving")
            {
                Vector3Int selectedCell = GetCellUnderMouse();
                if (currentPlayer.ReachableCells.Contains(selectedCell) && CanMoveTo(selectedCell))
                {
                    currentPlayer.MoveTo(selectedCell);
                    currentPlayer.ReachableCells.Clear();
                    RefreshReachableCells();
                    Mode = "selecting";
                }
            }
            else if (Mode == "selecting")
            {
                Vector3Int selectedCell = GetCellUnderMouse();
                foreach (Player player in players)
                {
                    if (player.Position == selectedCell)
                    {
                        currentPlayer = player;
                        break;
                    }
                }
            }
        }
    }

    private Vector3Int GetCellUnderMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellCoords = playerTilemap.WorldToCell(mousePosition);
        cellCoords.z = 0;
        return cellCoords;
    }

    private void RefreshReachableCells()
    {
        foreach (Vector3Int cell in overlayTilemap.cellBounds.allPositionsWithin)
        {
            overlayTilemap.SetTile(cell, null);
        }
        foreach (Vector3Int cell in currentPlayer.ReachableCells)
        {
            overlayTilemap.SetTile(cell, tileProvider.GetOverlayTile("Vanilla", "reachable"));
        }
    }

    public void SetWalkableDistance(int distance)
    {
        currentPlayer.SetWalkableDistance(distance);
        RefreshReachableCells();
    }

    private void LoadPlayers()
    {
        players = new List<Player>();
        PlayerJsonData[] playerJsons = JsonHelper.FromJson<PlayerJsonData>(playersConfig.text);
        foreach (PlayerJsonData playerJsonData in playerJsons)
        {
            players.Add(new Player(
                playerTilemap,
                tileProvider.GetPlayerTile("Vanilla", playerJsonData.tile),
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
