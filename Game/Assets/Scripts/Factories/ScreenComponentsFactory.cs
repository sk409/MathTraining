using UnityEngine;

public class ScreenComponentsFactory
{

    public static ScreenComponents Make()
    {

        var canvasObject = GameObject.Find(ResourceNames.canvas);

        var fieldWidth = Screen.width * 0.95f;
        var headerMarginTop = Screen.height * 0.025f;
        var headerMarginBottom = headerMarginTop;
        var headerWidth = fieldWidth;
        var headerToolsMargin = headerWidth * 0.02f;
        var headerToolsWidth = headerWidth - (headerToolsMargin * 2f);
        var decorationCirclesWidth = headerToolsWidth * 0.05f;
        var decorationCirclesHeight = decorationCirclesWidth * 2f;
        var headerHeight = decorationCirclesHeight;
        var fieldMarginBottom = headerMarginTop * 2f;
        var fieldHeight = (Screen.height - headerMarginTop - headerHeight - headerMarginBottom) - fieldMarginBottom;

        

        var outerFrameObject = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.gameFieldOuterFrameImage));
        outerFrameObject.transform.SetParent(canvasObject.transform, false);
        var outerFrameRectTransform = outerFrameObject.GetComponent<RectTransform>();
        outerFrameRectTransform.sizeDelta = new Vector2(fieldWidth, fieldHeight);
        outerFrameRectTransform.anchoredPosition = new Vector2(0f, fieldMarginBottom);

        var innerFrameObject = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.gameFieldInnerFrameImage));
        innerFrameObject.transform.SetParent(outerFrameObject.transform, false);
        var innerFrameRectTransform = innerFrameObject.GetComponent<RectTransform>();
        var innerFrameSize = new Vector2(fieldWidth - 30f, fieldHeight - 30f);
        innerFrameRectTransform.sizeDelta = innerFrameSize;

        return new ScreenComponents(
            fieldMarginBottom,
            fieldWidth,
            fieldHeight,
            headerWidth,
            headerHeight,
            headerMarginTop,
            headerMarginBottom,
            headerToolsWidth,
            headerToolsMargin,
            decorationCirclesWidth,
            decorationCirclesHeight,
            outerFrameObject,
            innerFrameObject
            );
    }

}
