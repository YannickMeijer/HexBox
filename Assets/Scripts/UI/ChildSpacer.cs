using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSpacer : MonoBehaviour
{
    public float Margin;

    private void Start()
    {
        float y = GetComponent<RectTransform>().rect.yMax;
        foreach (Transform child in transform)
        {
            // Subtract half the height to align with the top.
            y -= child.GetComponent<RectTransform>().rect.height / 2;

            // Set the child position.
            Vector3 childPos = child.localPosition;
            childPos.y = y;
            child.localPosition = childPos;

            y -= Margin;
        }
    }
}
