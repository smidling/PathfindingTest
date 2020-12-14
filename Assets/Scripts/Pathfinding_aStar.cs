using UnityEngine;
using System.Collections.Generic;

public class Pathfinding_aStar : PathfindingAlgorithm
{
    public override List<PathfindingNode> FindPath(LevelGenerator levelNodes, PathfindingNode startNode, PathfindingNode targetNode)
    {
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            PathfindingNode node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
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

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }
}