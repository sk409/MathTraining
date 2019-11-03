using System;
using System.Collections.Generic;
using UnityEngine;

public class CurtainManager : MonoBehaviour
{

    private UIAnimator uiAnimator;
    private readonly List<Sprite> openSprites = new List<Sprite>();
    private readonly List<Sprite> closeSprites = new List<Sprite>();

    public void Open(Func<bool> completion, float animationDuration = 1f)
    {
        Animate(completion, animationDuration, openSprites);
    }

    public void Close(Func<bool> completion, float animationDuration = 1f)
    {
        Animate(completion, animationDuration, closeSprites);
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (uiAnimator == null)
        {
            uiAnimator = GetComponent<UIAnimator>();
        }
        void addSprites(string key, int count, List<Sprite> list)
        {
            for (var index = 1; index <= count; ++index)
            {
                var path = "Images/curtain-" + key + "-" + index.ToString();
                list.Add(Resources.Load<Sprite>(path));
            }

        }
        if (openSprites.Count == 0)
        {
            addSprites("open", 11, openSprites);
        }
        if (closeSprites.Count == 0)
        {
            addSprites("close", 18, closeSprites);
        }
        //addSprites("close", 18);
    }

    private void Animate(Func<bool> completion, float animationDuration, List<Sprite> sprites)
    {
        Setup();
        uiAnimator.Animate(UIAnimator.AnimationKey.Image, animationDuration, sprites, (animator) =>
        {
            completion?.Invoke();
            return true;
        });
    }

    

}
