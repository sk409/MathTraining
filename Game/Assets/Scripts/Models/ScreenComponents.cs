using UnityEngine;

public class ScreenComponents {

    public readonly float fieldMarginBottom;
    public readonly float fieldWidth;
    public readonly float fieldHeight;
    public readonly float headerWidth;
    public readonly float headerHeight;
    public readonly float headerMarginTop;
    public readonly float headerMarginBottom;
    public readonly float headerToolsWidth;
    public readonly float headerToolsMargin;
    public readonly float decorationCirclesWidth;
    public readonly float decorationCirclesHeight;
    public readonly GameObject outerFrameObject;
    public readonly GameObject innerFrameObject;

    public ScreenComponents(
        float fieldMarginBottom,
        float fieldWidth,
        float fieldHeight,
        float headerWidth,
        float headerHeight,
        float headerMarginTop,
        float headerMarginBottom,
        float headerToolsWidth,
        float headerToolsMargin,
        float decorationCirclesWidth,
        float decorationCirclesHeight,
        GameObject outerFrameObject,
        GameObject innerFrameObject
        )
    {
        this.fieldMarginBottom = fieldMarginBottom;
        this.fieldWidth = fieldWidth;
        this.fieldHeight = fieldHeight;
        this.headerWidth = headerWidth;
        this.headerHeight = headerHeight;
        this.headerMarginTop = headerMarginTop;
        this.headerMarginBottom = headerMarginBottom;
        this.headerToolsWidth = headerToolsWidth;
        this.headerToolsMargin = headerToolsMargin;
        this.decorationCirclesWidth = decorationCirclesWidth;
        this.decorationCirclesHeight = decorationCirclesHeight;
        this.outerFrameObject = outerFrameObject;
        this.innerFrameObject = innerFrameObject;
    }

}