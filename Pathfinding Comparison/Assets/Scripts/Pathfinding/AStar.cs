using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public GridManager gridManager;

    //void Update()
    //{
    //    gridManager = FindAnyObjectByType<GridManager>();

    //    Node start = gridManager.grid[0, 0];
    //    Node end = gridManager.grid[gridManager.width - 1, gridManager.height - 1];

    //    List<Node> path = FindPath(start, end);

    //    // Visualize path
    //    foreach (Node node in path)
    //    {
    //        Vector3 pos = new Vector3(node.x, node.y, 0);
    //        GameObject tile = GameObject.Find($"Tile_{node.x}_{node.y}");
    //        tile.GetComponent<SpriteRenderer>().color = Color.green;
    //    }
    //}

    public List<Node> FindPath(Node start, Node target)
    {
        List<Node> openSet = new List<Node> { start };
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            Node current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < current.FCost ||
                    (openSet[i].FCost == current.FCost && openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == target)
                return RetracePath(start, target);

            foreach (Node neighbor in gridManager.GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newCost = current.gCost + GetDistance(current, neighbor);
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetDistance(neighbor, target);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Node>(); // No path found
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    float GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return dx + dy; // Manhattan distance
    }
}
