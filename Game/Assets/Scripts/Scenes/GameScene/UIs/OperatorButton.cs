using UnityEngine;
using UnityEngine.UI;

public class OperatorButton : TermButton
{

    private Operator oper;

    public Operator Operator
    {
        get
        {
            return oper;
        }
        set
        {
            oper = value;
            var text = "";
            switch (oper.type)
            {
                case Operator.Type.add:
                    text = "+";
                    break;
                case Operator.Type.subtract:
                    text = "-";
                    break;
                case Operator.Type.multiply:
                    text = "×";
                    break;
                case Operator.Type.divide:
                    text = "÷";
                    break;
            }
            GetComponentInChildren<Text>().text = text;
        }
    }

    public override string GetValue()
    {
        switch (Operator.type)
        {
            case Operator.Type.add:
                return "+";
            case Operator.Type.subtract:
                return "-";
            case Operator.Type.multiply:
                return "×";
            case Operator.Type.divide:
                return "÷";
        }
        return "";
    }

    public override void SetPosition(Vector2Int position)
    {
        oper.position = position;
    }

    public override Vector2Int GetPosition()
    {
        return oper.position;
    }

    public override void SetNormalColor()
    {
        GetComponentInChildren<Image>().color = new Color(123f/255f, 246f/255f, 51f/255f);
    }

    public override void SetSelectedColor()
    {
        GetComponentInChildren<Image>().color = new Color(62f/255f, 122f/255f, 25f/255f);
    }

    

}
