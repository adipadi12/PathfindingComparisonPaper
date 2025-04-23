using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int start, end;
    public Color exploredColor = Color.cyan;
    public Color pathColor = Color.green;

    private void Start()
    {
        StartCoroutine(RunBFS());
    }

    IEnumerator RunBFS()
    {
        Node[,] grid = gridManager.grid;
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        Queue<Node> queue = new Queue<Node>();
        bool[,] visited = new bool[width, height];

        Node startNode = grid[start.x, start.y];
        Node endNode = grid[end.x, end.y];

        queue.Enqueue(startNode);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            // Visualize exploration
            ColorTile(current.x, current.y, exploredColor);
            yield return new WaitForSeconds(0.02f); // for visual pacing

            if (current == endNode)
            {
                break;
            }

            foreach (Node neighbor in GetNeighbors(current, grid, width, height))
            {
                if (!visited[neighbor.x, neighbor.y] && neighbor.walkable)
                {
                    visited[neighbor.x, neighbor.y] = true;
                    neighbor.parent = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Trace back path
        Node pathNode = endNode;
        while (pathNode != startNode && pathNode.parent != null)
        {
            ColorTile(pathNode.x, pathNode.y, pathColor);
            pathNode = pathNode.parent;
            yield return new WaitForSeconds(0.02f);
        }
    }

    List<Node> GetNeighbors(Node node, Node[,] grid, int width, int height)
    {
        List<Node> neighbors = new List<Node>();

        int[,] dirs = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            int nx = node.x + dirs[i, 0];
            int ny = node.y + dirs[i, 1];

            if (nx >= 0 && ny >= 0 && nx < width && ny < height)
            {
                neighbors.Add(grid[nx, ny]);
            }
        }

        return neighbors;
    }

    void ColorTile(int x, int y, Color color)
    {
        Collider2D col = Physics2D.OverlapPoint(new Vector2(x, y));
        if (col != null)
        {
            SpriteRenderer sr = col.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = color;
        }
    }
}
