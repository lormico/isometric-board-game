using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    public Level level;

    public List<Vector3Int> GetReachableCells(Vector3Int center, int distance)
    {
        Stack<Vector3Int> frontier = new Stack<Vector3Int>();
        HashSet<Vector3Int> reachableCells = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();

        frontier.Push(center);
        reachableCells.Add(center);
        costSoFar[center] = 0;

        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Pop();
            foreach (Vector3Int next in GetNeighboringWalkableCells(current))
            {
                int newCost = costSoFar[current] + 1;
                if (newCost > distance)
                {
                    continue;
                }
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    frontier.Push(next);
                    reachableCells.Add(next);
                }
            }
        }

        return new List<Vector3Int>(reachableCells);
    }

    private IList<Vector3Int> GetNeighboringWalkableCells(Vector3Int center)
    {
        IList<Vector3Int> neighboringCells = new List<Vector3Int>();
        IList<Vector2Int> movements = new List<Vector2Int>() {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
        };

        foreach (Vector2Int movement in movements)
        {
            Vector3Int neighboringCell = new Vector3Int(center.x + movement.x, center.y + movement.y, 0);
            if (IsCellWalkable(neighboringCell))
            {
                neighboringCells.Add(neighboringCell);
            }
        }

        return neighboringCells;
    }

    public bool IsCellWalkable(Vector3Int cell)
    {
        return level.GetTile((Vector2Int)cell) != null;
    }
}