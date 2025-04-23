using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public GameObject tilePrefab;
    public float obstacleChance = 0.2f;

    public Node[,] grid;

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

                bool walkable = Random.value > obstacleChance;
                tile.GetComponent<TileComponent>().Init(x, y, walkable);

                grid[x, y] = new Node(x, y, walkable);
            }
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
