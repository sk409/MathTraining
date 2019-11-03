using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{

    private struct Alert
    {
        public GameObject blackoutImageObject;
        public GameObject alertImageObject;
    }

    public static int stageIndex = 0;

    private static readonly float animationDuration = 1.5f;

    private bool isShowingClearConditionAlert = false;
    private StageController stageController;
    private StageData stageData;
    private GameObject canvas;
    private GameObject curtainObject;

    public void ShowClearConditionAlert()
    {
        if (isShowingClearConditionAlert)
        {
            return;
        }
        isShowingClearConditionAlert = true;
        var alert = MakeAlertObject();
        var blackoutImageObject = alert.blackoutImageObject;
        var alertImageObject = alert.alertImageObject;
        var alertImage = alertImageObject.GetComponent<AlertImage>();
        alertImage.Title = "クリア条件";
        alertImage.Message = stageData.moves.ToString() + "手で全消し";
        alertImage.AppendButton("ゲームスタート", (button) =>
        {
            var objects = new GameObject[]{ blackoutImageObject, alertImageObject };
            FadeOutObjects(objects, animationDuration, () =>
            {
                isShowingClearConditionAlert = false;
                return true;
            });
            return true;
        });
        alertImageObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, null);
    }

    public void ShowGameOverAlert()
    {
        var alert = MakeAlertObject();
        var blackoutImageObject = alert.blackoutImageObject;
        var alertImageObject = alert.alertImageObject;
        var alertImage = alertImageObject.GetComponent<AlertImage>();
        alertImage.Title = "ゲームオーバー";
        alertImage.Message = stageData.moves.ToString() + "手で全消ししてください";
        alertImage.AppendButton("ステージ選択", (button) =>
        {
            Transition(ResourceNames.stageSelectionScene);
            return true;
        });
        alertImage.AppendButton("直前に戻る", (button) =>
        {
            var objects = new GameObject[] { blackoutImageObject, alertImageObject };
            FadeOutObjects(objects, animationDuration, () =>
            {
                stageController.GoBackTurn();
                return true;
            });
            return true;
        });
        alertImageObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, null);
    }

    public void ShowMenu()
    {
        var blackoutImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.blackoutImage));
        blackoutImageObject.transform.SetParent(canvas.transform, false);
        var stageNameObject = Instantiate(Resources.Load<GameObject>(ResourceNames.stageNameText));
        stageNameObject.transform.SetParent(canvas.transform, false);
        var stageNameTextRectTransform = stageNameObject.GetComponent<RectTransform>();
        var stageNameTextAnchoredPosition = new Vector2(
            0f,
            -Screen.height * 0.1f
            );
        var stageNameTextSize = new Vector2(
            Screen.width * 0.8f,
            Screen.height * 0.1f
            );
        stageNameTextRectTransform.anchoredPosition = stageNameTextAnchoredPosition;
        stageNameTextRectTransform.sizeDelta = stageNameTextSize;
        var stageNameText = stageNameObject.GetComponent<Text>();
        stageNameText.text = "ステージ " + (stageIndex + 1).ToString();
        var stageNameTextColor = stageNameText.color;
        stageNameTextColor.a = 0;
        stageNameText.color = stageNameTextColor;
        stageNameObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, null);
        var stageNameTextMarginBottom = Screen.height * 0.1f;
        var buttonMarginBottom = Screen.height * 0.1f;
        var buttonPadding = Screen.height * 0.05f;
        var buttonCount = 5;
        var buttonHeight = (Screen.height + stageNameTextAnchoredPosition.y - stageNameTextSize.y - stageNameTextMarginBottom - (buttonPadding * (buttonCount - 1)) - buttonMarginBottom) / buttonCount;
        GameObject makeButton(int index, string title, Func<Button, bool> onClick)
        {
            var buttonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.menuToolButton));
            buttonObject.transform.SetParent(canvas.transform, false);
            var rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(
                0f,
                stageNameTextAnchoredPosition.y - stageNameTextSize.y - stageNameTextMarginBottom - (buttonPadding * index) - (buttonHeight * index));
            rectTransform.sizeDelta = new Vector2(
                buttonHeight * 4f,
                buttonHeight
                );
            var text = buttonObject.GetComponentInChildren<Text>();
            text.text = title;
            var textColor = text.color;
            textColor.a = 0f;
            text.color = textColor;
            var image = buttonObject.GetComponentInChildren<Image>();
            var imageColor = image.color;
            imageColor.a = 0;
            image.color = imageColor;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                onClick(button);
            });
            buttonObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, null);
            return buttonObject;
        }
        var tutorialButton = makeButton(1, "操作説明", (button) =>
        {
            curtainObject.transform.SetAsLastSibling();
            var curtainManager = curtainObject.GetComponent<CurtainManager>();
            curtainManager.Close(() =>
            {
                TutorialViewFactory.Make(canvas, curtainManager);
                return true;
            });
            return true;
        });
        var configButton = makeButton(2, "設定", (button) =>
        {
            curtainObject.transform.SetAsLastSibling();
            var curtainManager = curtainObject.GetComponent<CurtainManager>();
            curtainManager.Close(() =>
            {
                var configView = ConfigViewFactory.Make(canvas);
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
        var stageSelectionButton = makeButton(3, "ステージ選択", (button) =>
        {
            Transition(ResourceNames.stageSelectionScene);
            return true;
        });
        var titleButton = makeButton(4, "タイトルに戻る", (button) => {
            Transition(ResourceNames.titleScene);
            return true;
        });
        var backGameButton = makeButton(0, "ゲームに戻る", (button) =>
        {
            var toolObjects = new GameObject[] {
                blackoutImageObject,
                stageNameObject,
                stageSelectionButton,
                tutorialButton,
                titleButton,
                button.gameObject,
                configButton,
            };
            foreach (var toolObject in toolObjects)
            {
                toolObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (animator) =>
                {
                    Destroy(toolObject);
                    return true;
                });
            }
            return true;
        });
        
    }

    public void PerformClearTask()
    {
        var clearedStages = ClearedStages.Load();
        if (!clearedStages.stageIndices.Contains(stageIndex))
        {
            clearedStages.stageIndices.Add(stageIndex);
            clearedStages.Write();
        }
        SoundWareHouse.Instance.seGameClear.GetComponent<AudioSource>().Play();
        var stageClearTextObject = Instantiate(Resources.Load<GameObject>(ResourceNames.stageClearText));
        stageClearTextObject.transform.SetParent(canvas.transform, false);
        var stageClearTextRectTransform = stageClearTextObject.GetComponent<RectTransform>();
        var textHeight = stageController.innerFrameSize.y * 0.4f;
        stageClearTextRectTransform.sizeDelta = new Vector2(textHeight * 2f, textHeight);
        var stageClearText = stageClearTextObject.GetComponent<Text>();
        var stageClearTextColor = stageClearText.color;
        stageClearTextColor.a = 0;
        stageClearText.color = stageClearTextColor;
        stageClearText.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, (animator) =>
        {
            var textMarginTop = stageController.innerFrameSize.y * 0.05f;
            var y = stageController.innerFrameSize.y / 2f - textHeight / 2f - textMarginTop;
            animator.Animate(UIAnimator.AnimationKey.PositionY, animationDuration, y, (a) =>
            {
                var buttonHeight = stageController.innerFrameSize.y * 0.2f;
                var buttonWidth = buttonHeight * 2f;
                var buttonMarginTop = buttonHeight / 4f;
                var buttonPadding = buttonHeight * 0.1f;
                void MakeButton(string title, int buttonIndex, Func<Button, bool> onClick)
                {
                    var buttonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.button));
                    buttonObject.transform.SetParent(canvas.transform, false);
                    buttonObject.GetComponentInChildren<Text>().text = title;
                    var rectTransform = buttonObject.GetComponent<RectTransform>();
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(
                        0f,
                        y - (textHeight / 2f) - buttonMarginTop - (buttonHeight * buttonIndex) - (buttonPadding * buttonIndex)
                        );
                    rectTransform.sizeDelta = new Vector2(buttonWidth, buttonHeight);
                    var button = buttonObject.GetComponent<Button>();
                    button.onClick.AddListener(() =>
                    {
                        onClick(button);
                    });
                    var image = buttonObject.GetComponentInChildren<Image>();
                    var imageColor = image.color;
                    imageColor.a = 0f;
                    image.color = imageColor;
                    var buttonText = button.GetComponentInChildren<Text>();
                    var textColor = buttonText.color;
                    textColor.a = 0f;
                    buttonText.color = textColor;
                    buttonObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 1f, null);
                }
                var isExistNextStage = (stageIndex + 1) < StageManager.Stages.Length;
                var index = 0;
                if (isExistNextStage)
                {
                    MakeButton("次のステージ", index, (button) =>
                    {
                        ++stageIndex;
                        Transition(ResourceNames.gameScene);
                        return true;
                    });
                    ++index;
                }
                MakeButton("ステージ選択", index, (button) =>
                {
                    Transition(ResourceNames.stageSelectionScene);
                    return true;
                });
                ++index;
                MakeButton("タイトルに戻る", index, (button) =>
                {
                    Transition(ResourceNames.titleScene);
                    return true;
                });
                ++index;
                return true;
            });
            return true;
        });

    }

    private Alert MakeAlertObject(bool withBlackout = true)
    {
        var canvasObject = GameObject.Find(ResourceNames.canvas);
        var blackoutImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.blackoutImage));
        blackoutImageObject.transform.SetParent(canvasObject.transform, false);
        var blackoutImage = blackoutImageObject.GetComponent<Image>();
        var alertImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.alertImage));
        alertImageObject.transform.SetParent(canvasObject.transform, false);
        var alertImageRectTransform = alertImageObject.GetComponent<RectTransform>();
        alertImageRectTransform.sizeDelta = new Vector2(Screen.width * 0.8f, Screen.height * 0.8f);
        return new Alert() {
            blackoutImageObject = blackoutImageObject,
            alertImageObject = alertImageObject,
        };
    }

    private void FadeOutObjects(GameObject[] objects, float animationDuration = 2f, Func<bool> completion = null)
    {
        var completionCount = 0;
        void destroyIfCompleted()
        {
            if (completionCount == objects.Length)
            {
                foreach (var obj in objects)
                {
                    Destroy(obj);
                }
                completion?.Invoke();
            }
        }
        foreach (var obj in objects)
        {
            obj.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Alpha, animationDuration, 0f, (animator) =>
             {
                 ++completionCount;
                 destroyIfCompleted();
                 return true;
             });
        }
    }

    private void Transition(string sceneName)
    {
        curtainObject.transform.SetAsLastSibling();
        curtainObject.GetComponent<CurtainManager>().Close(() =>
        {
            SceneManager.LoadScene(sceneName);
            return true;
        });
    }

    private void Start()
    {

        canvas = GameObject.Find(ResourceNames.canvas);
        curtainObject = Instantiate(Resources.Load<GameObject>(ResourceNames.curtainImage));
        curtainObject.transform.SetParent(canvas.transform, false);

        if (!SoundWareHouse.Instance.bgmGame.isPlaying)
        {
            SoundWareHouse.Instance.StopAllBGM();
            SoundWareHouse.Instance.bgmGame.Play();
        }

        stageData = StageManager.LoadData(stageIndex);
        var stageObject = new GameObject() { name = "stage" };
        stageController = stageObject.AddComponent<StageController>();
        stageController.curtainObject = curtainObject;
        stageController.Build(stageData);
    }
    

}
