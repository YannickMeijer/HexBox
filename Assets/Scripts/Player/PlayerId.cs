using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerId : NetworkData
{
    [SerializeField]
    private int id;

    public PlayerId(int id)
    {
        this.id = id;
    }

    public int Id
    {
        get { return id; }
    }
}
