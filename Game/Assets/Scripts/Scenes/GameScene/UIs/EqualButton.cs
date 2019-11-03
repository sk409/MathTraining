using UnityEngine;
using UnityEngine.UI;

public class EqualButton : TermButton
{

    public Equal equal;

    public override string GetValue()
    {
        return "=";
    }

    public override void SetPosition(Vector2Int position)
    {
        equal.position = position;
    }

    public override Vector2Int GetPosition()
    {
        return equal.position;
    }

    public override void SetNormalColor()
    {
        GetComponentInChildren<Image>().color = new Color(246f/255f, 51f/255f, 123f/255f);
    }

    public override void SetSelectedColor()
    {
        GetComponentInChildren<Image>().color = new Color(122f/255f, 25f/255f, 62f/255f);
    }

}
