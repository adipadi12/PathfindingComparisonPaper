using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public GameObject tilePrefab;
    public float obstacleChance = 0.2f;

    public Node[,] grid;
    public Dictionary<Vector2Int, GameObject> tileObjects = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);

                tile.name = $"Tile_{x}_{y}";
                tileObjects[new Vector2Int(x, y)] = tile;

                bool walkable = Random.value > obstacleChance;
                tile.GetComponent<TileComponent>().Init(x, y, walkable);

                grid[x, y] = new Node(x, y, walkable);
            }
        }
    }

    public void VisualizePath(List<Node> path, Color color)
    {
        foreach (Node n in path)
        {
            var key = new Vector2Int(n.x, n.y);
            if (tileObjects.TryGetValue(key, out GameObject tile))
                tile.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue; // Skip the current node itself

                int checkX = node.x + dx;
                int checkY = node.y + dy;

                // Make sure we're within bounds
                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    Node neighbor = grid[checkX, checkY];
                    if (neighbor.walkable)
                        neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }
}
