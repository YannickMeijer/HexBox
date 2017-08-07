using System.Collections;
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

	private bool mouseOver;
	private string text;

	private void Start()
	{
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
		if (!mouseOver)
			return;

		Vector2 mouse = Event.current.mousePosition;
		GUI.Label(new Rect(mouse.x + 20, mouse.y + 10, 300, 100), text, style);
	}

	private void OnMouseEnter()
	{
		mouseOver = true;
	}

	private void OnMouseExit()
	{
		mouseOver = false;
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
