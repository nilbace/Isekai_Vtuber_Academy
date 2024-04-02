using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;


public class UI_Tutorial : UI_Popup, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static UI_Tutorial instance;
    public Sprite[] BubbleIMGs;
    public Sprite[] CharIMGs;
    TMPro.TMP_Text dialogueText;
    public List<Dialogue> dialogues;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public float TypingDelay;
    private WaitForSecondsRealtime typingDelay;
    [SerializeField] private int currentDialogueIndex = 0;
    Button NowSelctedBTN;
    bool NowSelectedBTNPressed = false;

    bool isEnd;

    public AlphaLerp alphaLerp;
    enum Texts { sentenceTMP, }
    enum Images
    {
        LeftIMG,
        RightIMG,
        FocusIMG,
        BlackIMG,
        ChatBubbleIMG
    }

    enum Buttons
    {
        Option1BTN,
        Option2BTN,
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        typingDelay = new WaitForSecondsRealtime(TypingDelay);


        //영수증 띄우기용 2주차 설정
        Managers.Data.PlayerData.NowWeek = 2;
        GetImage((int)Images.LeftIMG).gameObject.SetActive(false);
        GetImage((int)Images.RightIMG).gameObject.SetActive(false);
        dialogueText = GetText((int)Texts.sentenceTMP);
    }


    #region Pointer
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == GetImage((int)Images.BlackIMG).gameObject)
        {
            if (NowSelctedBTN != null && !NowSelectedBTNPressed)
            {
                NowSelectedBTNPressed = true;
                IPointerDownHandler[] pointerDownHandlers = NowSelctedBTN.GetComponents<IPointerDownHandler>();
                foreach (IPointerDownHandler down in pointerDownHandlers)
                {
                    down.OnPointerDown(eventData);
                }
            }
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (NowSelctedBTN != null && NowSelectedBTNPressed)
        {
            NowSelectedBTNPressed = false;
            IPointerUpHandler[] pointerUpHandlers = NowSelctedBTN.GetComponents<IPointerUpHandler>();
            foreach (IPointerUpHandler up in pointerUpHandlers)
            {
                up.OnPointerUp(eventData);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEnd) return;
        if (isTyping)
        {
            // 글자 출력 중이라면 모든 대사 한 번에 보여주기
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[currentDialogueIndex].sentence;
            isTyping = false;
        }
        else
        {
            if(dialogues[currentDialogueIndex].name == "대기")
            {
                return;
            }
            else if (dialogues[currentDialogueIndex].name == "진행")
            {
                Time.timeScale = 1;
            }
            

            //포커스 포인트가 있고, interactable이라면은
            if (dialogues[currentDialogueIndex].tutorialFocus != TutorialFocusPoint.MaxCount && dialogues[currentDialogueIndex].IsInteractable)
            {
                //포커스 된 부분을 잘 클릭 했다면
                if (eventData.pointerCurrentRaycast.gameObject == GetImage((int)Images.BlackIMG).gameObject)
                {
                    if (NowSelctedBTN != null)
                    {
                        NowSelctedBTN.onClick.Invoke();
                    }
                    NextDialogue();
                }
            }
            //Focus포인트가 없다면 다음 대사로 넘어감
            else
            {
                NextDialogue();
            }
        }
    }
    #endregion

    public void StartDiagloue(List<Dialogue> DiaList)
    {
        dialogues = DiaList;
        StartCoroutine(ShowDialogue(dialogues[0]));
    }

    IEnumerator ShowDialogue(Dialogue dialogue)
    {
        yield return new WaitForEndOfFrame();
        NowSelctedBTN = null;
        ChooseBubbleIMG(dialogue);
        ShowCharImage(dialogue);
        SetFocusImg(dialogue);
        SetCharAndBubbleYPoz(dialogue.Ypoz);
        // 대사 출력을 위한 코루틴 실행
        typingCoroutine = StartCoroutine(TypeSentence(dialogue));
    }

    public void NextDialogue()
    {
        if (!isEnd)
        {
            // 다음 대사 인덱스로 이동
            currentDialogueIndex++;
            // 대사가 모두 끝났을 경우 대화 종료
            if (currentDialogueIndex >= dialogues.Count)
            {
                isEnd = true;
                Debug.Log("끝남");
                return;
            }

            

            // 다음 대사 출력
            StartCoroutine(ShowDialogue(dialogues[currentDialogueIndex]));
        }
    }

    

    IEnumerator TypeSentence(Dialogue dialogue)
    {
        // 글자 출력 중임을 표시
        isTyping = true;

        // 대사 초기화
        dialogueText.text = "";

        int chatIndex = 0;

        foreach (char letter in dialogue.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return typingDelay;

            Managers.Sound.Play(Define.Sound.Chat1);
            chatIndex++;
        }
        // 글자 출력 완료 후 대사 정보 업데이트
        isTyping = false;
    }


    void ChooseBubbleIMG(Dialogue dialogue)
    {
        if (dialogue.name == "유저")
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[0];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[1];
        }
        else if (dialogue.name == "루비아")
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[2];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[3];
        }
        else if (dialogue.name == "뮹뮹")
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[4];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[5];
        }
        //빈칸은 투명 이미지
        else
        {
            GetImage((int)Images.ChatBubbleIMG).sprite = CharIMGs[13];
        }
    }

    void ShowCharImage(Dialogue dialogue)
    {
        bool isLeft = dialogue.isLeft;
        Sprite sprite = CharIMGs[(int)dialogue.Apperance];

        GetImage((int)Images.LeftIMG).gameObject.SetActive(isLeft);
        GetImage((int)Images.LeftIMG).sprite = isLeft ? sprite : null;
        GetImage((int)Images.LeftIMG).color = isLeft ? Color.white : Color.gray;

        GetImage((int)Images.RightIMG).gameObject.SetActive(!isLeft);
        GetImage((int)Images.RightIMG).sprite = !isLeft ? sprite : null;
        GetImage((int)Images.RightIMG).color = !isLeft ? Color.white : Color.gray;
    }

    void SetCharAndBubbleYPoz(string poz)
    {
        if (poz == "위")
        {
            GetImage((int)Images.LeftIMG).rectTransform.anchoredPosition = new Vector2(-75, 170);
            GetImage((int)Images.RightIMG).rectTransform.anchoredPosition = new Vector2(80, 170);
            GetImage((int)Images.ChatBubbleIMG).rectTransform.anchoredPosition = new Vector2(0, 72);
        }
        else if (poz == "중간")
        {
            GetImage((int)Images.LeftIMG).rectTransform.anchoredPosition = new Vector2(-75, 5);
            GetImage((int)Images.RightIMG).rectTransform.anchoredPosition = new Vector2(80, 5);
            GetImage((int)Images.ChatBubbleIMG).rectTransform.anchoredPosition = new Vector2(0, -100);
        }
        else if (poz == "아래")
        {
            GetImage((int)Images.LeftIMG).rectTransform.anchoredPosition = new Vector2(-75, -32);
            GetImage((int)Images.RightIMG).rectTransform.anchoredPosition = new Vector2(80, -32);
            GetImage((int)Images.ChatBubbleIMG).rectTransform.anchoredPosition = new Vector2(0, -136);
        }
        else
            return;
    }

    //특정 선택지들은 선택박스가 딸려옴
    void SetFocusImg(Dialogue dialogue)
    {
        switch (dialogue.tutorialFocus)
        {
            case TutorialFocusPoint.Sketch:
                SetFocusImg("Sketch", dialogue.IsInteractable);
                StartCoroutine(FollowSubcontent("Sketch", dialogue.IsInteractable));
                break;

            case TutorialFocusPoint.Song:
                SetFocusImg("Song", dialogue.IsInteractable);
                StartCoroutine(FollowSubcontent("Song", dialogue.IsInteractable));
                break;

            case TutorialFocusPoint.BaseDraw:
                SetFocusImg("BaseDraw", dialogue.IsInteractable);
                StartCoroutine(FollowSubcontent("BaseDraw", dialogue.IsInteractable));
                break;
            case TutorialFocusPoint.BaseSong:
                SetFocusImg("BaseSong", dialogue.IsInteractable);
                StartCoroutine(FollowSubcontent("BaseSong", dialogue.IsInteractable));
                break;

            case TutorialFocusPoint.StartScheduleBTN:
                alphaLerp.maxAlpha = 0;
                SetFocusImg(dialogue.tutorialFocus.ToString(), dialogue.IsInteractable);
                break;

            case TutorialFocusPoint.RecieptBackGroundIMG:
                alphaLerp.maxAlpha = 0.36f;
                SetFocusImg(dialogue.tutorialFocus.ToString(), dialogue.IsInteractable);
                break;


            case TutorialFocusPoint.MaxCount:
                FocusImgDisappear();
                break;



            default:
                SetFocusImg(dialogue.tutorialFocus.ToString(), dialogue.IsInteractable);
                break;

        }
    }

    IEnumerator FollowSubcontent(string name, bool interactable)
    {
        float duration = 0.18f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            SetFocusImg(name, interactable);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void SetFocusImg(string Objectname, bool interactable)
    {
        var FocusImg = GetImage((int)Images.FocusIMG);
        GameObject go = FindGo(Objectname);
        Transform parent = go.GetComponent<RectTransform>().parent;
        RectTransform parentRect = go.transform.parent.GetComponent<RectTransform>();
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        FocusImg.sprite = go.GetComponent<Image>().sprite;
        FocusImg.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
        FocusImg.GetComponent<RectTransform>().anchoredPosition = go.GetComponent<RectTransform>().anchoredPosition;
        if (parentRect != null && parentRect.anchoredPosition != Vector2.zero)
        {
            FocusImg.GetComponent<RectTransform>().anchoredPosition += parent.GetComponent<RectTransform>().anchoredPosition;
        }

        if (parent.GetComponent<HorizontalLayoutGroup>() != null)
        {
            FocusImg.GetComponent<RectTransform>().anchoredPosition = GetAnchoredPositionRelativeToParent(go);
        }

        if (interactable)
        {
            Button button = go.GetComponent<Button>();

            // Button 컴포넌트가 있는지 확인.
            if (button != null)
            {
                NowSelctedBTN = button;
            }
            else
            {
                NowSelctedBTN = button;
            }
        }
    }

    GameObject FindGo(string name)
    {
        GameObject temp = null;
        GameObject contentGo = GameObject.Find("Content");
        switch (name)
        {
            case "Healing":
                temp = contentGo.transform.GetChild(0).gameObject;
                break;
            case "LOL":
                temp = contentGo.transform.GetChild(1).gameObject;
                break;
            case "Sketch":
                temp = contentGo.transform.GetChild(7).gameObject;
                break;
            case "Song":
                temp = contentGo.transform.GetChild(4).gameObject;
                break;
            case "BaseGame":
                temp = contentGo.transform.GetChild(0).gameObject;
                break;
            case "BaseSong":
                temp = contentGo.transform.GetChild(3).gameObject;
                break;
            case "BaseDraw":
                temp = contentGo.transform.GetChild(6).gameObject;
                break;
            default:
                temp = GameObject.Find(name);
                break;
        }
        return temp;
    }



    Vector2 GetAnchoredPositionRelativeToParent(GameObject gameObject)
    {
        //Layout에 속해있는 N번째 오브젝트.
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        //Layout컴포넌트를 들고있고 얼마나 움직였는지 체크용
        RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

        //ScrollRect컴포넌트 및 가장 기본적인 Offset
        RectTransform grandparentRectTransform = parentRectTransform.parent.parent.GetComponent<RectTransform>();

        float halfWidth = grandparentRectTransform.rect.width * 0.5f;
        float paddingOffset = parentRectTransform.GetComponent<HorizontalLayoutGroup>().padding.left;
        float positionOffset = parentRectTransform.anchoredPosition.x;
        float currentPosition = -halfWidth + paddingOffset;

        int childIndex = -1;
        for (int i = 0; i < parentRectTransform.childCount; i++)
        {
            if (parentRectTransform.GetChild(i).gameObject == gameObject)
            {
                childIndex = i;
                break;
            }
            currentPosition += parentRectTransform.GetChild(i).GetComponent<RectTransform>().rect.width;
            currentPosition += parentRectTransform.GetComponent<HorizontalLayoutGroup>().spacing;
        }

        if (childIndex != -1)
        {
            currentPosition += rectTransform.rect.width * 0.5f;
        }

        return new Vector2(currentPosition + positionOffset, 0) + grandparentRectTransform.anchoredPosition;
    }


    void FocusImgDisappear()
    {
        var FocusImg = GetImage((int)Images.FocusIMG);
        FocusImg.GetComponent<RectTransform>().sizeDelta = new Vector3(0, 0, 0);
    }

  
    private void OnDisable()
    {
        Managers.Data.PersistentUser.WatchedTutorial = true;
    }
}