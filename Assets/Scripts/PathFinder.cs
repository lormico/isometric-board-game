using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Level Level;

    public List<Vector3Int> GetReachableCells(Vector3Int center, int maxDistance)
    {
        Stack<Level.Tile> frontier = new Stack<Level.Tile>();
        HashSet<Level.Tile> reachableTiles = new HashSet<Level.Tile>();
        Dictionary<Level.Tile, int> distanceSoFar = new Dictionary<Level.Tile, int>();

        Level.Tile centerTile = Level.GetTile((Vector2Int)center);
        Level.Room centerRoom = Level.GetRoom(centerTile.RoomName);
        frontier.Push(centerTile);
        reachableTiles.Add(centerTile);
        distanceSoFar[centerTile] = 0;

        while (frontier.Count > 0)
        {
            Level.Tile currentTile = frontier.Pop();
            Level.Room currentRoom = Level.GetRoom(currentTile.RoomName);
            foreach (Level.Tile nextTile in GetNeighboringWalkableTiles(currentTile))
            {
                int newDistance = distanceSoFar[currentTile] + 1;
                if (newDistance > maxDistance)
                {
                    continue;
                }

                Level.Room nextRoom = Level.GetRoom(nextTile.RoomName);
                if (currentRoom != centerRoom && nextRoom != currentRoom)
                {
                    /* Next would be a different room but we're pathing from another room
                     * than the starting one: denied (can't pass through multiple rooms)
                     */
                    continue;
                }

                if (!distanceSoFar.ContainsKey(nextTile) || newDistance < distanceSoFar[nextTile])
                {
                    distanceSoFar[nextTile] = newDistance;
                    frontier.Push(nextTile);
                    reachableTiles.Add(nextTile);
                }
            }
        }

        return reachableTiles.Select(tile => (Vector3Int)tile.Position).ToList();
    }

    /// <summary>
    /// Gets the 4 strictly adjacent tiles around a specified "center" tile, and checks if each
    /// is walkable and if the movement from the center tile to it is allowed.
    /// </summary>
    /// <param name="center">The position around which the walkable tiles are retrieved</param>
    /// <returns>The list of adjacent walkable board tiles</returns>
    private IList<Level.Tile> GetNeighboringWalkableTiles(Level.Tile centerTile)
    {
        IList<Level.Tile> neighboringTiles = new List<Level.Tile>();
        IList<Vector2Int> movements = new List<Vector2Int>() {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
        };

        foreach (Vector2Int movement in movements)
        {
            Level.Tile neighboringTile = Level.GetTile(new Vector2Int(centerTile.Position.x + movement.x, centerTile.Position.y + movement.y));
            if (IsTileWalkable(neighboringTile) && IsMovementAllowed(centerTile, neighboringTile))
            {
                neighboringTiles.Add(neighboringTile);
            }
        }

        return neighboringTiles;
    }

    private bool IsTileWalkable(Level.Tile cell)
    {
        return cell != null;
    }

    private bool IsMovementAllowed(Level.Tile originTile, Level.Tile destinationTile)
    {
        // Check if the path is impeded by an obstacle
        return !Level.HasObstacle(originTile.Position, destinationTile.Position);
    }
}
