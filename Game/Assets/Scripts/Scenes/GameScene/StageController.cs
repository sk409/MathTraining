using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{

    public static float padding = 5f;

    public List<TermButton> TermButtons
    {
        set
        {
            foreach (var row in grid)
            {
                foreach(var termButton in row)
                {
                    if (termButton == null)
                    {
                        continue;
                    }
                    Destroy(termButton.gameObject);
                }
            }
            grid.Clear();
            termButtons = value;
            var maxX = 0;
            var maxY = 0;
            foreach (var termButton in termButtons)
            {
                maxX = Math.Max(maxX, termButton.GetPosition().x);
                maxY = Math.Max(maxY, termButton.GetPosition().y);
            }
            for (var x = 0; x <= maxX; ++x)
            {
                grid.Add(new List<TermButton>(maxY + 1));
                for (var y = 0; y <= maxY; ++y)
                {
                    grid[x].Add(null);
                }
                for (var y = 0; y <= maxY; ++y)
                {
                    foreach (var termButton in termButtons)
                    {
                        if (termButton.GetPosition().x == x && termButton.GetPosition().y == y)
                        {
                            grid[x][y] = termButton;
                            break;
                        }
                    }
                }
            }
        }
    }

    public bool IsEmpty
    {
        get
        {
            foreach (var row in grid)
            {
                foreach (var termButton in row)
                {
                    if (termButton != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public bool IsClear
    {
        get
        {
            if (moves != stageData.moves)
            {
                return false;
            }
            return IsEmpty;
        }
    }

    public GameObject curtainObject;
    public Vector2 innerFrameSize;

    private int moves = 0;
    private int turn = 0;
    private int maxX = int.MaxValue;
    private bool isAnimation = false;
    private float buttonSize;
    private StageData stageData;
    private GameObject innerFrameObject;
    private GameDirector gameDirector;
    private Formula formula = new Formula();
    private FormulaText formulaText;
    private Text movesText;
    private List<TermButton> termButtons;
    private List<List<TermButton>> grid = new List<List<TermButton>>();
    private readonly List<Vector2Int> selectedPositions = new List<Vector2Int>();
    private readonly List<SnapShot> snapShots = new List<SnapShot>();

    public void Build(StageData stageData, bool fieldOnly = false, bool curtain = true, bool withSnapShot = true, bool dropAnimation = true)
    {
        this.stageData = stageData;
        var canvas = GameObject.Find(ResourceNames.canvas);
        var innerFramePaddingBottom = 5f;
        if (gameDirector == null)
        {
            gameDirector = GameObject.Find(ResourceNames.gameDirector).GetComponent<GameDirector>();
        }
        if (!fieldOnly)
        {

            var screenComponents = ScreenComponentsFactory.Make();
            var fieldMarginBottom = screenComponents.fieldMarginBottom;
            var fieldWidth = screenComponents.fieldWidth;
            var fieldHeight = screenComponents.fieldHeight;
            var headerHeight = screenComponents.headerHeight;
            var headerToolsWidth = screenComponents.headerToolsWidth;
            var headerMarginTop = screenComponents.headerMarginTop;
            var headerMarginBottom = screenComponents.headerMarginBottom;
            var headerToolsMargin = screenComponents.headerToolsMargin;
            var decorationCirclesWidth = screenComponents.decorationCirclesWidth;
            var decorationCirclesHeight = screenComponents.decorationCirclesHeight;

            var menuButtonWidth = headerToolsWidth * 0.1f;
            var formulaImageWidth = headerToolsWidth - menuButtonWidth - decorationCirclesWidth;
            var formulaImageHeight = formulaImageWidth / 10f;
            var menuButtonHeight = menuButtonWidth;

            var menuButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.menuButton));
            menuButtonObject.transform.SetParent(canvas.transform, false);
            var menuButtonRectTransform = menuButtonObject.GetComponent<RectTransform>();
            menuButtonRectTransform.anchoredPosition = new Vector2(
                (Screen.width - fieldWidth) / 2,
                -headerMarginTop - (menuButtonHeight / 2f) - ((headerHeight - menuButtonHeight) /2f)
                );
            menuButtonRectTransform.sizeDelta = new Vector2(menuButtonWidth, menuButtonHeight);
            var menuButton = menuButtonObject.GetComponent<Button>();
            menuButton.onClick.AddListener(() =>
            {
                if (isAnimation || IsEmpty)
                {
                    SoundWareHouse.Instance.seButtonError.Play();
                    return;
                }
                SoundWareHouse.Instance.seSpecialButton.Play();
                gameDirector.ShowMenu();
            });
            var formulaImageObject = Instantiate(Resources.Load<GameObject>(ResourceNames.formulaImage));
            formulaImageObject.transform.SetParent(canvas.transform, false);
            var formulaImageRectTransform = formulaImageObject.GetComponent<RectTransform>();
            formulaImageRectTransform.anchoredPosition = new Vector2(
                menuButtonRectTransform.anchoredPosition.x + menuButtonRectTransform.sizeDelta.x + headerToolsMargin,
                -headerMarginTop - (formulaImageHeight / 2f) - ((headerHeight - formulaImageHeight) / 2f)
                );
            formulaImageRectTransform.sizeDelta = new Vector2(
                formulaImageWidth,
                formulaImageHeight
                );
            formulaText = formulaImageObject.GetComponentInChildren<FormulaText>();
            var formulaTextRectTransform = formulaText.GetComponent<RectTransform>();
            formulaTextRectTransform.anchoredPosition = new Vector2(
                formulaImageWidth * 0.01f,
                0f
                );
            formulaTextRectTransform.sizeDelta = formulaImageRectTransform.sizeDelta;
            formulaText.text = "";
            var decorationCirclesObject = Instantiate(Resources.Load<GameObject>(ResourceNames.decorationCirclesImage));
            decorationCirclesObject.transform.SetParent(canvas.transform, false);
            var decorationCirclesRectTransform = decorationCirclesObject.GetComponent<RectTransform>();
            decorationCirclesRectTransform.anchoredPosition = new Vector2(
                formulaImageRectTransform.anchoredPosition.x + formulaImageRectTransform.sizeDelta.x + headerToolsMargin,
                -headerMarginTop - (decorationCirclesHeight / 2f) - ((headerHeight - decorationCirclesHeight) / 2f)
                );
            decorationCirclesRectTransform.sizeDelta = new Vector2(
                decorationCirclesWidth,
                decorationCirclesHeight
                );

            var outerFrameObject = Instantiate(Resources.Load<GameObject>(ResourceNames.gameFieldOuterFrameImage));
            outerFrameObject.transform.SetParent(canvas.transform, false);
            var outerFrameRectTransform = outerFrameObject.GetComponent<RectTransform>();
            outerFrameRectTransform.sizeDelta = new Vector2(fieldWidth, fieldHeight);
            outerFrameRectTransform.anchoredPosition = new Vector2(0f, fieldMarginBottom);
            innerFrameObject = Instantiate(Resources.Load<GameObject>(ResourceNames.gameFieldInnerFrameImage));
            innerFrameObject.transform.SetParent(outerFrameObject.transform, false);
            var innerFrameRectTransform = innerFrameObject.GetComponent<RectTransform>();
            innerFrameSize = new Vector2(fieldWidth - 30f, fieldHeight - 30f);
            innerFrameRectTransform.sizeDelta = innerFrameSize;
            
            //innerFrameRectTransform.sizeDelta = outerFrameRectTransform.sizeDelta * 0.97f;
            var numberOfButtonsInColumn = 5;
            buttonSize = (innerFrameRectTransform.sizeDelta.y - innerFramePaddingBottom - (padding * (numberOfButtonsInColumn - 1))) / numberOfButtonsInColumn;
            var maxToolCount = 4;
            var toolMargin = innerFrameSize.y * 0.05f;
            var maxButtonCount = 7;
            var rightSpace = (innerFrameSize.x - (buttonSize * maxButtonCount) - (padding * (maxButtonCount - 1))) / 2f;
            var toolSize = (innerFrameSize.y - (toolMargin * (maxToolCount + 2))) / maxToolCount;
            //Debug.Log(toolSize);
            var toolPositionX = (innerFrameSize.x / 2f) - ((rightSpace - toolSize) / 2f) - (toolSize / 2f);
            var helpButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.helpButton));
            helpButtonObject.transform.SetParent(innerFrameObject.transform, false);
            var helpButtonRectTransform = helpButtonObject.GetComponent<RectTransform>();
            helpButtonRectTransform.anchoredPosition = new Vector2(toolPositionX, -(innerFrameSize.y / 2f) + (toolSize / 2f) + toolMargin);
            helpButtonRectTransform.sizeDelta = new Vector2(toolSize, toolSize);
            var helpButton = helpButtonObject.GetComponent<Button>();
            helpButton.onClick.AddListener(() => {
                if (isAnimation || IsEmpty)
                {
                    SoundWareHouse.Instance.seButtonError.Play();
                    return;
                }
                SoundWareHouse.Instance.seSpecialButton.Play();
                gameDirector.ShowClearConditionAlert();
            });
            var backButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.backButton));
            backButtonObject.transform.SetParent(innerFrameObject.transform, false);
            var backButtonRectTransform = backButtonObject.GetComponent<RectTransform>();
            backButtonRectTransform.anchoredPosition = new Vector2(toolPositionX, -(innerFrameSize.y / 2f) + (toolSize * 1.5f) + (toolMargin * 2f));
            backButtonRectTransform.sizeDelta = new Vector2(toolSize, toolSize);
            var backButton = backButtonObject.GetComponent<Button>();
            backButton.onClick.AddListener(() => {
                if (GoBackTurn())
                {
                    SoundWareHouse.Instance.seControlButton.GetComponent<AudioSource>().Play();
                } else
                {
                    SoundWareHouse.Instance.seButtonError.GetComponent<AudioSource>().Play();
                }
            });
            var forwardButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.forwardButton));
            forwardButtonObject.transform.SetParent(innerFrameObject.transform, false);
            var forwardButtonRectTransform = forwardButtonObject.GetComponent<RectTransform>();
            forwardButtonRectTransform.anchoredPosition = new Vector2(toolPositionX, -(innerFrameSize.y / 2f) + (toolSize * 2.5f) + (toolMargin * 3f));
            forwardButtonRectTransform.sizeDelta = new Vector2(toolSize, toolSize);
            var forwardButton = forwardButtonObject.GetComponent<Button>();
            forwardButton.onClick.AddListener(() => {
                if (GoForwardTurn())
                {
                    SoundWareHouse.Instance.seControlButton.GetComponent<AudioSource>().Play();
                } else
                {
                    SoundWareHouse.Instance.seButtonError.GetComponent<AudioSource>().Play();
                }
            });
            var movesTextObject = Instantiate(Resources.Load<GameObject>(ResourceNames.movesText));
            movesTextObject.transform.SetParent(innerFrameObject.transform, false);
            var movesTextRectTransform = movesTextObject.GetComponent<RectTransform>();
            movesTextRectTransform.anchoredPosition = new Vector2(toolPositionX, -(innerFrameSize.y / 2f) + (toolSize * 3.5f) + (toolMargin * 5f));
            movesTextRectTransform.sizeDelta = new Vector2(toolSize, toolSize);
            movesText = movesTextObject.GetComponent<Text>();
            movesText.text = "0/" + stageData.moves.ToString() + "手";
        }

        void MakeButtons()
        {
            var termButtons = new List<TermButton>();
            var maxX = 0;
            foreach (var operand in stageData.operands)
            {
                maxX = Math.Max(maxX, operand.position.x);
                var operandButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.operandButton));
                var operandButton = operandButtonObject.GetComponent<OperandButton>();
                operandButton.Operand = operand;
                operandButton.onClick.AddListener(() =>
                {
                    HandleOperandButton(operandButton);
                });
                termButtons.Add(operandButton);
            }
            foreach (var oper in stageData.operators)
            {
                maxX = Math.Max(maxX, oper.position.x);
                var operatorButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.operatorButton));
                var operatorButton = operatorButtonObject.GetComponent<OperatorButton>();
                operatorButton.Operator = oper;
                operatorButton.onClick.AddListener(() =>
                {
                    HandleOperatorButton(operatorButton);
                });
                termButtons.Add(operatorButton);
            }
            foreach (var equal in stageData.equals)
            {
                maxX = Math.Max(maxX, equal.position.x);
                var equalButtonObject = Instantiate(Resources.Load<GameObject>(ResourceNames.equalButton));
                var equalButton = equalButtonObject.GetComponent<EqualButton>();
                equalButton.equal = equal;
                equalButton.onClick.AddListener(() =>
                {
                    HandleEqualButton(equalButton);
                });
                termButtons.Add(equalButton);
            }
            termButtons.Sort((a, b) =>
            {
                var pa = a.GetPosition();
                var pb = b.GetPosition();
                if (pa.y != pb.y)
                {
                    return pa.y - pb.y;
                }
                return pa.x - pb.x;
            });
            var index = 0;
            var paddingBetweenTermButtons = Screen.height * 0.05f;
            var notificationCount = 0;
            var buttonCount = termButtons.Count;
            if (this.maxX == int.MaxValue)
            {
                this.maxX = maxX;
            } else
            {
                maxX = this.maxX;
            }
            foreach (var termButton in termButtons)
            {
                termButton.transform.SetParent(innerFrameObject.transform, false);
                var rect = termButton.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(buttonSize, buttonSize);
                var buttonPosition = termButton.GetPosition();
                var offsetX = innerFrameSize.x / 2f - ((float)(maxX + 1) / 2 * buttonSize) - ((float)maxX / 2 * padding);
                var x = buttonSize * buttonPosition.x + padding * buttonPosition.x + offsetX;
                var y = buttonSize * buttonPosition.y + padding * buttonPosition.y + innerFramePaddingBottom;
                if (dropAnimation)
                {
                    rect.anchoredPosition = new Vector2(
                        x,
                        y + Screen.height + (paddingBetweenTermButtons * buttonPosition.y) + (paddingBetweenTermButtons / (maxX - buttonPosition.x + 1))
                    );
                    var delay = 0.1f;
                    var animationDuration = 0.5f + (delay * index);
                    termButton.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.PositionY, animationDuration, y, (animator) =>
                    {
                        ++notificationCount;
                        if (notificationCount == buttonCount)
                        {
                            gameDirector.ShowClearConditionAlert();
                        }
                        SoundWareHouse.Instance.seButtonDropped.GetComponent<AudioSource>().Play();
                        return true;
                    });
                }
                else
                {
                    rect.anchoredPosition = new Vector2(
                        x,
                        y
                    );
                }

                ++index;
            }
            TermButtons = termButtons;
            if (withSnapShot)
            {
                SaveSnapShot();
            }
        }

        if (curtain)
        {
            curtainObject.transform.SetAsLastSibling();
            curtainObject.GetComponent<CurtainManager>().Open(() =>
            {
                curtainObject.transform.SetAsFirstSibling();
                MakeButtons();
                return true;
            });
        } else
        {
            MakeButtons();
        }

    }

    public bool GoBackTurn()
    {
        if (isAnimation || turn <= 0 || IsClear)
        {
            return false;
        }
        --turn;
        var snapShot = snapShots[turn];
        Build(snapShot);
        return true;
    }

    public bool GoForwardTurn()
    {
        if (isAnimation || snapShots.Count <= (turn + 1) || IsClear)
        {
            return false;
        }
        ++turn;
        var snapShot = snapShots[turn];
        Build(snapShot);
        return true;
    }

    private void Build(SnapShot snapShot)
    {
        moves = snapShot.moves;
        movesText.text = snapShot.moves.ToString() + "/" + this.stageData.moves.ToString() + "手";
        formulaText.text = snapShot.formulaText;
        var stageData = snapShot.ToStageData(this.stageData.moves);
        Build(stageData, true, false, false, false);
        selectedPositions.Clear();
        foreach (var data in snapShot.datas)
        {
            if (data.isSelected)
            {
                selectedPositions.Add(new Vector2Int(data.position.x, data.position.y));
                grid[data.position.x][data.position.y].isSelected = true;
                grid[data.position.x][data.position.y].SetSelectedColor();
            }
        }
        formula = new Formula(snapShot.formula);
    }

    private void AdvanceTurn()
    {
        for(var i = snapShots.Count - turn; i > 1; --i)
        {
            snapShots.RemoveAt(snapShots.Count - 1);
        }
        ++turn;
        SaveSnapShot();
    }

    private void SaveSnapShot()
    {
        var snapShot = new SnapShot
        {
            moves = moves,
            formulaText = formulaText.text,
            formula = new Formula(formula),
        };
        for (var x = 0; x < grid.Count; ++x)
        {
            for (var y = 0; y < grid[x].Count; ++y)
            {
                if (grid[x][y] == null)
                {
                    continue;
                }
                var snapShotData = new SnapShotData
                {
                    isSelected = grid[x][y].isSelected,
                    value = grid[x][y].GetValue(),
                    position = grid[x][y].GetPosition(),
                };
                snapShot.datas.Add(snapShotData);
            }
        }
        snapShots.Add(snapShot);
    }

    private void RemoveTermButtons(List<List<Vector2Int>> positions, Func<bool> completion, bool isGroup = false)
    {
        var targets = new List<List<TermButton>>();
        foreach (var row in positions)
        {
            var group = new List<TermButton>();
            foreach(var position in row)
            {
                group.Add(grid[position.x][position.y]);
            }
            targets.Add(group);
        }
        var flatTargets = targets.SelectMany(list => list).ToList();
        var frameObjects = new List<GameObject>();
        void addFrame(List<TermButton> group)
        {
            var minX = float.MaxValue;
            var maxX = 0f;
            var minY = float.MaxValue;
            var maxY = 0f;
            foreach (var target in group)
            {
                var rectTransform = target.GetComponent<RectTransform>();
                minX = Mathf.Min(minX, rectTransform.anchoredPosition.x);
                maxX = Mathf.Max(maxX, rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x);
                minY = Mathf.Min(minY, rectTransform.anchoredPosition.y);
                maxY = Mathf.Max(maxY, rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y);
            }
            var padding = StageController.padding / 4f;
            minX -= padding;
            maxX += padding;
            minY -= padding;
            maxY += padding;
            var frameObject = Instantiate(Resources.Load<GameObject>(ResourceNames.frameImage));
            frameObject.transform.SetParent(innerFrameObject.transform, false);
            var frameRectTransform = frameObject.GetComponent<RectTransform>();
            frameRectTransform.anchoredPosition = new Vector2(minX, minY);
            frameRectTransform.sizeDelta = new Vector2(maxX - minX, maxY - minY);
            frameObjects.Add(frameObject);
        }
        if (isGroup)
        {
            foreach (var group in targets)
            {
                addFrame(group);
            }
        }
        else
        {
            foreach (var target in flatTargets)
            {
                addFrame(new List<TermButton>() { target });
            }
        }
        var maxNotificationCount = flatTargets.Count;
        var notificationCount = 0;
        foreach (var target in flatTargets)
        {
            target.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.Color, 2f, Color.white, (animator) =>
            {
                termButtons.Remove(target);
                grid[target.GetPosition().x][target.GetPosition().y] = null;
                Destroy(target.gameObject);
                ++notificationCount;
                if (notificationCount == maxNotificationCount)
                {
                    SoundWareHouse.Instance.seRemoveButton.GetComponent<AudioSource>().Play();
                    foreach (var frameObject in frameObjects)
                    {
                        Destroy(frameObject);
                    }
                    DropTermButtons(completion);
                }
                return true;
            });
        }
    }

    private void DropTermButtons(Func<bool> completion)
    {

        var droppedButtons = new List<TermButton>();
        var toPositionYs = new List<float>();
        var oldPositions = new List<Vector2Int>();
        var newPositions = new List<Vector2Int>();
        for (var x = 0; x < grid.Count; ++x)
        {
            for (var y = 0; y < grid[x].Count; ++y)
            {
                //var done = false;
                if (grid[x][y] == null)
                {
                    var space = 1;
                    for (var y2 = y + 1; y2 < grid[x].Count; ++y2)
                    {
                        if (grid[x][y2] == null)
                        {
                            ++space;
                        }
                        else
                        {
                            var rectTransform = grid[x][y2].GetComponent<RectTransform>();
                            var fromPositionY = rectTransform.anchoredPosition.y;
                            var toPositionY = fromPositionY - (rectTransform.sizeDelta.y * space) - (padding * space);
                            var newPosition = new Vector2Int(grid[x][y2].GetPosition().x, grid[x][y2].GetPosition().y - space);
                            droppedButtons.Add(grid[x][y2]);
                            toPositionYs.Add(toPositionY);
                            oldPositions.Add(grid[x][y2].GetPosition());
                            newPositions.Add(newPosition);
                            //for (var y3 = y2; y3 < grid[x].Count; ++y3)
                            //{
                            //    if (grid[x][y3] != null)
                            //    {
                            //        var rectTransform = grid[x][y3].GetComponent<RectTransform>();
                            //        var fromPositionY = rectTransform.anchoredPosition.y;
                            //        var toPositionY = fromPositionY - (rectTransform.sizeDelta.y * (y2 - y)) - (StageController.padding * (y2 - y));
                            //        var newPosition = new Vector2Int(grid[x][y3].GetPosition().x, grid[x][y3].GetPosition().y - (y2 - y));
                            //        droppedButtons.Add(grid[x][y3]);
                            //        toPositionYs.Add(toPositionY);
                            //        oldPositions.Add(grid[x][y3].GetPosition());
                            //        newPositions.Add(newPosition);
                            //    }
                            //}
                            //done = true;
                            //break;
                        }
                    }
                    break;
                    //if (done)
                    //{
                    //    break;
                    //}
                }
            }
        }
        if (droppedButtons.Count == 0)
        {
            completion();
        }
        var maxNotificationCount = droppedButtons.Count;
        var notificationCount = 0;
        var index = 0;
        foreach (var oldPosition in oldPositions)
        {
            grid[oldPosition.x][oldPosition.y] = null;
        }
        foreach (var droppedButton in droppedButtons)
        {
            //var oldPosition = oldPositions[index];
            var newPosition = newPositions[index];
            droppedButton.GetComponent<UIAnimator>().Animate(UIAnimator.AnimationKey.PositionY, 2f, toPositionYs[index], (termButton) =>
            {
                droppedButton.SetPosition(newPosition);
                //grid[oldPosition.x][oldPosition.y] = null;
                //grid[newPosition.x][newPosition.y] = termButton;
                grid[newPosition.x][newPosition.y] = droppedButton;
                ++notificationCount;
                if (notificationCount == maxNotificationCount)
                {
                    PerformCombo(completion);
                }
                return true;
            });
            ++index;
        }
    }

    private void PerformCombo(Func<bool> completion)
    {
        var positions = new List<List<Vector2Int>>();
        foreach (var termButton in termButtons)
        {
            var isOperandButton = termButton is OperandButton;
            if (!isOperandButton)
            {
                continue;
            }
            var y = termButton.GetPosition().y;
            var formula = new Formula();
            for (var x = termButton.GetPosition().x; x < grid.Count; ++x)
            {
                var t = grid[x][y];
                if (!formula.CanAppendTermButton(t))
                {
                    break;
                }
                formula.AppendTermButton(t);
                if (!formula.IsCorrect)
                {
                    continue;
                }
                var group = new List<Vector2Int>(); 
                for (var x2 = termButton.GetPosition().x; x2 <= x; ++x2)
                {
                    var position = new Vector2Int(x2, y);
                    group.Add(position);
                }
                var removedGroups = new List<List<Vector2Int>>();
                var ok = true;
                foreach (var g in positions)
                {
                    foreach (var position in g)
                    {
                        if (group.Contains(position))
                        {
                            if (group.Count <= g.Count)
                            {
                                ok = false;
                                
                            } else
                            {
                                removedGroups.Add(g);
                            }
                            break;
                        }
                    }
                    if (!ok)
                    {
                        break;
                    }
                }
                if (ok)
                {
                    foreach (var removedGroup in removedGroups)
                    {
                        positions.Remove(removedGroup);
                    }
                    positions.Add(group);
                }
            }
        }
        if (positions.Count == 0)
        {
            completion();
        }
        else
        {
            RemoveTermButtons(positions, completion, true);
        }
    }

    private void HandleOperandButton(OperandButton operandButton)
    {
        if (isAnimation || operandButton.isSelected || !formula.CanAppendOperand(operandButton.Operand))
        {
            SoundWareHouse.Instance.seButtonError.GetComponent<AudioSource>().Play();
            return;
        }
        SoundWareHouse.Instance.seTermButton.GetComponent<AudioSource>().Play();
        formula.AppendOperand(operandButton.Operand);
        formulaText.AppendOperand(operandButton.Operand);
        HandleTermButton(operandButton);
        if (formula.IsCorrect)
        {
            isAnimation = true;
            var targets = new List<List<Vector2Int>>() { selectedPositions };
            RemoveTermButtons(targets, () =>
            {
                isAnimation = false;
                selectedPositions.Clear();
                formula.Clear();
                formulaText.text = "";
                ++moves;
                movesText.text = moves.ToString() + "/" + stageData.moves.ToString() + "手";
                snapShots.RemoveAt(snapShots.Count - 1);
                if (IsClear)
                {
                    gameDirector.PerformClearTask();
                }
                else if ((!IsEmpty && (stageData.moves <= moves)) || (IsEmpty && (moves != stageData.moves)) || (stageData.moves < moves))
                {
                    gameDirector.ShowGameOverAlert();
                }
                else
                {
                    SaveSnapShot();
                }
                return true;
            });
        }
    }

    private void HandleOperatorButton(OperatorButton operatorButton)
    {
        if (isAnimation || operatorButton.isSelected || !formula.CanAppendOperator(operatorButton.Operator))
        {
            SoundWareHouse.Instance.seButtonError.GetComponent<AudioSource>().Play();
            return;
        }
        SoundWareHouse.Instance.seTermButton.GetComponent<AudioSource>().Play();
        formula.AppendOperator(operatorButton.Operator);
        formulaText.AppendOperator(operatorButton.Operator);
        HandleTermButton(operatorButton);
    }

    private void HandleEqualButton(EqualButton equalButton)
    {
        if (isAnimation || equalButton.isSelected || !formula.CanAppendEqual(equalButton.equal))
        {
            SoundWareHouse.Instance.seButtonError.GetComponent<AudioSource>().Play();
            return;
        }
        SoundWareHouse.Instance.seTermButton.GetComponent<AudioSource>().Play();
        formula.AppendEqual(equalButton.equal);
        formulaText.AppendEqual();
        HandleTermButton(equalButton);
    }

    private void HandleTermButton(TermButton termButton)
    {
        if (isAnimation || termButton.isSelected)
        {
            return;
        }
        selectedPositions.Add(termButton.GetPosition());
        termButton.isSelected = true;
        termButton.SetSelectedColor();
        AdvanceTurn();
    }

}
