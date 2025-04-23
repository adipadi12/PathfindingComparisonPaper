using UnityEngine;

public class Node
{
    public int x, y;
    public bool walkable;
    public float gCost, hCost;
    public float FCost => gCost + hCost;
    public Node parent;

    public Node(int x, int y, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
    }
}

