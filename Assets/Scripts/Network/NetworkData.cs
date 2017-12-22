using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NetworkData
{
    [SerializeField]
    private string type;

    public NetworkData()
    {
        type = GetType().Name;
    }

    public string Type
    {
        get { return type; }
    }
}

[Serializable]
public class TextNetworkData : NetworkData
{
    [SerializeField]
    private string text;

    public TextNetworkData(string text)
    {
        this.text = text;
    }

    public override string ToString()
    {
        return text;
    }
}
