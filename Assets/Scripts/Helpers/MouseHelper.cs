using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHelper : MonoBehaviour
{
    public delegate void OnClickHandler();
    public event OnClickHandler OnClick;

    private void OnMouseEnter()
    {
        IsMouseOver = true;
    }

    private void OnMouseExit()
    {
        IsMouseOver = false;
    }

    private void OnMouseUpAsButton()
    {
        if (OnClick != null)
            OnClick();
    }

    public bool IsMouseOver
    {
        get;
        private set;
    }
}
