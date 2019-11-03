using System.Collections.Generic;

public class SnapShot
{

    public int moves;
    public string formulaText;
    public Formula formula;
    public readonly List<SnapShotData> datas = new List<SnapShotData>();

    public StageData ToStageData(int moves)
    {
        var operands = new List<Operand>();
        var operators = new List<Operator>();
        var equals = new List<Equal>();
        foreach (var data in datas)
        {
            if (int.TryParse(data.value, out int value))
            {
                var operand = new Operand
                {
                    value = value,
                    position = data.position
                };
                operands.Add(operand);
            }
            else if (data.value == "+")
            {
                var oper = new Operator
                {
                    type = Operator.Type.add,
                    position = data.position
                };
                operators.Add(oper);
            }
            else if (data.value == "-")
            {
                var oper = new Operator
                {
                    type = Operator.Type.subtract,
                    position = data.position
                };
                operators.Add(oper);
            }
            else if (data.value == "×")
            {
                var oper = new Operator
                {
                    type = Operator.Type.multiply,
                    position = data.position
                };
                operators.Add(oper);
            }
            else if (data.value == "÷")
            {
                var oper = new Operator
                {
                    type = Operator.Type.divide,
                    position = data.position
                };
                operators.Add(oper);
            }
            else if (data.value == "=")
            {
                var equal = new Equal
                {
                    position = data.position
                };
                equals.Add(equal);
            }
        }
        var stageData = new StageData
        {
            moves = moves,
            operands = operands.ToArray(),
            operators = operators.ToArray(),
            equals = equals.ToArray()
        };
        return stageData;
    }

}
