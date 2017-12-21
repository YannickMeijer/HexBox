using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CardInfo
{
    [SerializeField]
    private string name;
    [SerializeField]
    private string description;

    public virtual void Apply(GameObject gameObject)
    {
        Card card = gameObject.GetComponent<Card>();
        card.name = name;

        Tooltip tooltip = gameObject.GetComponent<Tooltip>();
        tooltip.TooltipName = name;
        tooltip.Description = description;
    }

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }
}
