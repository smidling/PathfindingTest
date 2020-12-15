using UnityEngine;
using System.Collections.Generic;

public class Pathfinding_aStar : PathfindingAlgorithm
{


    public override List<PathfindingNode> FindPath(LevelGenerator tempLevelNodes, PathfindingNode tempStartNode, PathfindingNode tempTargetNode)
    {
        StartPathfindingSteps(tempLevelNodes, tempStartNode, tempTargetNode);
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
            foreach (PathfindingNode neighbour in GetNeighbours(node))
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


    public override bool PathfindingStep()
    {
        if (!pathfindingActive && pathfindingDone)
            return true;

        if (openSet.Count > 0)
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
                pathfindingDone = true;
                return true;
            }
            foreach (PathfindingNode neighbour in GetNeighbours(node))
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
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        cycleNum++;
        return false;
    }
    
}