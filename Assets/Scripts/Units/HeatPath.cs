using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatPath : MonoBehaviour {
    public PlayingFieldController Controller { get; set; }

    //all nodes still unchecked for the path
    List<HexTemperature> open;
    //contains all nodes already checked.
    HashSet<HexTemperature> checkedNodes;
    //A list containing every value of the HexagonDirection Enum for easy iteration

    //Returns all tiles which can be reached from the start tile with the range of maxDepth
    public HashSet<HexTemperature> HeatArea(HexTemperature start, float temperature, int maxDepth, HashSet<HexTemperature> existing = null, List<HexTemperature> border = null)
    {
        open = new List<HexTemperature>();
        checkedNodes = new HashSet<HexTemperature>();
        HashSet<HexTemperature> output = new HashSet<HexTemperature>();

        if (border == null)
        {
            open.Add(start);
            checkedNodes.Add(start);
            output.Add(start);
            start.ModifyLocalTemp(start, 0, maxDepth, temperature);
        }
        else
        {
            open = border;
            checkedNodes = existing;
        }

        int currentDepth = 0;

        //Take the current node, find its neighbours and update their temperature
        for (int x = 0; x < open.Count; x++)
        {
            List<HexagonTile> neighbours = new List<HexagonTile>();
            neighbours = Controller.GetAllNeighbours(open[x].MyHex);
            currentDepth = 1 + open[x].tempParents[start].myDepth;

            //add the 6 neighbours of the node that's currently being checked.
            foreach (HexagonTile futureTile in neighbours)
            {

                if (futureTile == null || checkedNodes.Contains(futureTile.MyTemp))
                    continue;
                else
                {
                    HexTemperature futureTemp = futureTile.MyTemp;
                    if (currentDepth + 1 <= maxDepth && !futureTile.blocked)
                        open.Add(futureTemp);

                    output.Add(futureTemp);
                    futureTemp.ModifyLocalTemp(start, currentDepth, maxDepth, temperature);
                    checkedNodes.Add(futureTemp);

                }
            }
        }
        return output;
    }
}