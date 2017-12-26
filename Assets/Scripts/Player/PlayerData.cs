using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : NetworkData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;

    public PlayerData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
    }
}
