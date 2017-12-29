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
        GetComponent<Slider>().onValueChanged.AddListener(value => ValueText.text = value.ToString());
    }
}
