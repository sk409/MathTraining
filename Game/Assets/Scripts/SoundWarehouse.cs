using System.Collections.Generic;
using UnityEngine;

public class SoundWareHouse
{

    public readonly static SoundWareHouse Instance = new SoundWareHouse();

    private static AudioSource MakeSound(string soundName, bool isBGM = false)
    {
        var soundObject = new GameObject() { name = isBGM ? "BGM" : "SE" };
        Object.DontDestroyOnLoad(soundObject);
        var audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(soundName);
        audioSource.volume = isBGM ? SoundVolums.BGMVolume : SoundVolums.SEVolume;
        audioSource.priority = isBGM ? 0 : 255;
        audioSource.loop = isBGM;
        return audioSource;
    }

    public readonly List<AudioSource> bgms = new List<AudioSource>();
    public readonly List<AudioSource> ses = new List<AudioSource>();

    public readonly AudioSource bgmNormal = MakeSound(ResourceNames.bgmNormal, true);
    public readonly AudioSource bgmGame = MakeSound(ResourceNames.bgmGame, true);
    public readonly AudioSource seButtonDropped = MakeSound(ResourceNames.seButtonDropped);
    public readonly AudioSource seButtonError = MakeSound(ResourceNames.seButtonError);
    public readonly AudioSource seControlButton = MakeSound(ResourceNames.seControlButton);
    public readonly AudioSource seGameClear = MakeSound(ResourceNames.seGameClear);
    public readonly AudioSource seRemoveButton = MakeSound(ResourceNames.seRemoveButton);
    public readonly AudioSource seSystemButton = MakeSound(ResourceNames.seSystemButton);
    public readonly AudioSource seSpecialButton = MakeSound(ResourceNames.seSpecialButton);
    public readonly AudioSource seTermButton = MakeSound(ResourceNames.seTermButton);

    public void StopAllBGM()
    {
        foreach (var bgm in bgms)
        {
            bgm.Stop();
        }
    }

    private SoundWareHouse()
    {
        bgms.Add(bgmNormal);
        bgms.Add(bgmGame);
        ses.Add(seButtonDropped);
        ses.Add(seButtonError);
        ses.Add(seControlButton);
        ses.Add(seGameClear);
        ses.Add(seRemoveButton);
        ses.Add(seSystemButton);
        ses.Add(seSpecialButton);
        ses.Add(seTermButton);
    }

}
