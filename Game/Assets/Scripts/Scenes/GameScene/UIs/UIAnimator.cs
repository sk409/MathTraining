using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{

    public struct AnimationKey
    {
        public static string None = "None";
        public static string PositionY = "PositionY";
        public static string Size = "Size";
        public static string Color = "Color";
        public static string Alpha = "Alpha";
        public static string Image = "Image";
    }

    private string key = AnimationKey.None;
    private float duration = 0f;
    private float elapsedTime = 0f;
    private object fromValue;
    private object toValue;
    private Func<UIAnimator, bool> completion;

    private RectTransform rectTransform;
    private readonly List<object> imageFromValues = new List<object>();
    private readonly List<Image> images = new List<Image>();
    private readonly List<object> textFromValues = new List<object>();
    private readonly List<Text> texts = new List<Text>();

    public void Animate(string key, float duration, object value, Func<UIAnimator, bool> completion)
    {
        this.key = key;
        this.duration = duration;
        toValue = value;
        this.completion = completion;
        elapsedTime = 0;
        imageFromValues.Clear();
        images.Clear();
        textFromValues.Clear();
        texts.Clear();
        if (key == AnimationKey.PositionY)
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            fromValue = rectTransform.anchoredPosition.y;
        }
        else if (key == AnimationKey.Size)
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            fromValue = rectTransform.sizeDelta;
        }
        else if (key == AnimationKey.Color)
        {
            AddImages();
            foreach (var image in images)
            {
                imageFromValues.Add(image.color);
            }
            AddTexts();
            foreach (var text in texts)
            {
                textFromValues.Add(text.color);
            }
        } else if (key == AnimationKey.Alpha)
        {
            AddImages();
            foreach (var image in images)
            {
                imageFromValues.Add(image.color.a);
            }
            AddTexts();
            foreach (var text in texts)
            {
                textFromValues.Add(text.color.a);
            }
        } else if (key == AnimationKey.Image)
        {
            fromValue = GetComponent<Image>();
        }
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AddImages();
        AddTexts();
    }

    private void Update()
    {
        if (key == AnimationKey.None)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        elapsedTime = Mathf.Min(elapsedTime, duration);
        if (key == AnimationKey.PositionY)
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = Convert.ToInt32(fromValue) + (Convert.ToInt32(toValue) - Convert.ToInt32(fromValue)) * (elapsedTime / duration);
            rectTransform.anchoredPosition = anchoredPosition;
        }
        else if (key == AnimationKey.Size)
        {
            var toSize = (Vector2)toValue;
            var fromSize = (Vector2)fromValue;
            rectTransform.sizeDelta = fromSize + ((toSize - fromSize) * elapsedTime / duration);
        }
        else if (key == AnimationKey.Color)
        {
            var toColor = (Color)toValue;
            var index = 0;
            foreach (var image in images)
            {
                var fromColor = (Color)imageFromValues[index];
                image.color = fromColor + (toColor - fromColor) * elapsedTime / duration;
                ++index;
            }
            index = 0;
            foreach (var text in texts)
            {
                var fromColor = (Color)textFromValues[index];
                text.color = fromColor + (toColor - fromColor) * elapsedTime / duration;
                ++index;
            }
        } else if (key == AnimationKey.Alpha)
        {
            var toAlpha = (float)toValue;
            var index = 0;
            foreach (var image in images)
            {
                var fromAlpha = (float)imageFromValues[index];
                var color = image.color;
                color.a = fromAlpha + (toAlpha - fromAlpha) * elapsedTime / duration;
                image.color = color;
                ++index;
            }
            index = 0;
            foreach (var text in texts)
            {
                var fromAlpha = (float)textFromValues[index];
                var color = text.color;
                color.a = fromAlpha + (toAlpha - fromAlpha) * elapsedTime / duration;
                text.color = color;
                ++index;
            }
        } else if (key == AnimationKey.Image)
        {
            var sprites = (List<Sprite>)(toValue);
            var index = (int)Mathf.Floor((sprites.Count - 1) * elapsedTime / duration);
            GetComponent<Image>().sprite = sprites[index];
        }
        if (duration <= elapsedTime)
        {
            key = AnimationKey.None;
            completion?.Invoke(this);
        }
    }

    private void AddImages()
    {
        if (images.Count != 0)
        {
            return;
        }
        var image = GetComponent<Image>();
        if (image != null)
        {
            images.Add(image);
        }
        var subtransforms = GetSubtransforms();
        foreach (var subtransform in subtransforms)
        {
            var i = subtransform.GetComponent<Image>();
            if (i == null)
            {
                continue;
            }
            images.Add(i);
        }
    }

    private void AddTexts()
    {
        if (texts.Count != 0)
        {
            return;
        }
        var text = GetComponent<Text>();
        if (text != null)
        {
            texts.Add(text);
        }
        var subtransforms = GetSubtransforms();
        foreach (var subtransform in subtransforms)
        {
            var t = subtransform.GetComponent<Text>();
            if (t == null)
            {
                continue;
            }
            texts.Add(t);
        }
    }

    private List<Transform> GetSubtransforms(GameObject gameObject = null, List<Transform> subtransforms = null)
    {
        if (gameObject == null)
        {
            gameObject = this.gameObject;
        }
        if (subtransforms == null)
        {
            subtransforms = new List<Transform>();
        }
        subtransforms.Add(gameObject.transform);
        var children = gameObject.GetComponentInChildren<Transform>();
        foreach (Transform child in children)
        {
            GetSubtransforms(child.gameObject, subtransforms);
        }
        return subtransforms;
    }

}
