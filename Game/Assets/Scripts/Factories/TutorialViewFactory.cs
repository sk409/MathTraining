using UnityEngine;
using UnityEngine.UI;

public class TutorialViewFactory
{

    private class Component
    {
        public string title;
        public string content;
        public Sprite[] sprites;

        public Component(string title, string content, Sprite sprite)
        {
            this.title = title;
            this.content = content;
            this.sprites = new Sprite[] { sprite };
        }

        public Component(string title, string content, Sprite[] sprites)
        {
            this.title = title;
            this.content = content;
            this.sprites = sprites;
        }

    }

    public static GameObject Make(GameObject canvasObject, CurtainManager curtainManager)
    {
        GameObject tutorialView = null;
        var curtainObject = curtainManager.gameObject;

        tutorialView = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.tutorialView));
        tutorialView.transform.SetParent(canvasObject.transform, false);
        var closeTutorialButtonObject = tutorialView.transform.Find(ResourceNames.closeTutorialButton);
        var closeTutorialButtonRectTransform = closeTutorialButtonObject.GetComponent<RectTransform>();
        var closeTutorialButtonAnchoredPositionX = -Screen.width * 0.05f;
        var closeTutorialButtonAnchoredPositionY = closeTutorialButtonAnchoredPositionX;
        closeTutorialButtonRectTransform.anchoredPosition = new Vector2(
            closeTutorialButtonAnchoredPositionX,
            closeTutorialButtonAnchoredPositionY
            );
        var closeTutorialButtonWidth = Screen.width * 0.08f;
        var closeTutorialButtonHeight = closeTutorialButtonWidth;
        closeTutorialButtonRectTransform.sizeDelta = new Vector2(
            closeTutorialButtonWidth,
            closeTutorialButtonHeight
            );
        var closeButton = closeTutorialButtonObject.GetComponent<Button>();
        closeButton.onClick.AddListener(() => {
            curtainObject.transform.SetAsLastSibling();
            curtainManager.Close(() =>
            {
                Object.Destroy(tutorialView);
                curtainManager.Open(() =>
                {
                    curtainManager.transform.SetAsFirstSibling();
                    return true;
                });
                return true;
            });
        });

        var scrollContentObject = tutorialView.transform.Find(ResourceNames.tutorialScrollRect).Find(ResourceNames.tutorialScrollViewport).Find(ResourceNames.tutorialScrollContent);
        float MakeComponents(float top, string title, string content, Sprite[] sprites, bool withScrollGuide)
        {
            var pointer = top;
            var padding = Screen.height * 0.05f;
            var margin = padding * 1.41421356f;
            void MakeTextObject(string name, string str, int fontSize)
            {
                var textObject = Object.Instantiate(Resources.Load<GameObject>(name));
                textObject.transform.SetParent(scrollContentObject.transform, false);
                var text = textObject.GetComponent<Text>();
                text.text = str;
                text.fontSize = fontSize;
                var rectTransform = textObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0f, pointer);
                rectTransform.sizeDelta = new Vector2(
                    Mathf.Min(Screen.width, text.preferredWidth),
                    text.preferredHeight
                    );
                rectTransform.sizeDelta = new Vector2(
                    Mathf.Min(Screen.width, text.preferredWidth),
                    text.preferredHeight
                    );
                pointer -= (rectTransform.sizeDelta.y + padding);
            }
            MakeTextObject(ResourceNames.tutorialTitleText, title, FontSizes.Medium);
            MakeTextObject(ResourceNames.tutorialContentText, content, FontSizes.Small);
            var spriteIndex = 0;
            foreach (var sprite in sprites)
            {
                var imageObject = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.tutorialImage));
                imageObject.transform.SetParent(scrollContentObject.transform, false);
                var imageRectTransform = imageObject.GetComponent<RectTransform>();
                imageRectTransform.anchoredPosition = new Vector2(0f, pointer);
                var imageWidth = Screen.width * 0.6f;
                var imageHeight = imageWidth / 1.5f;
                imageRectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
                var image = imageObject.GetComponent<Image>();
                image.sprite = sprite;
                pointer -= (imageRectTransform.sizeDelta.y + padding);
                if (spriteIndex == 0 && withScrollGuide)
                {
                    var scrollGuide = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.tutorialScrollGuide));
                    scrollGuide.transform.SetParent(scrollContentObject.transform, false);
                    var scrollGuideRectTransform = scrollGuide.GetComponent<RectTransform>();
                    var halfSpace = (Screen.width - imageRectTransform.sizeDelta.x) / 2f;
                    var scrollGuideAnchoredPositionX = (imageRectTransform.sizeDelta.x / 2f) + (halfSpace / 2f);
                    var scrollGuideHeight = imageHeight;
                    var scrollGuideWidth = scrollGuideHeight / 4f;
                    scrollGuideRectTransform.anchoredPosition = new Vector2(
                        scrollGuideAnchoredPositionX,
                        imageRectTransform.anchoredPosition.y
                        );
                    scrollGuideRectTransform.sizeDelta = new Vector2(
                        scrollGuideWidth,
                        scrollGuideHeight
                        );
                }
                ++spriteIndex;
            }
            pointer -= margin;
            return pointer;
        }

        var components = new Component[]
        {
            new Component(
                "1.クリア条件の確認",
                "ゲームを始めるとクリア条件が表示されるので確認しましょう。\n今回は「2手で全消し」です。\nこの後の説明にもありますが、ボタンをクリックして式を1つ作成すると1手に数えられます。\nこの条件と同じ手数ですべてのボタンを消すとゲームクリアになります。\nゲームスタートボタンを押すとこのクリア条件確認画面を閉じることができます。",
                Resources.Load<Sprite>(ResourceNames.tutorialClearCondition)
                ),
            new Component(
                "2.式の作成",
                "クリア条件確認画面を閉じるとこの画面になるので、ボタンをクリックして式を作成しましょう。\nボタンは好きな位置にあるものをクリックできます。\n作成途中の式は画面上部に表示されるので確認しながら作業を進めましょう。\nもしクリア条件を忘れてしまったら画面右下にある「?」ボタンでクリア条件確認画面を表示することができます。",
                Resources.Load<Sprite>(ResourceNames.tutorialCreateFormula)
                ),
            new Component(
                "3.ボタンの削除",
                "ボタンをクリックして式を作成できると、このようにオレンジの枠で囲まれて、徐々に消えていきます。\nこのように式を1つ作成すると1手に数えられます。\nこの後の処理がすべて終わると画面真ん中より少し右上にある手数を表示している部分に反映されます。",
                Resources.Load<Sprite>(ResourceNames.tutorialRemoveButton)
                ),
            new Component(
                "4.ボタンの落下",
                "全てのボタンが消えると、上にあったボタンが落下します。\nボタンは下に新たなボタンが現れるか、最下部まで落下します。",
                Resources.Load<Sprite>(ResourceNames.tutorialDropButton)
                ),
            new Component(
                "5.コンボの発生",
                "全てのボタンが落下した後に、連続した位置に式がそろっていれば、手数を増やさずにボタンを消すことができます。\nこれ以降「4.ボタンの落下」と「5.コンボの発生」を繰り返します。",
                Resources.Load<Sprite>(ResourceNames.tutorialCombo)
                ),
            new Component(
                "6.ゲームクリア・ゲームオーバーの確認",
                "「4.ボタンの落下」と「5.コンボの発生」の繰り返しが終了した時点で手数が画面真ん中より少し右上にある手数を表示する部分に反映されます。\nもし現在の手数とクリア条件の手数が同じで、すべてのボタンを消すことができていた場合、ゲームクリアです。\nもし現在の手数とクリア条件の手数が同じで、すべてのボタンを消すことができていなかった場合や、現在の手数がクリア条件の手数よりも少ない段階で全てのボタンを消してしまった場合はゲームオーバーです。\nゲームクリアでもゲームオーバーでもない場合には、また「2.式の作成」から「6.ゲームクリア・ゲームオーバーの確認」を繰り返します。\n今回はまだクリアしていないので、「2.式の作成」に戻ります。",
                Resources.Load<Sprite>(ResourceNames.tutorialRestart)
                ),
            new Component(
                "7.戻る、進むボタン",
                "ボタンのクリックを1つだけ戻したい場合には、画面右の「←」ボタンで戻ることができます。\nこのボタンは1つ前のクリックがあればいつでも押して戻ることができます。\nもし戻りすぎてしまった場合には、「←」ボタンの上にある「→」ボタンで1つ進むことができます。\n「→」ボタンも「←」ボタンで戻っていればいつでも押して進むことができます。",
                Resources.Load<Sprite>(ResourceNames.tutorialBackward)
                ),
            new Component(
                "8-1.ゲームクリア",
                "もしゲームをクリアすることができたら、ゲームクリア画面が表示されます。\nこの画面から次のステージに進んだり、ステージ選択画面に戻ったり、タイトル画面に戻ったりすることができます。",
                new Sprite[] { Resources.Load<Sprite>(ResourceNames.tutorialRestart), Resources.Load<Sprite>(ResourceNames.tutorialClearWay1) , Resources.Load<Sprite>(ResourceNames.tutorialClearWay2), Resources.Load<Sprite>(ResourceNames.tutorialGameClear) }
                ),
            new Component(
                "8-2.ゲームオーバー",
                "もしゲームオーバーになってしまった場合にはゲームオーバー画面が表示されます。\nゲームオーバー画面からは「ステージ選択」と「直前に戻る」を選択することができます。\n「直前に戻る」を押すと、最後のボタンをクリックする前に戻ることができます。\nこの状態からまたゲームクリアを目指していきましょう。",
                new Sprite[] { Resources.Load<Sprite>(ResourceNames.tutorialGameOverWay), Resources.Load<Sprite>(ResourceNames.tutorialGameOver), Resources.Load<Sprite>(ResourceNames.tutorialGameOverBack), Resources.Load<Sprite>(ResourceNames.tutorialGameOverRestart),  }
                ),
        };
        var y = 0f;
        var componentIndex = 0;
        foreach (var component in components)
        {
            y = MakeComponents(
                y,
                component.title,
                component.content,
                component.sprites,
                componentIndex == 0
                );
            ++componentIndex;
        }

        var tutorialEndTextObject = Object.Instantiate(Resources.Load<GameObject>(ResourceNames.tutorialEndText));
        tutorialEndTextObject.transform.SetParent(scrollContentObject.transform, false);
        var tutorialEndText = tutorialEndTextObject.GetComponent<Text>();
        tutorialEndText.fontSize = FontSizes.Medium;
        var tutorialEndTextRectTransform = tutorialEndTextObject.GetComponent<RectTransform>();
        tutorialEndTextRectTransform.anchoredPosition = new Vector2(0f, y);
        tutorialEndTextRectTransform.sizeDelta = new Vector2(
            tutorialEndText.preferredWidth,
            tutorialEndText.preferredHeight
            );
        tutorialEndTextRectTransform.sizeDelta = new Vector2(
            tutorialEndText.preferredWidth,
            tutorialEndText.preferredHeight
            );

        y -= tutorialEndTextRectTransform.sizeDelta.y;

        var scrollContentRectTransform = scrollContentObject.GetComponent<RectTransform>();
        var scrollContentSize = scrollContentRectTransform.sizeDelta;
        scrollContentSize.y = -y;
        scrollContentRectTransform.sizeDelta = scrollContentSize;

        curtainObject.transform.SetAsLastSibling();
        curtainManager.Open(() =>
        {
            curtainObject.transform.SetAsFirstSibling();
            return true;
        });

        return tutorialView;
    }
}
