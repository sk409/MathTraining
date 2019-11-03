using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ClearedStages {

    

    public static void Reset()
    {
        var empty = new ClearedStages();
        empty.Write();
    }

    public static ClearedStages Load()
    {
        var json = File.ReadAllText(ResourceNames.clearedStages);
        return JsonUtility.FromJson<ClearedStages>(json);
    }

    [SerializeField]
    public List<int> stageIndices;

    public void Write()
    {
        var json = JsonUtility.ToJson(this);
        File.WriteAllText(ResourceNames.clearedStages, json);
    }

}

