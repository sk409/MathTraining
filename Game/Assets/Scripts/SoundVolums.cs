using UnityEngine;

public class SoundVolums
{

    public static readonly float minVolume = 0f;
    public static readonly float maxVolume = 1f;

    public static float BGMVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = Mathf.Clamp(value, minVolume, maxVolume);
            Config.Instance.bgmVolume = bgmVolume;
            Config.Instance.Write();
            foreach(var bgm in SoundWareHouse.Instance.bgms)
            {
                bgm.volume = bgmVolume;
            }
        }
    }

    public static float SEVolume
    {
        get
        {
            return seVolume;
        }
        set
        {
            seVolume = Mathf.Clamp(value, minVolume, maxVolume);
            Config.Instance.seVolume = seVolume;
            Config.Instance.Write();
            foreach (var se in SoundWareHouse.Instance.ses)
            {
                se.volume = seVolume;
            }
        }
    }

    private static float bgmVolume = Config.Instance.bgmVolume;
    private static float seVolume = Config.Instance.seVolume;

}
