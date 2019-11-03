using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{

    public static int ExecuteCalculation(List<int> operands, List<Operator.Type> operators)
    {
        var operands2 = new List<int>
            {
                operands[0]
            };
        var operators2 = new List<Operator.Type>();
        var index = 0;
        foreach (var oper in operators)
        {
            if (oper == Operator.Type.multiply)
            {
                operands2[operands2.Count - 1] = operands2[operands2.Count - 1] * operands[index + 1];
            }
            else if (oper == Operator.Type.divide)
            {
                operands2[operands2.Count - 1] = operands2[operands2.Count - 1] * operands[index + 1];
            }
            else
            {
                operands2.Add(operands[index + 1]);
                operators2.Add(oper);
            }
            ++index;
        }
        index = 0;
        var result = operands2[0];
        foreach (var oper in operators2)
        {
            if (oper == Operator.Type.add)
            {
                result += (operands2[index + 1]);
            }
            else if (oper == Operator.Type.subtract)
            {
                result -= (operands2[index + 1]);
            }
            ++index;
        }
        return result;
    }

    public bool IsCorrect
    {
        get
        {
            return hasEqual &&
                ((lhsOperands.Count - lhsOperators.Count) == 1) &&
                ((rhsOperands.Count - rhsOperators.Count) == 1) &&
                ExecuteCalculation(lhsOperands, lhsOperators) == ExecuteCalculation(rhsOperands, rhsOperators);
        }
    }

    private bool hasEqual = false;
    private List<int> lhsOperands = new List<int>();
    private List<int> rhsOperands = new List<int>();
    private List<Operator.Type> lhsOperators = new List<Operator.Type>();
    private List<Operator.Type> rhsOperators = new List<Operator.Type>();

    public Formula()
    {

    }

    public Formula(Formula other)
    {
        hasEqual = other.hasEqual;
        lhsOperands = new List<int>(other.lhsOperands);
        rhsOperands = new List<int>(other.rhsOperands);
        lhsOperators = new List<Operator.Type>(other.lhsOperators);
        rhsOperators = new List<Operator.Type>(other.rhsOperators);
    }

    //public void Reverse()
    //{
    //    lhsOperands.Reverse();
    //    rhsOperands.Reverse();
    //    lhsOperators.Reverse();
    //    rhsOperators.Reverse();
    //}

    public void Clear()
    {
        lhsOperands.Clear();
        rhsOperands.Clear();
        lhsOperators.Clear();
        rhsOperators.Clear();
        hasEqual = false;
    }

    public bool CanAppendTermButton(TermButton termButton)
    {
        var operandButton = termButton as OperandButton;
        if (operandButton != null)
        {
            return CanAppendOperand(operandButton.Operand);
        } else
        {
            var operatorButton = termButton as OperatorButton;
            if (operatorButton != null)
            {
                return CanAppendOperator(operatorButton.Operator);
            } else
            {
                var equalButton = termButton as EqualButton;
                if (equalButton != null)
                {
                    return CanAppendEqual(equalButton.equal);
                } else
                {
                    return false;
                }
            }
        }
    }

    public bool CanAppendOperand(Operand operand)
    {
        if (hasEqual)
        {
            return rhsOperands.Count == rhsOperators.Count;
        } else
        {
            return lhsOperands.Count == lhsOperators.Count;
        }
    }

    public bool CanAppendOperator(Operator oper)
    {
        if (hasEqual)
        {
            return (rhsOperands.Count - rhsOperators.Count) == 1;
        } else
        {
            return (lhsOperands.Count - lhsOperators.Count) == 1;
        }
    }

    public bool CanAppendEqual(Equal equal)
    {
        if (hasEqual)
        {
            return false;
        }
        return (lhsOperands.Count - lhsOperators.Count) == 1;
    }

    public bool AppendTermButton(TermButton termButton)
    {
        if (!CanAppendTermButton(termButton))
        {
            return false;
        }
        var operandButton = termButton as OperandButton;
        if (operandButton != null)
        {
            return AppendOperand(operandButton.Operand);
        }
        else
        {
            var operatorButton = termButton as OperatorButton;
            if (operatorButton != null)
            {
                return AppendOperator(operatorButton.Operator);
            }
            else
            {
                var equalButton = termButton as EqualButton;
                if (equalButton != null)
                {
                    return AppendEqual(equalButton.equal);
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public bool AppendOperand(Operand operand)
    {
        if (!CanAppendOperand(operand))
        {
            return false;
        }
        if (hasEqual)
        {
            rhsOperands.Add(operand.value);
        } else
        {
            lhsOperands.Add(operand.value);
        }
        return true;
    }

    public bool AppendOperator(Operator oper)
    {
        if (!CanAppendOperator(oper))
        {
            return false;
        }
        if (hasEqual)
        {
            rhsOperators.Add(oper.type);
        } else
        {
            lhsOperators.Add(oper.type);
        }
        return true;
    }

    public bool AppendEqual(Equal equal)
    {
        if(!CanAppendEqual(equal))
        {
            return false;
        }
        hasEqual = true;
        return true;
    }

}
