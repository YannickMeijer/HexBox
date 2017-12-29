using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script sets text based on the value of a slider.
/// </summary>
public class SliderValueText : MonoBehaviour
{
    public Text ValueText;

    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        // Initialize the text, hook into the value changed event.
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        ValueText.text = value.ToString();
    }
}
