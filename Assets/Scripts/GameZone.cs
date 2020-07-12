using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class GameZone : MonoBehaviour
    {
        Tilemap boardTilemap;
        Tilemap overlayTilemap;
        PathFinder pathFinder;
        List<Vector3Int> reachableCells;
        bool mousePressed = false;

        // Start is called before the first frame update
        void Start()
        {
            SetupTiles();
            pathFinder = new PathFinder(boardTilemap);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int cellCoords = GetCellUnderMouse();
                if (pathFinder.IsCellWalkable(cellCoords))
                {
                    mousePressed = true;
                    reachableCells = pathFinder.GetReachableCells(cellCoords, 4);
                    foreach (Vector3Int cell in reachableCells)
                    {
                        overlayTilemap.SetTile(cell, Tiles.GREEN);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (mousePressed)
                {
                    mousePressed = false;
                    foreach (Vector3Int cell in reachableCells)
                    {
                        overlayTilemap.SetTile(cell, null);
                    }

                }
            }
            return;
        }

        private Vector3Int GetCellUnderMouse()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellCoords = boardTilemap.WorldToCell(mousePosition);
            cellCoords.z = 0;
            return cellCoords;
        }

        private void SetupTiles()
        {
            Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap.gameObject.name == "BoardTilemap")
                {
                    boardTilemap = tilemap;
                }

                if (tilemap.gameObject.name == "OverlayTilemap")
                {
                    overlayTilemap = tilemap;
                }
            }
        }
    }
}
