using System.Collections.Generic;
using UnityEngine;

public class BFSPathfinder : MonoBehaviour
{
    public int startX, startY, endX, endY;      // set in Inspector
    public Color pathColor = Color.yellow;       // path will be this color

    public GridManager gridManager;

    void Update()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        Node start = gridManager.grid[startX, startY];
        Node end = gridManager.grid[endX, endY];
        List<Node> path = FindPath(start, end);
        gridManager.VisualizePath(path, pathColor);
    }

    public List<Node> FindPath(Node start, Node end)
    {
        var queue = new Queue<Node>();
        var visited = new HashSet<Node>();
        var cameFrom = new Dictionary<Node, Node>();

        queue.Enqueue(start);
        visited.Add(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == end) break;

            foreach (var nbr in gridManager.GetNeighbors(current))
            {
                if (!nbr.walkable || visited.Contains(nbr))
                    continue;

                visited.Add(nbr);
                cameFrom[nbr] = current;
                queue.Enqueue(nbr);
            }
        }

        // backtrack
        var path = new List<Node>();
        Node node = end;
        while (node != null && cameFrom.ContainsKey(node))
        {
            path.Add(node);
            node = cameFrom[node];
        }
        path.Reverse();
        return path;
    }
}
