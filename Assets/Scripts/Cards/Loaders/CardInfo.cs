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

    /// <summary>
    /// Apply this card to a game object.
    /// </summary>
    /// <param name="gameObject">The game object to apply to.</param>
    public virtual void Apply(GameObject gameObject)
    {
        Card card = gameObject.GetComponent<Card>();
        card.name = name;

        Tooltip tooltip = gameObject.GetComponent<Tooltip>();
        tooltip.TooltipName = name;
        tooltip.Description = description;
    }
}
