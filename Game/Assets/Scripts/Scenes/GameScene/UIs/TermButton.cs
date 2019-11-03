using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TermButton : AppButton
{

    

    public bool isSelected = false;

    public abstract string GetValue();
    public abstract void SetPosition(Vector2Int position);
    public abstract Vector2Int GetPosition();
    public abstract void SetNormalColor();
    public abstract void SetSelectedColor();

    protected override void Start()
    {
        base.Start();
        //ButtonType = Type.term;
        //onClick.RemoveAllListeners();
    }

}
