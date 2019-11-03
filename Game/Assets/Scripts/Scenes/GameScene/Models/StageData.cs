using System;
using UnityEngine;

[Serializable]
public class StageData
{

    public int moves = 0;
    public Operand[] operands = { };
    public Operator[] operators = { };
    public Equal[] equals = { };

}
