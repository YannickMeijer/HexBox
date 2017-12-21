﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GUIStyle style;

    [SerializeField]
    private string tooltipName;
    [SerializeField]
    private string description;

    private MouseHelper mouseHelper;
    private string text;

    private void Start()
    {
        mouseHelper = GetComponent<MouseHelper>();
        UpdateText();
    }

    private void UpdateText()
    {
        // Build the tooltip text.
        text = new StringBuilder()
            .Append("<b>").Append(tooltipName).AppendLine("</b>")
            .Append(description)
            .ToString();
    }

    private void OnGUI()
    {
        if (!mouseHelper.IsMouseOver)
            return;

        Vector2 mouse = Event.current.mousePosition;
        GUI.Label(new Rect(mouse.x + 20, mouse.y + 10, 300, 100), text, style);
    }

    public string TooltipName
    {
        get { return tooltipName; }
        set
        {
            tooltipName = value;
            UpdateText();
        }
    }

    public string Description
    {
        get { return description; }
        set
        {
            description = value;
            UpdateText();
        }
    }
}
