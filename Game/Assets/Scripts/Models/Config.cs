using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Config
{

    public static Config Instance = Load();

    private static Config Load()
    {
        var json = File.ReadAllText(ResourceNames.config);
        return JsonUtility.FromJson<Config>(json);
    }

    public float bgmVolume;
    public float seVolume;

    public void Write()
    {
        var json = JsonUtility.ToJson(this);
        File.WriteAllText(ResourceNames.config, json);
    }

    private Config()
    {

    }
    
}
