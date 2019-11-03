using UnityEngine;
using UnityEngine.UI;

public class AppButton : Button
{


    protected override void Start()
    {
        base.Start();
        if (transition == Transition.ColorTint)
        {
            var colors = this.colors;
            colors.normalColor = new Color(1f, 1f, 1f);
            colors.highlightedColor = new Color(0.8f, 0.8f, 0.8f);
            colors.pressedColor = new Color(0.7f, 0.7f, 0.7f);
            colors.selectedColor = new Color(1f, 1f, 1f);
            this.colors = colors;
        }
    }



}
