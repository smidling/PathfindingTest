using UnityEngine;
using System.Collections.Generic;

public abstract class PathfindingAlgorithm
{ 
    protected  List<PathfindingNode> openSet = new List<PathfindingNode>();
    protected  HashSet<PathfindingNode> closedSet = new HashSet<PathfindingNode>();

    public abstract List<PathfindingNode> FindPath(LevelGenerator levelNodes, PathfindingNode startNode,
        PathfindingNode targetNode);

    public HashSet<PathfindingNode> GetClosedSet()
    {
        return closedSet;
    }

    public List<PathfindingNode> GetOpenSet()
    {
        return openSet;
    }

    protected List<PathfindingNode> RetracePath(PathfindingNode startNode, PathfindingNode endNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    protected int GetDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return 10 * (dstY + dstX);
    }
}
