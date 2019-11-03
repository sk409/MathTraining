using UnityEngine;
using UnityEngine.UI;

public class ConfigViewFactory {

    public static GameObject Make(GameObject canvasObject)
    {

        float setupComponent(float y, GameObject textObject, GameObject inputFieldObject, GameObject sliderObject)
        {
            var marginLeft = Screen.width * 0.1f;
            var marginRight = Screen.width * 0.1f;
            var padding = Screen.width * 0.05f;
            var inputFieldHeight = Screen.height * 0.1f;
            var x = marginLeft;
            var text = textObject.GetComponent<Text>();
            text.fontSize = FontSizes.Medium;
            var textWidth = Screen.width * 0.2f;
            var textRectTransform = text.GetComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(textWidth, text.preferredHeight);
            textRectTransform.sizeDelta = new Vector2(textWidth, text.preferredHeight);
            textRectTransform.anchoredPosition = new Vector2(
                x,
                y - (inputFieldHeight / 2f) + (textRectTransform.sizeDelta.y / 2f)
                );
            var w = Screen.width - marginLeft - marginRight - (padding * 2f) - textRectTransform.sizeDelta.x;
            var inputFieldWidth = w * 0.3f;
            x += (textRectTransform.sizeDelta.x + padding);
            var inputFieldRectTransform = inputFieldObject.GetComponent<RectTransform>();
            inputFieldRectTransform.anchoredPosition = new Vector2(x, y);
            inputFieldRectTransform.sizeDelta = new Vector2(
                inputFieldWidth, inputFieldHeight
                );
            x += (inputFieldRectTransform.sizeDelta.x + padding);
            var sliderWidth = w - inputFieldWidth;
            var sliderHeight = sliderWidth / 8f;
            var sliderRectTransform = sliderObject.GetComponent<RectTransform>(); ;
            sliderRectTransform.anchoredPosition = new Vector2(x, y);
            sliderRectTransform.sizeDelta = new Vector2(sliderWidth, sliderHeight);
            return y - inputFieldHeight - (Screen.height * 0.1f);
        }

        var configViewObject = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.configView));
        configViewObject.transform.SetParent(canvasObject.transform, false);
        var bgmTextObject = configViewObject.transform.Find(ResourceNames.bgmText).gameObject;
        var bgmInputFieldObject = configViewObject.transform.Find(ResourceNames.bgmInputField).gameObject;
        var bgmSliderObject = configViewObject.transform.Find(ResourceNames.bgmSlider).gameObject;
        var seTextObject = configViewObject.transform.Find(ResourceNames.seText).gameObject;
        var seInputFieldObject = configViewObject.transform.Find(ResourceNames.seInputField).gameObject;
        var seSliderObject = configViewObject.transform.Find(ResourceNames.seSlider).gameObject;

        void InputFieldOnValueChanged(InputField inputField, Slider slider, string newString, bool isBGM)
        {
            if (float.TryParse(newString, out float newValue))
            {
                if (newValue < SoundVolums.minVolume || SoundVolums.maxVolume < newValue)
                {
                    newValue = Mathf.Clamp(newValue, SoundVolums.minVolume, SoundVolums.maxVolume);
                    inputField.text = newValue.ToString();
                }
                if (isBGM)
                {
                    SoundVolums.BGMVolume = newValue;
                } else
                {
                    SoundVolums.SEVolume = newValue;
                }
                
                if (slider.value != newValue)
                {
                    slider.value = newValue;
                }
            }
        }

        void SliderOnValueChanged(InputField inputField, Slider slider, float newValue, bool isBGM)
        {
            slider.value = newValue;
            if (isBGM)
            {
                SoundVolums.BGMVolume = newValue;
            } else
            {
                SoundVolums.SEVolume = newValue;
            }
            if (float.TryParse(inputField.text, out float inputFieldValue))
            {
                if (inputFieldValue != newValue)
                {
                    inputField.text = newValue.ToString();
                }
            }
        }

        var bgmText = bgmTextObject.GetComponent<Text>();
        bgmText.text += ("(" + SoundVolums.minVolume.ToString() + "~" + SoundVolums.maxVolume.ToString() + ")");
        var bgmInputField = bgmInputFieldObject.GetComponent<InputField>();
        var bgmSlider = bgmSliderObject.GetComponent<Slider>();
        bgmInputField.text = SoundVolums.BGMVolume.ToString();
        var bgmInputFieldText = bgmInputField.GetComponentInChildren<Text>();
        bgmInputFieldText.fontSize = FontSizes.Small;
        bgmSlider.value = SoundVolums.BGMVolume;
        bgmInputField.onValueChanged.AddListener((newString) =>
        {
            InputFieldOnValueChanged(bgmInputField, bgmSlider, newString, true);
        });
        bgmSlider.onValueChanged.AddListener((newValue) =>
        {
            SliderOnValueChanged(bgmInputField, bgmSlider, newValue, true);
        });

        var seText = seTextObject.GetComponent<Text>();
        seText.text += ("(" + SoundVolums.minVolume.ToString() + "~" + SoundVolums.maxVolume.ToString() + ")");
        var seInputField = seInputFieldObject.GetComponent<InputField>();
        var seSlider = seSliderObject.GetComponent<Slider>();
        seInputField.text = SoundVolums.SEVolume.ToString();
        var seInputFieldText = seInputField.GetComponentInChildren<Text>();
        seInputFieldText.fontSize = FontSizes.Small;
        seInputField.onValueChanged.AddListener((newString) =>
        {
            InputFieldOnValueChanged(seInputField, seSlider, newString, false);
        });
        seSlider.value = SoundVolums.SEVolume;
        seSlider.onValueChanged.AddListener((newValue) =>
        {
            SliderOnValueChanged(seInputField, seSlider, newValue, false);
        });

        

        var pointer = -Screen.height * 0.1f;
        pointer = setupComponent(pointer, bgmTextObject, bgmInputFieldObject, bgmSliderObject);
        pointer = setupComponent(pointer, seTextObject, seInputFieldObject, seSliderObject);

        var closeConfigButtonObject = configViewObject.transform.Find(ResourceNames.closeConfigButton).gameObject;
        var closeConfigText = closeConfigButtonObject.GetComponentInChildren<Text>();
        closeConfigText.fontSize = FontSizes.Medium;
        var closeConfigButtonRectTransform = closeConfigButtonObject.GetComponent<RectTransform>();
        var textHeight = closeConfigText.preferredHeight * 1.1f;
        closeConfigButtonRectTransform.sizeDelta = new Vector2(
            textHeight * 4f,
            textHeight
            );
        closeConfigButtonRectTransform.anchoredPosition = new Vector2(
            0f,
            (-Screen.height * 0.9f) + closeConfigButtonRectTransform.sizeDelta.y
            );

        return configViewObject;

    }

}