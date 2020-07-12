using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Game
{
    public class GameZone : MonoBehaviour
    {
        public Button scrambleButton;
        public Text diceResult;
        public Tilemap boardTilemap;
        public Tilemap overlayTilemap;
        public Tilemap playerTilemap;
        public Player player;

        PathFinder pathFinder;

        bool mousePressed = false;
        List<Vector3Int> reachableCells = new List<Vector3Int>();

        // Start is called before the first frame update
        void Start()
        {
            pathFinder = new PathFinder(boardTilemap);
            SetupUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3Int selectedCell = GetCellUnderMouse();
                if (reachableCells.Contains(selectedCell))
                {
                    player.MoveTo(selectedCell);
                    reachableCells.Clear();
                    RefreshReachableCells();
                }
            }
        }

        private Vector3Int GetCellUnderMouse()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellCoords = boardTilemap.WorldToCell(mousePosition);
            cellCoords.z = 0;
            return cellCoords;
        }

        private void SetupUI()
        {
            Button scrambleButton = this.scrambleButton.GetComponent<Button>();
            scrambleButton.onClick.AddListener(ScrambleClicked);

            Text diceResult = this.diceResult.GetComponent<Text>();
            diceResult.text = "";
        }

        private void ScrambleClicked()
        {
            int dice1 = Random.Range(1, 6);
            int dice2 = Random.Range(1, 6);
            int distance = dice1 + dice2;
            diceResult.GetComponent<Text>().text = distance.ToString();
            reachableCells = pathFinder.GetReachableCells(player.GetPosition(), distance);
            RefreshReachableCells();
        }

        private void RefreshReachableCells()
        {
            foreach (Vector3Int cell in overlayTilemap.cellBounds.allPositionsWithin)
            {
                overlayTilemap.SetTile(cell, null);
            }
            foreach (Vector3Int cell in reachableCells)
            {
                overlayTilemap.SetTile(cell, Tiles.GREEN);
            }
        }
    }
}
