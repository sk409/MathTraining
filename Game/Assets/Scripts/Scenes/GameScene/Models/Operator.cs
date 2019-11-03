using System;
using UnityEngine;

[Serializable]
public class Operator
{
    public enum Type
    {
        add,
        subtract,
        multiply,
        divide,
    }

    public Type type = Type.add;
    public Vector2Int position = Vector2Int.zero;

}
