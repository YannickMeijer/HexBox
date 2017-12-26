using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    PlayingFieldController controller;

    //all nodes still unchecked for the path
    List<Node> open;
    //Key is the child, value is the parent. Used to find the path from the end to the start
    Dictionary<Point, Point> previousNode;
    //contains all nodes already checked.
    HashSet<Point> closed;
    //A list containing every value of the HexagonDirection Enum for easy iteration
    HexagonDirection[] directions;

	void Start ()
    {
        directions = (HexagonDirection[])Enum.GetValues(typeof(HexagonDirection));
    }

    //Runs A*, parameters speak for themselves. Start should be the point the unit is on.
    public List<Point> FindPath(Point start, Point goal)
    {
        resetStructures();
        int heuristicCost = Heuristic(start, goal);
        Node startNode = new Node(start, 0, heuristicCost, 0);

        open.Add(startNode);

        bool pathfound = false;
        //Take the current node, find its neighbours and add them to relevant list with costs calculated.
        while (!pathfound)
        {
            Node currentNode = open[open.Count - 1];
            open.RemoveAt(open.Count - 1);

            closed.Add(currentNode.thisNode);

            for (int x = 0; x < 6; x++)
            {
                Point futureNode = controller.GetNeighbor(currentNode.thisNode, directions[x]);
                if (closed.Contains(futureNode))
                    continue;

                previousNode.Add(futureNode, currentNode.thisNode);
                heuristicCost = Heuristic(futureNode, goal);
                int additionalCost = currentNode.additionalCost + AdditionalCostFunction(futureNode);
                Node addNode = new Node(futureNode, currentNode.fromStart + 1, heuristicCost, 0);
                open.Add(addNode);
                if (futureNode == goal)
                    pathfound = true;
            }
            open.OrderByDescending(a => a.totalCost);
            if (open.Count == 0)
                return null;
        }

        return UnwindPath(goal, start);
    }

    //Starts from the goal and finds its way back to the start point, then reverses the list so the first move is at 0
    List<Point> UnwindPath(Point goal, Point start)
    {
        List<Point> returnList = new List<Point>();
        returnList.Add(goal);

        bool pathUnwound = false;
        while (!pathUnwound)
        {
            if (returnList[returnList.Count - 1] == start)
            {
                pathUnwound = true;
                continue;
            }
            returnList.Add(previousNode[returnList[returnList.Count - 1]]);
        }
        returnList.Reverse();
        return returnList;
    }

    //The heuristic for hexagons
    public int Heuristic(Point currentHex, Point goalHex)
    {
        float difX = Mathf.Abs(goalHex.X - currentHex.X);
        float difY = Mathf.Abs(goalHex.Y - currentHex.Y);

        if (difX % 2 == 1)
            difY += 0.5f;

        return (int)(difX + difY - difX / 2);
    }

    //The cost function for additional stuff, like not wanting to move through lava
    public int AdditionalCostFunction(Point costIncurred)
    {
        return 0;
    }

    //resets everything so a new call can be done with a clean slate
    public void resetStructures()
    {
        open.Clear();
        closed.Clear();
        previousNode.Clear();
    }

    // A node containing everything needed for pathfinding
    public struct Node {
        public Point thisNode;
        public int fromStart;
        public int tillFinish;
        public int additionalCost;
        public int totalCost;

        public Node(Point location, int costSoFar, int costTillFinish, int additional)
        {
            thisNode   = location;
            fromStart  = costSoFar;
            tillFinish = costTillFinish;
            additionalCost = additional;
            totalCost = fromStart + tillFinish + additionalCost;
        }
            
    }
}
