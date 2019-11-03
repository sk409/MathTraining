using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertImage : Image
{
    
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
            transform.Find(ResourceNames.alertTitleImage).GetComponentInChildren<Text>().text = value;
        }
    }
    public string Message
    {
        get
        {
            return message;
        }
        set
        {
            message = value;
            transform.Find(ResourceNames.alertMessageImage).GetComponentInChildren<Text>().text = value;
        }
    }

    private string title = "";
    private string message = "";
    private GameObject buttonsStackImageObject;
    private Image buttonsStackImage;
    private readonly List<GameObject> buttonObjects = new List<GameObject>();

    public void AppendButton(string title, Func<Button, bool> onClick, float alpha = 0f)
    {
        var buttonsStackImage = transform.Find(ResourceNames.alertButtonsStackImage).gameObject;
        var buttonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.button));
        buttonObject.transform.SetParent(buttonsStackImage.transform, false);
        var button = buttonObject.GetComponent<Button>();
        var buttonImage = button.GetComponentInChildren<Image>();
        var buttonImageColor = buttonImage.color;
        buttonImageColor.a = alpha;
        buttonImage.color = buttonImageColor;
        var buttonText = button.GetComponentInChildren<Text>();
        var buttonTextColor = buttonText.color;
        buttonTextColor.a = alpha;
        buttonText.color = buttonTextColor;
        buttonText.text = title;
        button.onClick.AddListener(() =>
        {
            onClick(button);
        });
        buttonObjects.Add(buttonObject);
        FitButtonObjects();
    }

    protected override void Start()
    {
        base.Start();
        var alertImageRectTransform = GetComponent<RectTransform>();
        var childWidth = alertImageRectTransform.sizeDelta.x * 0.8f;
        var margin = alertImageRectTransform.sizeDelta.y * 0.05f;
        var apparentHeight = alertImageRectTransform.sizeDelta.y - margin * 4f;
        var titleImage = transform.Find(ResourceNames.alertTitleImage);
        var titleImageRectTransform = titleImage.GetComponent<RectTransform>();
        titleImageRectTransform.anchoredPosition = new Vector2(0f, -margin);
        titleImageRectTransform.sizeDelta = new Vector2(childWidth, apparentHeight * 0.2f);
        var messageImage = transform.Find(ResourceNames.alertMessageImage);
        var messageImageRectTransform = messageImage.GetComponent<RectTransform>();
        messageImageRectTransform.anchoredPosition = new Vector2(0f, -margin * 2f - titleImageRectTransform.sizeDelta.y);
        messageImageRectTransform.sizeDelta = new Vector2(childWidth, apparentHeight * 0.6f);
        buttonsStackImageObject = transform.Find(ResourceNames.alertButtonsStackImage).gameObject;
        buttonsStackImage = buttonsStackImageObject.GetComponent<Image>();
        var buttonsStackImageRectTransform = buttonsStackImageObject.GetComponent<RectTransform>();
        buttonsStackImageRectTransform.anchoredPosition = new Vector2(0f, -margin * 3f - titleImageRectTransform.sizeDelta.y - messageImageRectTransform.sizeDelta.y);
        buttonsStackImageRectTransform.sizeDelta = new Vector2(childWidth, apparentHeight * 0.2f);
        FitButtonObjects();
    }

    private void Update()
    {
        var buttonsStackImageColor = buttonsStackImage.color;
        buttonsStackImageColor.a = 0;
        buttonsStackImage.color = buttonsStackImageColor;
    }

    private void FitButtonObjects()
    {
        var buttonsStackImage = buttonsStackImageObject;
        if (buttonsStackImage == null)
        {
            buttonsStackImage = transform.Find(ResourceNames.alertButtonsStackImage).gameObject;
        }
        var stackRectTransform = buttonsStackImage.GetComponent<RectTransform>();
        if (buttonObjects.Count == 1)
        {
            var rectTransform = buttonObjects[0].GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(stackRectTransform.sizeDelta.x * 0.6f, stackRectTransform.sizeDelta.y * 0.8f);
            return;
        }
        var margin = 8f;
        var padding = 8f;
        var buttonWidth = (stackRectTransform.sizeDelta.x - (margin * 2) - (padding * (buttonObjects.Count - 1))) / buttonObjects.Count;
        var index = 0;
        foreach (var buttonObject in buttonObjects)
        {
            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 0.5f);
            rectTransform.anchorMax = new Vector2(0f, 0.5f);
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(margin + buttonWidth * index + padding * index, 0);
            rectTransform.sizeDelta = new Vector2(buttonWidth, stackRectTransform.sizeDelta.y);
            ++index;
        }
    }

}
