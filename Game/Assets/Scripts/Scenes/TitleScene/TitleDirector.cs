using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{

    public static float animationDuration = 1f;

    private GameObject canvasObject;

    private void Start()
    {

        canvasObject = GameObject.Find(ResourceNames.canvas);

        if (!SoundWareHouse.Instance.bgmNormal.isPlaying)
        {
            SoundWareHouse.Instance.StopAllBGM();
            SoundWareHouse.Instance.bgmNormal.Play();
        }


        var screenComponents = ScreenComponentsFactory.Make();
        var headerWidth = screenComponents.headerWidth;
        var headerHeight = screenComponents.headerHeight;
        var headerMarginTop = screenComponents.headerMarginTop;
        var headerToolsMargin = screenComponents.headerToolsMargin;
        var innerFrameObject = screenComponents.innerFrameObject;
        var innerFrameSize = innerFrameObject.GetComponent<RectTransform>().sizeDelta;


        var titleImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.titleImage));
        //Debug.Log(titleImageObject);
        titleImageObject.transform.SetParent(canvasObject.transform, false);
        var titleImageRectTransform = titleImageObject.GetComponent<RectTransform>();
        var titleImageSize = new Vector2(
            headerHeight * 4f,
            headerHeight
            );
        var titleImageAnchoredPosition = new Vector2(
            0f,
            -(Screen.height / 2f) + (titleImageSize.y / 2f)
            );
        titleImageRectTransform.anchoredPosition = titleImageAnchoredPosition;
        titleImageRectTransform.sizeDelta = Vector2.zero;

        var buttonPadding = innerFrameSize.y * 0.05f;
        var buttonMarginTop = innerFrameSize.y * 0.13f;
        var buttonMarginBottom = innerFrameSize.y * 0.13f;
        var buttonCount = 5;
        GameObject makeButton(string title, int index, Func<Button, bool> onClick)
        {
            var buttonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.titleButton));
            buttonObject.transform.SetParent(innerFrameObject.transform, false);
            var rectTransform = buttonObject.GetComponent<RectTransform>();
            var height = (innerFrameSize.y - buttonMarginTop - buttonMarginBottom - (buttonPadding * (buttonCount - 1))) / buttonCount;
            var width = height * 4f;
            rectTransform.anchoredPosition = new Vector2(
                0f,
                -buttonMarginTop - (buttonPadding * index) - (height * index)
                );
            rectTransform.sizeDelta = new Vector2(
                width,
                height
                );
            var text = buttonObject.GetComponentInChildren<Text>();
            text.text = title;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                onClick(button);
            });
            return buttonObject;
        }

        var curtainObject = Instantiate(Resources.Load<GameObject>(ResourceNames.curtainImage));
        curtainObject.transform.SetParent(canvasObject.transform, false);
        var curtainManager = curtainObject.GetComponent<CurtainManager>();

        var startButton = makeButton("ゲームスタート", 0, (button) =>
        {
            Transition(ResourceNames.stageSelectionScene, curtainManager);
            return true;
        });
        var tutorialButton = makeButton("操作説明", 1, (button) =>
        {
            curtainObject.transform.SetAsLastSibling();
            curtainManager.Close(() =>
            {
                TutorialViewFactory.Make(canvasObject, curtainManager);
                return true;
            });
            return true;
        });
        var resetButton = makeButton("初期化", 2, (button) =>
        {
            var blackoutImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.blackoutImage));
            blackoutImageObject.transform.SetParent(canvasObject.transform, false);
            var resetConfirmation = Instantiate(Resources.Load<GameObject>(ResourceNames.resetCofirmation));
            resetConfirmation.transform.SetParent(canvasObject.transform, false);
            var resetConfirmationText = resetConfirmation.transform.Find(ResourceNames.titleText).gameObject;
            var resetConfirmationTextRectTransform = resetConfirmationText.GetComponent<RectTransform>();
            resetConfirmationTextRectTransform.sizeDelta = new Vector2(
                Screen.width,
                Screen.height * 0.2f
                );
            GameObject MakeButton(int sign, string name, Func<bool> onClick)
            {
                var buttonObject = resetConfirmation.transform.Find(name).gameObject;
                var rectTransform = buttonObject.GetComponent<RectTransform>();
                var width = innerFrameSize.x * 0.3f;
                var height = width / 4f;
                rectTransform.anchoredPosition = new Vector2(sign * (width + Screen.width * 0.05f) / 2f, 0f);
                rectTransform.sizeDelta = new Vector2(width, height);
                var yesButton = buttonObject.GetComponent<Button>();
                yesButton.onClick.AddListener(() =>
                {
                    onClick();
                });
                return buttonObject;
            }
            MakeButton(-1, ResourceNames.yesButton, () =>
            {
                ClearedStages.Reset();
                resetConfirmation.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (animator) =>
                {
                    var resetAlert = Instantiate(Resources.Load<GameObject>(ResourceNames.resetAlert));
                    resetAlert.transform.SetParent(canvasObject.transform, false);
                    var resetAlertText = resetAlert.transform.Find(ResourceNames.titleText).gameObject;
                    resetAlertText.GetComponent<RectTransform>().sizeDelta = resetConfirmationTextRectTransform.sizeDelta;
                    var closeButtonObject = resetAlert.transform.Find(ResourceNames.closeButton).gameObject;
                    var closeButtonRectTransform = closeButtonObject.GetComponent<RectTransform>();
                    var closeButtonWidth = Screen.width * 0.3f;
                    var closeButtonHeight = closeButtonWidth / 4f;
                    closeButtonRectTransform.sizeDelta = new Vector2(closeButtonWidth, closeButtonHeight);
                    var closeButton = closeButtonObject.GetComponent<Button>();
                    closeButton.onClick.AddListener(() =>
                    {
                        blackoutImageObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (a2) =>
                        {
                            Destroy(blackoutImageObject);
                            return true;
                        });
                        resetAlert.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (a2) =>
                        {
                            Destroy(resetAlert);
                            return true;
                        });
                    });
                    resetAlert.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, (a2) =>
                    {
                        return true;
                    });
                    Destroy(resetConfirmation);
                    return true;
                });
                return true;
            });
            MakeButton(1, ResourceNames.noButton, () =>
            {
                blackoutImageObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (animator) =>
                {
                    Destroy(blackoutImageObject);
                    return true;
                });
                resetConfirmation.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (animator) =>
                {
                    Destroy(resetConfirmation);
                    return true;
                });
                return true;
            });
            resetConfirmation.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, (animator) =>
            {
                return true;
            });
            return true;
        });
        var configButton = makeButton("設定", 3, (button) =>
        {
            curtainObject.transform.SetAsLastSibling();
            curtainManager.Close(() =>
            {
                var configView = ConfigViewFactory.Make(canvasObject);
                curtainObject.transform.SetAsLastSibling();
                curtainManager.Open(() =>
                {
                    curtainObject.transform.SetAsFirstSibling();
                    var closeConfigButton = configView.transform.Find(ResourceNames.closeConfigButton).GetComponent<Button>();
                    closeConfigButton.onClick.AddListener(() =>
                    {
                        curtainObject.transform.SetAsLastSibling();
                        curtainManager.Close(() =>
                        {
                            Destroy(configView);
                            curtainManager.Open(() =>
                            {
                                curtainObject.transform.SetAsFirstSibling();
                                return true;
                            });
                            return true;
                        });
                    });
                    return true;
                });
                return true;
            });
            return true;
        });
        var endButton = makeButton("終了", 4, (button) =>
        {
            UnityEngine.Application.Quit();
            return true;
        });

        
        curtainManager.Open(() =>
        {
            curtainObject.transform.SetAsFirstSibling();
            titleImageRectTransform.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Size, animationDuration, titleImageSize * 2f, (animator) =>
            {
                animator.Animate(UIAnimator.AnimationKey.Size, animationDuration, titleImageSize, (a2) =>
                {
                    animator.Animate(UIAnimator.AnimationKey.PositionY, animationDuration, -headerMarginTop, (a3) =>
                    {
                        var buttons = new GameObject[] { startButton, tutorialButton, resetButton, configButton, endButton };
                        foreach (var button in buttons)
                        {
                            button.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, (a4) =>
                            {
                                return true;
                            });
                        }
                        return true;
                    });
                    return true;
                });
                return true;
            });
            return true;
        }, animationDuration);
    }

    private void Transition(string sceneName, CurtainManager curtainManager)
    {
        curtainManager.gameObject.transform.SetAsLastSibling();
        curtainManager.Close(() =>
        {
            SceneManager.LoadScene(sceneName);
            return true;
        });
    }

}
