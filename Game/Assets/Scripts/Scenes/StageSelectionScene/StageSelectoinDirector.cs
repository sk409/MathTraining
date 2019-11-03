using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectoinDirector : MonoBehaviour
{

    private class ButtonObjectHolder
    {
        public readonly int stageIndex;
        public readonly float y;
        public readonly GameObject buttonObject;

        public ButtonObjectHolder(int stageIndex, float y, GameObject buttonObject)
        {
            this.stageIndex = stageIndex;
            this.y = y;
            this.buttonObject = buttonObject;
        }

    }

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

        var stageSelectionTextObject = Instantiate(Resources.Load<GameObject>(ResourceNames.stageSelectionText));
        stageSelectionTextObject.transform.SetParent(canvasObject.transform, false);
        var stageSelectionTextRectTransform = stageSelectionTextObject.GetComponent<RectTransform>();
        var stageSelectionTextAnchoredPosition = new Vector2(
            0f,
            -headerMarginTop
            );
        var stageSelectionTextSize = new Vector2(
            headerWidth * 0.8f,
            headerHeight
            );
        stageSelectionTextRectTransform.anchoredPosition = stageSelectionTextAnchoredPosition;
        stageSelectionTextRectTransform.sizeDelta = stageSelectionTextSize;

        var backToPreviousSceneButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.backToPreviousSceneButton));
        backToPreviousSceneButtonObject.transform.SetParent(canvasObject.transform, false);
        var backToPreviousSceneButtonRectTransform = backToPreviousSceneButtonObject.GetComponent<RectTransform>();
        backToPreviousSceneButtonRectTransform.anchoredPosition = new Vector2(
            (Screen.width - headerWidth) / 2,
            -headerMarginTop
            );
        backToPreviousSceneButtonRectTransform.sizeDelta = new Vector2(
            headerHeight,
            headerHeight
            );

        //
        screenComponents.outerFrameObject.transform.SetAsLastSibling();
        //

        var curtainObject = Instantiate(Resources.Load<GameObject>(ResourceNames.curtainImage));
        curtainObject.transform.SetParent(canvasObject.transform, false);
        var curtainManager = curtainObject.GetComponent<CurtainManager>();
        
        var backToPreviousSceneButton = backToPreviousSceneButtonObject.GetComponent<Button>();
        backToPreviousSceneButton.onClick.AddListener(() =>
        {
            curtainManager.transform.SetAsLastSibling();
            curtainManager.Close(() =>
            {
                SceneManager.LoadScene(ResourceNames.titleScene);
                return true;
            });
        });

        
        curtainManager.Open(() =>
        {
            curtainObject.transform.SetAsFirstSibling();
            var numberOfButtonsInRow = 10;
            var stageCount = StageManager.Stages.Length;
            //Debug.Log(stageCount);
            var numberOfButtonsInColumn = Mathf.Ceil((float)stageCount / (float)numberOfButtonsInRow);
            var expectedButtonWidth = (innerFrameSize.x / numberOfButtonsInRow);
            var expectedButtonHeight = (innerFrameSize.y / numberOfButtonsInColumn);
            var padding = innerFrameSize.y * 0.05f;
            var buttonSize = (expectedButtonWidth < expectedButtonHeight) ?
            (innerFrameSize.x - (padding * (numberOfButtonsInRow + 1))) / numberOfButtonsInRow :
            (innerFrameSize.y - (padding * (numberOfButtonsInColumn + 1))) / numberOfButtonsInColumn;
            var horizontalMargin = (expectedButtonWidth < expectedButtonHeight) ?
            padding : (innerFrameSize.x - (buttonSize * numberOfButtonsInRow) - (padding * (numberOfButtonsInRow - 1))) / 2f;
            var verticalMargin = (expectedButtonWidth < expectedButtonHeight) ?
            (innerFrameSize.y - (buttonSize * numberOfButtonsInColumn) - (padding * (numberOfButtonsInColumn - 1))) / 2f : padding;
            var baseButtonY = -verticalMargin;
            var clearedStages = ClearedStages.Load();
            var buttonObjectHolders = new List<ButtonObjectHolder>();
            for (var stageIndex = 0; stageIndex < stageCount; ++stageIndex)
            {
                var buttonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.stageButton));
                buttonObjectHolders.Add(new ButtonObjectHolder(
                    stageIndex,
                    baseButtonY,
                    buttonObject
                    ));
                buttonObject.transform.SetParent(innerFrameObject.transform, false);
                var buttonRectTransform = buttonObject.GetComponent<RectTransform>();
                var indexInRow = (stageIndex % numberOfButtonsInRow);
                buttonRectTransform.anchoredPosition = new Vector2(
                    (buttonSize * indexInRow) + (padding * indexInRow) + horizontalMargin,
                    baseButtonY + Screen.height
                    );
                buttonRectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
                var button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    curtainObject.transform.SetAsLastSibling();
                    HandleStageButton(button, curtainManager);
                });
                var text = buttonObject.GetComponentInChildren<Text>();
                text.text = (stageIndex + 1).ToString();
                if (clearedStages.stageIndices.Contains(stageIndex))
                {
                    var image = buttonObject.GetComponentInChildren<Image>();
                    image.sprite = Resources.Load<Sprite>(ResourceNames.stageButtonCleared);
                }
                if ((stageIndex + 1) % (numberOfButtonsInRow) == 0)
                {
                    baseButtonY -= (padding + buttonSize);
                }
            }
            buttonObjectHolders.Sort((h1, h2) =>
            {
                var y1 = h1.stageIndex / numberOfButtonsInRow;
                var y2 = h2.stageIndex / numberOfButtonsInRow;
                if (y1 != y2)
                {
                    return y2 - y1;
                }
                var x1 = h1.stageIndex % numberOfButtonsInRow;
                var x2 = h2.stageIndex % numberOfButtonsInRow;
                return x1 - x2;
            });
            var index = 0;
            var delay = 0.05f;
            foreach (var buttonObjectHolder in buttonObjectHolders)
            {
                var buttonObject = buttonObjectHolder.buttonObject;
                var animationDuration = 0.5f + (delay * index);
                buttonObject.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.PositionY, animationDuration, buttonObjectHolder.y, (animator) =>
                {
                    SoundWareHouse.Instance.seButtonDropped.GetComponent<AudioSource>().Play();
                    return true;
                });
                ++index;
            }
            return true;
        });

    }

    private void HandleStageButton(
        Button stageButton,
        CurtainManager curtainManager
        )
    {
        var title = stageButton.GetComponentInChildren<Text>().text;
        if (int.TryParse(title, out int stageNumber))
        {
            curtainManager.Close(() =>
            {
                GameDirector.stageIndex = (stageNumber - 1);
                SceneManager.LoadScene(ResourceNames.gameScene);
                return true;
            });
        }
    }


}
