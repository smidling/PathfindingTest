using UnityEngine;
using System.Collections.Generic;

public abstract class PathfindingAlgorithm
{
    protected List<PathfindingNode> openSet;
    protected List<PathfindingNode> closedSet;
    protected PathfindingNode[,] LevelPathfindingNodes;
    protected bool pathfindingActive = false;
    protected bool pathfindingDone = false;
    protected PathfindingNode startNode;
    protected PathfindingNode targetNode;
    protected int maxCycles = 500;
    protected int cycleNum = 0;

    public abstract List<PathfindingNode> FindPath(LevelGenerator tempLevelNodes, PathfindingNode tempStartNode,
        PathfindingNode tempTargetNode);

    public void StartPathfindingSteps(LevelGenerator tempLevelNodes, PathfindingNode tempStartNode, PathfindingNode tempTargetNode)
    {
        LevelPathfindingNodes = new PathfindingNode[tempLevelNodes.LevelSize, tempLevelNodes.LevelSize];
        LevelPathfindingNodes = tempLevelNodes.CopyLevelNodes();
        startNode = LevelPathfindingNodes[tempStartNode.gridX, tempStartNode.gridY];
        targetNode = LevelPathfindingNodes[tempTargetNode.gridX, tempTargetNode.gridY];
        openSet = new List<PathfindingNode>();
        closedSet = new List<PathfindingNode>();
        openSet.Add(startNode);
        pathfindingActive = true;
    }

    public abstract bool PathfindingStep();
    
    public List<PathfindingNode> GetPath()
    {
        if (pathfindingDone)
        {
            return RetracePath(startNode, targetNode);
        }
        return null;
    }
    public List<PathfindingNode> GetClosedSet()
    {
        return closedSet;
    }

    public List<PathfindingNode> GetOpenSet()
    {
        return openSet;    
    }

    protected List<PathfindingNode> RetracePath(PathfindingNode fromNode, PathfindingNode goalNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        PathfindingNode currentNode = goalNode;
        while (currentNode != fromNode && currentNode.parent != currentNode && currentNode.parent != null)
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


    protected List<PathfindingNode> GetNeighbours(PathfindingNode node)
    {
        int levelSize = (int)Mathf.Sqrt(LevelPathfindingNodes.Length);
        List<PathfindingNode> neighbours = new List<PathfindingNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                if (Mathf.Abs(x) + Mathf.Abs(y) == 2)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < levelSize && checkY >= 0 && checkY < levelSize)
                {
                    neighbours.Add(LevelPathfindingNodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
}
