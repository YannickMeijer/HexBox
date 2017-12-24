using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    PlayingFieldController controller;
    List<Node> open;
    Dictionary<Point, Point> previousNode;
    HashSet<Point> closed;
    HexagonDirection[] directions;

	// Use this for initialization
	void Start () {
        directions = (HexagonDirection[])Enum.GetValues(typeof(HexagonDirection));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public List<Point> FindPath(Point start, Point goal)
    {
        resetStructures();
        int heuristicCost = costFunction(start, goal);
        Node startNode = new Node(start, 0, heuristicCost);

        open.Add(startNode);

        bool pathfound = false;

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
                heuristicCost = costFunction(futureNode, goal);
                Node addNode = new Node(futureNode, currentNode.fromStart + 1, heuristicCost);
                open.Add(addNode);
                if (futureNode == goal)
                    pathfound = true;
            }
            open.OrderByDescending(a => a.totalCost);
        }

        List<Point> returnList = new List<Point>();
        bool pathUnwound = false;
        returnList.Add(goal);

        while (!pathUnwound)
        {
            if (returnList[returnList.Count - 1] == start)
            {
                pathUnwound = true;
                continue;
            }
                returnList.Add(previousNode[returnList[returnList.Count - 1]]);
        }
            return returnList;
    }

    public int costFunction(Point currentHex, Point goalHex)
    {
        float difX = Mathf.Abs(goalHex.X - currentHex.X);
        float difY = Mathf.Abs(goalHex.Y - currentHex.Y);

        if (difX % 2 == 1)
            difY += 0.5f;

        return (int)(difX + difY - difX / 2);
    }

    public void resetStructures()
    {
        open.Clear();
        closed.Clear();
        previousNode.Clear();
    }

    public struct Node {
        public Point thisNode;
        public int fromStart;
        public int tillFinish;
        public int totalCost;

        public Node(Point location, int costSoFar, int costTillFinish)
        {
            thisNode   = location;
            fromStart  = costSoFar;
            tillFinish = costTillFinish;
            totalCost  = fromStart + tillFinish;
        }
            
    }
}
