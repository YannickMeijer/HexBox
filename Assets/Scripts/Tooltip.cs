using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
	public GUIStyle Style;
	public string Name;
	public string Description;

	private bool mouseOver;
	private string text;

	private void Start()
	{
		// Build the tooltip text.
		text = new StringBuilder()
			.Append("<b>").Append(Name).AppendLine("</b>")
			.Append(Description)
			.ToString();
	}

	private void OnGUI()
	{
		if (!mouseOver)
			return;

		Debug.Log("Gui");
		Vector2 mouse = Event.current.mousePosition;
		GUI.Label(new Rect(mouse.x + 20, mouse.y + 10, 300, 100), text, Style);
	}

	private void OnMouseEnter()
	{
		mouseOver = true;
	}

	private void OnMouseExit()
	{
		mouseOver = false;
	}
}
