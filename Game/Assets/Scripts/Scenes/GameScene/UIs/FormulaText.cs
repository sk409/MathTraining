using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormulaText : Text
{
    
    public void AppendOperand(Operand operand)
    {
        text += (operand.value.ToString() + " ");
    }
    
    public void AppendOperator(Operator oper)
    {
        switch (oper.type)
        {
            case Operator.Type.add:
                text += "+";
                break;
            case Operator.Type.subtract:
                text += "-";
                break;
            case Operator.Type.multiply:
                text += "×";
                break;
            case Operator.Type.divide:
                text += "÷";
                break;
        }
        text += " ";
    }

    public void AppendEqual()
    {
        text += "= ";
    }

}
