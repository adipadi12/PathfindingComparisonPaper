using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public int x, y;
    public bool walkable;

    public void Init(int x, int y, bool walkable)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;

        GetComponent<SpriteRenderer>().color = walkable ? Color.white : Color.black;
    }
}

