using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Communication : UI_Popup
{
    public Sprite[] BubbleIMGs;
    public Sprite[] CharIMGs;
    TMPro.TMP_Text dialogueText;
    public Dialogue[] dialogues;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public float TypingDelay;
    private WaitForSeconds typingDelay;
    private int currentDialogueIndex = 0;
    
    enum Texts { sentenceTMP }
    enum Images 
    { LeftIMG, RightIMG,
        ChatBubbleIMG }

    private void Start()
    {
        Init();
        typingDelay = new WaitForSeconds(TypingDelay);
        ShowDialogue(dialogues[currentDialogueIndex]);

    }

    public override void Init()
    {
        base.Init();
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetImage((int)Images.LeftIMG).gameObject.SetActive(false);
        GetImage((int)Images.RightIMG).gameObject.SetActive(false);
        dialogueText = GetText((int)Texts.sentenceTMP);
    }

    void Update()
    {
        // 화면을 터치하면 다음 대사로 넘어가기
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
            || Input.GetMouseButtonDown(0))
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
                // 다음 대사로 넘어가기
                NextDialogue();
            }
        }
    }

    void ShowDialogue(Dialogue dialogue)
    {
        // 대사 출력을 위한 코루틴 실행
        typingCoroutine = StartCoroutine(TypeSentence(dialogue));
    }

    void NextDialogue()
    {
        // 다음 대사 인덱스로 이동
        currentDialogueIndex++;

        // 대사가 모두 끝났을 경우 대화 종료
        if (currentDialogueIndex >= dialogues.Length)
        {
            EndDialogue();
            return;
        }

        // 다음 대사 출력
        ShowDialogue(dialogues[currentDialogueIndex]);
    }

    void EndDialogue()
    {
        Debug.Log("HI");
        Managers.UI_Manager.CloseALlPopupUI();
    }

    IEnumerator TypeSentence(Dialogue dialogue)
    {
        ChooseBubbleIMG(dialogue);
        ShowImage(dialogue);

        // 글자 출력 중임을 표시
        isTyping = true;

        // 대사 초기화
        dialogueText.text = "";

        // 한 글자씩 출력
        foreach (char letter in dialogue.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return typingDelay; // 밖에서 선언한 WaitForSeconds 변수 재사용
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
        else
        {
            if (dialogue.isLeft) GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[4];
            else GetImage((int)Images.ChatBubbleIMG).sprite = BubbleIMGs[5];
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
        if (isLeft)
        {
            GetImage((int)Images.LeftIMG).gameObject.SetActive(true);
            GetImage((int)Images.LeftIMG).sprite = sprite;
            GetImage((int)Images.LeftIMG).color = Color.white;
            GetImage((int)Images.RightIMG).color = Color.gray;
        }
        else
        {
            GetImage((int)Images.RightIMG).gameObject.SetActive(true);
            GetImage((int)Images.RightIMG).sprite = sprite;
            GetImage((int)Images.LeftIMG).color = Color.gray;
            GetImage((int)Images.RightIMG).color = Color.white;
        }
    }
}