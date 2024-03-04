using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Communication : UI_Popup
{
    public static UI_Communication instance;

    public Sprite[] BubbleIMGs;
    public Sprite[] CharIMGs;
    TMPro.TMP_Text dialogueText;
    public List<Dialogue> dialogues;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public float TypingDelay;
    private WaitForSeconds typingDelay;
    private int currentDialogueIndex = 0;

    public float TargetX;
    public float MoveTime;
    public float PeriodScale;

    public bool IsWeeklyCommunication = false;
    bool isEnd;

    enum Texts { sentenceTMP, Option1TMP, Option2TMP }
    enum Images 
    {   LeftIMG, 
        RightIMG,
        ChatBubbleIMG
    }

    enum Buttons
    {
        Option1BTN,
        Option2BTN,
        SkipBTN
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
        GetButton(2).onClick.AddListener(SkipBTN);
    }

    void Update()
    {
        // 화면을 터치하면 다음 대사로 넘어가기
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            if (UI_DefaultPopup.instance != null &&  UI_DefaultPopup.instance.gameObject.activeInHierarchy) return;

            if (isTyping)
            {
                // 글자 출력 중이라면 모든 대사 한 번에 보여주기
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogues[currentDialogueIndex].sentence;
                isTyping = false;
            }
            else
            {
                // 다음 대사로 넘어가기
                NextDialogue();
            }
        }
    }

    #region SkipBTN
    void SkipBTN()
    {
        Managers.UI_Manager.ClosePopupUI();
        Managers.Sound.Play(Sound.Skip);
    }

    public void HideSkipBTN()
    {
        GetButton(2).gameObject.SetActive(false);
    }
    #endregion


    public void StartDiagloue(List<Dialogue> DiaList)
    {
        dialogues = DiaList;
        ShowDialogue(dialogues[0]);
    }

    void ShowDialogue(Dialogue dialogue)
    {
        // 대사 출력을 위한 코루틴 실행
        typingCoroutine = StartCoroutine(TypeSentence(dialogue));
    }

    void NextDialogue()
    {
        if(!isEnd)
        {
            // 다음 대사 인덱스로 이동
            currentDialogueIndex++;

            // 대사가 모두 끝났을 경우 대화 종료
            if (currentDialogueIndex >= dialogues.Count)
            {
                EndDialogue();
                return;
            }

            //선택지 출력 단계
            if (dialogues[currentDialogueIndex].name == "1")
            {
                ShowOptionBTN();
                return;
            }

            // 다음 대사 출력
            ShowDialogue(dialogues[currentDialogueIndex]);
        }
    }

    void EndDialogue()
    {
        if(IsWeeklyCommunication && !Managers.Data.PlayerData.WeeklyCommunicationRewarded)
        {
            UI_DefaultPopup.WeeklyCommuncationEnd();
            return;
        }
        Managers.UI_Manager.ClosePopupUI();
    }

    IEnumerator TypeSentence(Dialogue dialogue)
    {
        ChooseBubbleIMG(dialogue);
        ShowImage(dialogue);

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
        if(dialogue.name == "유저")
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[0];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[1];
        }
        else if(dialogue.name == "루비아")
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[2];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[3];
        }
        else if(dialogue.name == "뮹뮹")
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

    void TurnOnImage(bool isLeft, Sprite sprite)
    {
        GetImage((int)Images.LeftIMG).gameObject.SetActive(isLeft);
        GetImage((int)Images.LeftIMG).sprite = isLeft ? sprite : null;
        GetImage((int)Images.LeftIMG).color = isLeft ? Color.white : Color.gray;

        GetImage((int)Images.RightIMG).gameObject.SetActive(!isLeft);
        GetImage((int)Images.RightIMG).sprite = !isLeft ? sprite : null;
        GetImage((int)Images.RightIMG).color = !isLeft ? Color.white : Color.gray;
    }

    void ShowOptionBTN()
    {
        isEnd = true;

        var option1Button = GetButton((int)Buttons.Option1BTN);
        var option2Button = GetButton((int)Buttons.Option2BTN);
        var option1Text = GetText((int)Texts.Option1TMP);
        var option2Text = GetText((int)Texts.Option2TMP);

        option1Button.transform.DOLocalMoveX(TargetX, MoveTime).SetEase(Ease.OutElastic, 0, PeriodScale);
        option2Button.transform.DOLocalMoveX(TargetX, MoveTime).SetEase(Ease.OutElastic, 0, PeriodScale);

        option1Text.text = dialogues[currentDialogueIndex].sentence;
        option2Text.text = dialogues[currentDialogueIndex + 1].sentence;

        if (dialogues[currentDialogueIndex].UserHasEnoughGold())
        {
            option1Button.interactable = true;
        }
        else
        {
            option1Button.interactable = false;
            option1Button.GetComponent<Image>().sprite = option1Button.spriteState.pressedSprite;
        }

        if (dialogues[currentDialogueIndex + 1].UserHasEnoughGold())
        {
            option2Button.interactable = true;
        }
        else
        {
            option2Button.interactable = false;
            option1Button.GetComponent<Image>().sprite = option1Button.spriteState.pressedSprite;
        }
        option1Button.onClick.AddListener(ShowOption1);
        option2Button.onClick.AddListener(ShowOption2);
    }

    void ShowOption1()
    {
        if (dialogues[currentDialogueIndex].IsNeedGold())
        {
            Managers.Sound.Play(Sound.Buy);
            Managers.Data.PlayerData.nowGoldAmount -= dialogues[currentDialogueIndex].CostGold;

            for (int i = 0; i < dialogues[currentDialogueIndex + 2].rewardStats.Count; i++)
            {
                Managers.Data.PlayerData.StatUpByDialogue(dialogues[currentDialogueIndex + 2].rewardStats[i]);
            }
        }
        else
        {
            Managers.Sound.Play(Sound.SmallBTN);
            for (int i = 0; i < dialogues[currentDialogueIndex + 2].rewardStats.Count; i++)
            {
                Managers.Data.PlayerData.StatUpByDialogue(dialogues[currentDialogueIndex + 2].rewardStats[i]);
            }
        }
        UI_DefaultPopup.SetDefaultPopupUI(DefaultPopupState.Normal, dialogues[currentDialogueIndex + 2].sentence, "임시");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
    }

    void ShowOption2()
    {
        if (dialogues[currentDialogueIndex + 1].IsNeedGold())
        {
            Managers.Sound.Play(Sound.Buy);
            Managers.Data.PlayerData.nowGoldAmount -= dialogues[currentDialogueIndex + 1].CostGold;

            for (int i = 0; i < dialogues[currentDialogueIndex + 3].rewardStats.Count; i++)
            {
                Managers.Data.PlayerData.StatUpByDialogue(dialogues[currentDialogueIndex + 3].rewardStats[i]);
            }
        }
        else
        {
            Managers.Sound.Play(Sound.SmallBTN);
            for (int i = 0; i < dialogues[currentDialogueIndex + 3].rewardStats.Count; i++)
            {
                Managers.Data.PlayerData.StatUpByDialogue(dialogues[currentDialogueIndex + 3].rewardStats[i]);
            }
        }
        UI_DefaultPopup.SetDefaultPopupUI(DefaultPopupState.Normal, dialogues[currentDialogueIndex + 3].sentence, "임시");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
    }

}