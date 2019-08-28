using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding{
/*
    PlayingFieldController controller;

    //all nodes still unchecked for the path
    public List<HexNode> open;
    //Key is the childnode, value is the parentnode. Used to find the path from the end to the start
    Dictionary<HexagonTile, HexagonTile> previousNode;
    //contains all nodes already checked.
    HashSet<HexagonTile> checkedNodes;
    //A list containing every value of the HexagonDirection Enum for easy iteration
    

	void Start ()
    {
        
        open = new List<HexNode>();
        controller = GameObject.FindGameObjectWithTag("PlayingField").GetComponent<PlayingFieldController>();
        previousNode = new Dictionary<HexagonTile, HexagonTile>();
        checkedNodes = new HashSet<HexagonTile>();
    }

    //Runs A*, parameters speak for themselves. Start should be the point the unit is on.
    public Queue<HexagonTile> FindPath(HexagonTile start, HexagonTile goal, Unit requester)
    {
        resetStructures();
        int heuristicCost = Heuristic(start, goal);
        HexNode startNode = new HexNode(start, 0, heuristicCost, 0);

        open.Add(startNode);
        checkedNodes.Add(startNode.currentHex);

        bool pathfound = false;

        //Take the current node, find its neighbours and add them to relevant list with costs calculated.
        while (!pathfound)
        {
            List<HexagonTile> neighbours = new List<HexagonTile>();
            HexNode currentNode = open[open.Count - 1];
            open.RemoveAt(open.Count - 1);
            neighbours = controller.GetAllNeighbours(currentNode.currentHex);

            //add the 6 neighbours of the node that's currently being checked.
            foreach(HexagonTile futureTile in neighbours)
            {
                if (requester.InSight(futureTile) )
                {
                    if (futureTile == null || checkedNodes.Contains(futureTile))
                        continue;
                    else
                    {
                        checkedNodes.Add(futureTile);
                        if (futureTile.blocked)
                            continue;
                    }

                    CreateAndAddHexNode(futureTile, goal, currentNode);
                    if (futureTile == goal)
                        pathfound = true;
                }
            }

            open = open.OrderByDescending(a => a.totalCost).ToList<HexNode>();

            if (open.Count == 0)
                return new Queue<HexagonTile>();
        }

        return UnwindPath(goal, start);
    }

    //Starts from the goal and finds its way back to the start point, then reverses the list so the first move is at 0
    Queue<HexagonTile> UnwindPath(HexagonTile goal, HexagonTile start)
    {
        Queue<HexagonTile> returnQueue = new Queue<HexagonTile>();
        List<HexagonTile> temp = new List<HexagonTile>();
        temp.Add(goal);

        bool pathUnwound = false;
        while (!pathUnwound)
        {
            if (temp[temp.Count - 1] == start)
            {
                pathUnwound = true;
                continue;
            }
            temp.Add(previousNode[temp[temp.Count - 1]]);
        }
        while(temp.Count != 0)
        {
            returnQueue.Enqueue(temp[temp.Count - 1]);
            temp.RemoveAt(temp.Count - 1);
        }
        return returnQueue;
    }

    //The heuristic for hexagons
    public int Heuristic(HexagonTile currentHex, HexagonTile goalHex)
    {
        //shift the Y value by .5 if they're on an uneven X tile for calculations
        float addedCur = 0;
        float addedGoal = 0;
        if (goalHex.TileX % 2 == 1)
            addedGoal = 0.5f;
        if (currentHex.TileX % 2 == 1)
            addedCur = 0.5f;

        float difX = Mathf.Abs(goalHex.TileX - currentHex.TileX);
        float difY = Mathf.Abs((addedGoal + goalHex.TileZ) - (addedCur + currentHex.TileZ));

        return (int)(difX + Mathf.Clamp(difY - (difX / 2), 0, float.MaxValue));
    }

    //The cost function for additional stuff, like not wanting to move through lava
    int AdditionalCostFunction(HexagonTile costIncurred)
    {
        return 0;
    }

    //resets everything so a new call can be done with a clean slate
    void resetStructures()
    {
        open.Clear();
        checkedNodes.Clear();
        previousNode.Clear();
    }

    void CreateAndAddHexNode(HexagonTile current, HexagonTile goal, HexNode parentNode)
    {
        previousNode.Add(current, parentNode.currentHex);
        int heuristicCost = Heuristic(current, goal);
        int additionalCost = parentNode.additionalCost + AdditionalCostFunction(current);
        HexNode addNode = new HexNode(current, parentNode.fromStart + 1, heuristicCost, additionalCost);
        open.Add(addNode);
    } 

    // A node containing everything needed for pathfinding
    public struct HexNode {
        public HexagonTile currentHex;
        public int fromStart;
        public int tillFinish;
        public int additionalCost;
        public int totalCost;

        public HexNode(HexagonTile location, int costSoFar, int costTillFinish, int additional)
        {
            currentHex   = location;
            fromStart  = costSoFar;
            tillFinish = costTillFinish;
            additionalCost = additional;
            totalCost = fromStart + tillFinish + additionalCost;
        }
            
    }
    */
}
