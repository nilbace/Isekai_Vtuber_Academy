using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;


public class UI_Tutorial : UI_Popup, IPointerClickHandler
{
    public static UI_Tutorial instance;
    public Sprite[] BubbleIMGs;
    public Sprite[] CharIMGs;
    TMPro.TMP_Text dialogueText;
    public List<Dialogue> dialogues;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public float TypingDelay;
    private WaitForSeconds typingDelay;
    private int currentDialogueIndex = 0;
    Button NowSelctedBTN;


    bool isEnd;

    enum Texts { sentenceTMP, Option1TMP, Option2TMP }
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
        typingDelay = new WaitForSeconds(TypingDelay);


        GetImage((int)Images.LeftIMG).gameObject.SetActive(false);
        GetImage((int)Images.RightIMG).gameObject.SetActive(false);
        dialogueText = GetText((int)Texts.sentenceTMP);
    }

    void Update()
    {
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        //{
        //    // 화면을 터치하면 다음 대사로 넘어가기
        //    if (isTyping)
        //    {
        //        // 글자 출력 중이라면 모든 대사 한 번에 보여주기
        //        StopCoroutine(typingCoroutine);
        //        dialogueText.text = dialogues[currentDialogueIndex].sentence;
        //        isTyping = false;
        //    }
        //    //대사가 끝난 상태에서
        //    else
        //    {
        //        //Focus포인트가 있다면
        //        if (dialogues[currentDialogueIndex].tutorialFocus != TutorialFocusPoint.MaxCount)
        //        {
        //            //넘어가는건 OnPointerClick에 있음
        //            return;
        //        }
        //        //Focus포인트가 없다면 다음 대사로 넘어감
        //        else
        //        {
        //            NextDialogue();
        //        }
        //    }
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTyping)
        {
            // 글자 출력 중이라면 모든 대사 한 번에 보여주기
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[currentDialogueIndex].sentence;
            isTyping = false;
        }
        else
        {
            if (dialogues[currentDialogueIndex].tutorialFocus != TutorialFocusPoint.MaxCount)
            {
                // 터치한 GameObject가 img인지 확인
                if (eventData.pointerCurrentRaycast.gameObject == GetImage((int)Images.BlackIMG).gameObject)
                {
                    if (NowSelctedBTN != null)
                    {
                        Debug.Log("버튼임");
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

        //원본

        //// 터치한 GameObject가 img인지 확인
        //if (eventData.pointerCurrentRaycast.gameObject == GetImage((int)Images.BlackIMG).gameObject)
        //{
        //    if (NowSelctedBTN != null)
        //    {
        //        NowSelctedBTN.onClick.Invoke();
        //    }
        //    NextDialogue();
        //}
    }

    public void StartDiagloue(List<Dialogue> DiaList)
    {
        dialogues = DiaList;
        ShowDialogue(dialogues[0]);
    }

    void ShowDialogue(Dialogue dialogue)
    {
        ChooseBubbleIMG(dialogue);
        ShowImage(dialogue);
        SetFocusImg(dialogue);
        // 대사 출력을 위한 코루틴 실행
        typingCoroutine = StartCoroutine(TypeSentence(dialogue));
    }

    void NextDialogue()
    {
        if (!isEnd)
        {
            // 다음 대사 인덱스로 이동
            currentDialogueIndex++;

            // 대사가 모두 끝났을 경우 대화 종료
            if (currentDialogueIndex >= dialogues.Count)
            {
                EndDialogue();
                return;
            }

            // 다음 대사 출력
            ShowDialogue(dialogues[currentDialogueIndex]);
        }
    }

    void EndDialogue()
    {
        Managers.UI_Manager.ClosePopupUI();
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
        else
        {
            GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[6];
        }
    }

    void ShowImage(Dialogue dialogue)
    {
        if (dialogue.name == "유저")
        {
            TurnOnImage(dialogue.isLeft, CharIMGs[0]);
        }
        else if (dialogue.name == "루비아")
        {
            TurnOnImage(dialogue.isLeft, CharIMGs[1]);
        }
        else
        {
            TurnOnImage(dialogue.isLeft, CharIMGs[2]);
        }
    }

    void SetFocusImg(Dialogue dialogue)
    {
        
        switch (dialogue.tutorialFocus)
        {
            case TutorialFocusPoint.StartSchedule:
                SetFocusImg("CreateScheduleBTN");
                break;
            case TutorialFocusPoint.Screen:
                SetFocusImg("ScreenIMG");
                break;
            case TutorialFocusPoint.BroadcastBTN:
                break;
            case TutorialFocusPoint.RestBTN:
                break;
            case TutorialFocusPoint.GoOutBTN:
                break;
            case TutorialFocusPoint.Healing:
                break;
            case TutorialFocusPoint.LOL:
                break;
            case TutorialFocusPoint.Sketch:
                break;
            case TutorialFocusPoint.BC_Draw:
                break;
            case TutorialFocusPoint.MaxCount:
                FocusImgDisappear();
                break;
        }
    }

    GameObject SetFocusImg(string Objectname)
    {
        var FocusImg = GetImage((int)Images.FocusIMG);
        GameObject go = GameObject.Find(Objectname);
        FocusImg.sprite = go.GetComponent<Image>().sprite;
        FocusImg.GetComponent<RectTransform>().sizeDelta = go.GetComponent<RectTransform>().sizeDelta;
        FocusImg.GetComponent<RectTransform>().anchoredPosition = go.GetComponent<RectTransform>().anchoredPosition;

        Button button = go.GetComponent<Button>();

        // Button 컴포넌트가 있는지 확인
        if (button != null)
        {
            NowSelctedBTN = button;
            Debug.Log("버튼 있다");
        }
        else
        {
            NowSelctedBTN = button;
            Debug.Log("버튼 없다");
        }

        return go;
    }

    void FocusImgDisappear()
    {
        var FocusImg = GetImage((int)Images.FocusIMG);
        FocusImg.GetComponent<RectTransform>().sizeDelta = new Vector3(0, 0, 0);
    }

    void TurnOnImage(bool isLeft, Sprite sprite)
    {
        GetImage((int)Images.LeftIMG).gameObject.SetActive(isLeft);
        GetImage((int)Images.LeftIMG).sprite = isLeft ? sprite : null;
        GetImage((int)Images.LeftIMG).color = isLeft ? Color.white : Color.gray;

        GetImage((int)Images.RightIMG).gameObject.SetActive(!isLeft);
        GetImage((int)Images.RightIMG).sprite = !isLeft ? sprite : null;
        GetImage((int)Images.RightIMG).color = !isLeft ? Color.white : Color.gray;
    }

    
}