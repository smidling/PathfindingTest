
using UnityEngine;

public class PathfindingNode
{
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public PathfindingNode parent;

    public PathfindingNode(bool _walkable, int _gridX, int _gridY)
    {
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }

    public Vector3 GetWorldPos()
    {
        return new Vector3(gridX,gridY,0);
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public PathfindingNode CopyNode()
    {
        PathfindingNode result = new PathfindingNode(walkable,gridX,gridY);
        return result;
    }
}