using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTemperature : MonoBehaviour {

    public Dictionary<HexTemperature, DepthTemp> tempParents;
    private HashSet<HexTemperature> myChildren;

    public PlayingFieldController Field { get; set; }
    public HexagonTile MyHex { get; set; }
    public HeatPath Pathing { get; set; }

    public float MyHeatOutput { get; set; }
    public int HeatRange { get; set; }

    private float myTemperature;

    public bool debugCheck;

    public int DebugRange;
    public float DebugHeat;

    private Renderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = gameObject.GetComponent<Renderer>();
        tempParents = new Dictionary<HexTemperature, DepthTemp>();
        myChildren = new HashSet<HexTemperature>();
    }
	
	// This one is basically all for debugging of the heat stuff
	void Update ()
    {
        if (debugCheck)
        {
            if (myChildren.Count > 0)
            {
                List<HexTemperature> existing = UpdateChildren(DebugHeat, DebugRange);
                Pathing.HeatArea(this, MyHeatOutput, HeatRange, myChildren, existing);
            }
            else
                myChildren = Pathing.HeatArea(this, DebugHeat, DebugRange);
            debugCheck = false;
        }
		
	}

    //Used for the changes in globaltemperature, not the new absolute temperatures
    public void ModifyGlobalTemp(float temp)
    {
        myTemperature += temp;
    }

    //Adds a parent to the dictionary of parents if the parent isn't a key yet.
    //if the temperature or maxDepth parameters have changed from what was noted
    //the temperature effect on this tile is recalculated and the temperature of this tile is changed
    public void ModifyLocalTemp(HexTemperature relevantParent, int depth, int maxDepth, float temperature)
    {
        if (!tempParents.ContainsKey(relevantParent)) //A new parent is added to this tile
        {
            float newTempMod = HeatCalculation(depth, maxDepth, temperature);
            tempParents.Add(relevantParent, new DepthTemp(depth, maxDepth, temperature, newTempMod));
            myTemperature += HeatCalculation(depth, maxDepth, temperature);
        }
        else if(depth > maxDepth) //The tile falls outside the zone of tiles it can be in
        {
            myTemperature -= tempParents[relevantParent].tempMod;
            tempParents.Remove(relevantParent);
        }
        else if(maxDepth != tempParents[relevantParent].maxDepth || temperature != tempParents[relevantParent].parentTemp)
        {
            //an existing tile is altered
            float newTempMod = HeatCalculation(depth, maxDepth, temperature);
            myTemperature = (myTemperature + newTempMod) - tempParents[relevantParent].tempMod;
            tempParents[relevantParent] = new DepthTemp(depth, maxDepth, temperature, newTempMod);
        }

        //change the colour of the tile to comply with the temperature
        float tempShift = myTemperature / 40;
        Color tempColor = Color.Lerp(Color.green, Color.red, tempShift);
        renderer.material.color = tempColor;
    }

    //A semi-placeholder for the calculation of the effect of the temperature
    private float HeatCalculation(int myDepth, int maxDepth, float temp)
    {
        return ((maxDepth + 1 - myDepth) * temp )/ (maxDepth +1);
    }

    //Only called when one of the two input variables differ from what is noted in the variables
    //returns the edge of the area of children, removes children if they fall outside of the new range
    private List<HexTemperature> UpdateChildren(float newTemp, int newRange)
    {
        List<HexTemperature> output = new List<HexTemperature>();
        HashSet<HexTemperature> removal = new HashSet<HexTemperature>();
        foreach (HexTemperature child in myChildren)
        {
            if (newRange > HeatRange)
            {
                if (child.tempParents[this].myDepth == child.tempParents[this].maxDepth)
                    output.Add(child);
            }
            else if (newRange <= HeatRange)
                if (child.tempParents[this].myDepth == newRange)
                    output.Add(child);

            if (child.tempParents[this].myDepth > newRange)
                removal.Add(child);

            child.ModifyLocalTemp(this, child.tempParents[this].myDepth, newRange, newTemp);
        }
        foreach (HexTemperature removee in removal)
            myChildren.Remove(removee);

        MyHeatOutput = newTemp;
        HeatRange = newRange;
        return output;
    }

}

//A struct that stores the depth of any given tile, the maxdepth
//,the parent which radiates the heat and the temperature effect on the tile
public struct DepthTemp
{
    public int myDepth;
    public int maxDepth;
    public float parentTemp;
    public float tempMod;

    public DepthTemp(int myDepth, int maxDepth, float temperature, float calcMod)
    {
        this.myDepth = myDepth;
        this.maxDepth = maxDepth;
        parentTemp = temperature;
        tempMod = calcMod;
    }
}