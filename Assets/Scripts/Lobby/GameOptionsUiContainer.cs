using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsUiContainer : MonoBehaviour
{
    public Dropdown WorldShapeDropdown;
    public Slider WorldSizeSlider;
    public Slider PlayerCountSlider;

    private void Start()
    {
        // Set the options for the world shapes.
        List<WorldShape> shapes = Enum.GetValues(typeof(WorldShape)).Cast<WorldShape>().ToList();
        shapes.Sort(); // Maybe not necessary?
        foreach (WorldShape shape in shapes)
        {
            string name = shape.ToString();
            WorldShapeDropdown.options.Add(new Dropdown.OptionData(name));
        }
        WorldShapeDropdown.RefreshShownValue();

    }

    /// <summary>
    /// Apply game options to the lobby, setting the UI elements accordingly.
    /// </summary>
    /// <param name="options">The new game options.</param>
    public void ApplyOptions(GameOptions options)
    {
        WorldShapeDropdown.value = (int)options.WorldShape;
        WorldSizeSlider.value = options.WorldSize;
        PlayerCountSlider.value = options.PlayerCount;
    }
}
