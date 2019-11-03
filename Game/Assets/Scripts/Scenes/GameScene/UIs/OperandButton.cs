using UnityEngine;
using UnityEngine.UI;

public class OperandButton : TermButton
{

    private Operand operand;

    public Operand Operand
    {
        get
        {
            return operand;
        }
        set
        {
            operand = value;
            GetComponentInChildren<Text>().text = operand.value.ToString();
        }
    }

    public override string GetValue()
    {
        return operand.value.ToString();
    }

    public override void SetPosition(Vector2Int position)
    {
        operand.position = position;
    }

    public override Vector2Int GetPosition()
    {
        return operand.position;
    }

    public override void SetNormalColor()
    {
        GetComponentInChildren<Image>().color = new Color(51f/255f, 123f/255f, 246f/255f);
    }

    public override void SetSelectedColor()
    {
        GetComponentInChildren<Image>().color = new Color(25f/255f, 62f/255f, 122f/255f);
    }

    

}
