using System.IO;
using UnityEngine;

public class StageManager
{

    private static string[] stages;

    public static string[] Stages
    {
        get
        {
            if (stages == null)
            {
                stages = Directory.GetFiles(ResourceNames.stageDatasDirectory, "*.json");
            }
            return stages;
        }
    }

    public static StageData LoadData(int index)
    {
        var stageDataJson = File.ReadAllText(ResourceNames.stageDatasDirectory + "/stage" + index.ToString() + ".json");
        return JsonUtility.FromJson<StageData>(stageDataJson);
    }

}
