using UnityEngine;

public class SystemButton : AppButton
{

    protected override void Start()
    {
        base.Start();
        onClick.AddListener(() =>
        {
            SoundWareHouse.Instance.seSystemButton.GetComponent<AudioSource>().Play();
        });
    }

}
