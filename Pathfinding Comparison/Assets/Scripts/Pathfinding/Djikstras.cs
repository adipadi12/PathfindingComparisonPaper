using System.Collections.Generic;
using UnityEngine;

public class Djikstras : MonoBehaviour
{
    public int startX, startY, endX, endY;
    public Color pathColor = Color.red;

    public GridManager gridManager;

    //void Update()
    //{
    //    gridManager = FindAnyObjectByType<GridManager>();
    //    Node start = gridManager.grid[startX, startY];
    //    Node end = gridManager.grid[endX, endY];
    //    List<Node> path = FindPath(start, end);
    //    gridManager.VisualizePath(path, pathColor);
    //}

    public List<Node> FindPath(Node start, Node end)
    {
        var openSet = new List<Node> { start };
        var cameFrom = new Dictionary<Node, Node>();
        start.gCost = 0;
        cameFrom[start] = null;

        while (openSet.Count > 0)
        {
            // pick node with smallest gCost
            Node current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
                if (openSet[i].gCost < current.gCost)
                    current = openSet[i];

            openSet.Remove(current);
            if (current == end) break;

            foreach (var nbr in gridManager.GetNeighbors(current))
            {
                if (!nbr.walkable) continue;

                float newCost = current.gCost + 1; // uniform weight
                if (!cameFrom.ContainsKey(nbr) || newCost < nbr.gCost)
                {
                    nbr.gCost = newCost;
                    cameFrom[nbr] = current;
                    if (!openSet.Contains(nbr))
                        openSet.Add(nbr);
                }
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
