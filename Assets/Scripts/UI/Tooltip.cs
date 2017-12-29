using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GUIStyle style;

    [SerializeField]
    private string text;
    private MouseHelper mouseHelper;

    private void Start()
    {
        mouseHelper = GetComponent<MouseHelper>();
    }

    private void OnGUI()
    {
        if (!mouseHelper.IsMouseOver)
            return;

        // Create a label at the current mouse position.
        Vector2 mouse = Event.current.mousePosition;
        GUI.Label(new Rect(mouse.x + 20, mouse.y + 10, 300, 100), text, style);
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }
}
