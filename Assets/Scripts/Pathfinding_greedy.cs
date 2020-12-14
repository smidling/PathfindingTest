using UnityEngine;
using System.Collections.Generic;

public class Pathfinding_greedy : PathfindingAlgorithm
{
    public override List<PathfindingNode> FindPath(LevelGenerator levelNodes, PathfindingNode startNode, PathfindingNode targetNode)
    {
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            PathfindingNode node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                float distNode = GetDistance(node, targetNode);
                float newDist = GetDistance(openSet[i], targetNode);
                if (newDist <= distNode)
                {
                    // favor y movement (down movement)
                    if (openSet[i].gridY < node.gridY)
                        node = openSet[i];
                }
            }
            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
            foreach (PathfindingNode neighbour in levelNodes.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                if (!openSet.Contains(neighbour))
                {
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }
}