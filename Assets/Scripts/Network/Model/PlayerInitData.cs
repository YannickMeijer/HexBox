using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitData : NetworkData
{
    [SerializeField]
    private int id;

    public PlayerInitData(int id)
    {
        this.id = id;
    }

    public int Id
    {
        get { return id; }
    }
}
