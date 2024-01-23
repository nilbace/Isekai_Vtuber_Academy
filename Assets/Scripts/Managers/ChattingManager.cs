using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Define;
public class ChattingManager : MonoSingleton<ChattingManager>
{
    const string Message_NameURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=0&range=A2:J";

    Dictionary<BroadCastType, List<string>> ChatMessage_NameDic = new Dictionary<BroadCastType, List<string>>();
    List<GameObject> ChatGOs = new List<GameObject>();

    //직접 텍스트가 들어가기 전까지 알기 어려운 width, height값 미리 체크용 GameObject
    public GameObject TransparentChatBox;

    [HideInInspector] public float ChatBubbleRiseDuration;
    [HideInInspector] public float MaxChatDelayTime;
    [HideInInspector] public float SpaceBetweenChats;
    [HideInInspector] public float TimeForChatGetBigger;
    const float ChatBoxYPos = -88f;
    const float ChatBoxXPos = -121f;
        
    void Start()
    {
        for (int i = 0; i < (int)BroadCastType.MaxCount_Name + 1; i++)
        {
            ChatMessage_NameDic[(BroadCastType)i] = new List<string>();
        }

        foreach (Transform childTransform in transform)
        {
            GameObject childObject = childTransform.gameObject;
            ChatGOs.Add(childObject);
        }
        StartCoroutine(RequestListDatasFromSheet());

        ScheduleExecuter.Inst.SetAniSpeedAction -= SetChatboxAniSpeed;
        ScheduleExecuter.Inst.SetAniSpeedAction += SetChatboxAniSpeed;
    }

    void SetChatboxAniSpeed(int speed)
    {
        MaxChatDelayTime = 0.1f / (float)speed;
        TimeForChatGetBigger = 0.08f / (float)speed;
        ChatBubbleRiseDuration = 0.3f / (float)speed;
    }

    //활성화, 비활성화로 관리되며 활성화 시 원래 채팅창들 전부 안보이게 처리하기 위함
    public void OnEnable()
    {
        foreach(GameObject ChatGO in ChatGOs)
        {
            ChatGO.SetActive(false);
        }
    }

    #region Temp
    IEnumerator RequestListDatasFromSheet()
    {
        Coroutine chatCoroutine = StartCoroutine(RequestAndSetDatas(Message_NameURL));

        yield return chatCoroutine;
        gameObject.SetActive(false);
    }

    IEnumerator RequestAndSetDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length).Split('\n');

        foreach (string datas in lines)
        {
            SetDataToDictionary(datas);
        }

        for (int i = 0; i < (int)BroadCastType.MaxCount_Name; i++)
        {
            ChatMessage_NameDic[(BroadCastType)i] = AutoLineBreak(ChatMessage_NameDic[(BroadCastType)i]);
        }
    }
    #endregion

    void SetDataToDictionary(string datas)
    {
        string[] EachData = datas.Substring(0, datas.Length).Split('\t');
        for (int i = 0; i < (int)BroadCastType.MaxCount_Name + 1; i++)
        {
            if (EachData[i] != "" && EachData[i] != "\r")
            {
                ChatMessage_NameDic[(BroadCastType)i].Add(EachData[i]);
            }
        }
    }

    //자동 줄바꿈
    public List<string> AutoLineBreak(List<string> lines)
    {
        List<string> result = new List<string>();

        foreach (string line in lines)
        {
            string templine = line;
            int count = 0;
            string modifiedLine = "";

            foreach (char c in templine)
            {
                modifiedLine += c;
                count++;

                if (count >= 9 && c == ' ')
                {
                    modifiedLine = modifiedLine.TrimEnd();
                    modifiedLine += "\n";
                    count = 0;
                }
                else if (count > 11)
                {
                    modifiedLine += "\n";
                    count = 0;
                }
            }

            if (modifiedLine.EndsWith("\n"))
            {
                modifiedLine = modifiedLine.Substring(0, modifiedLine.Length - 1);
            }
            result.Add(modifiedLine);
        }
        return result;
    }

    public void StartGenerateChattingByType(BroadCastType broadCastType)
    {
        StartCoroutine(StartGenerateChatting(ChatMessage_NameDic[broadCastType]));
    }

    IEnumerator StartGenerateChatting(List<string> messageList)
    {
        int index = 0;

        while (true)
        {
            // 투명 창 정보 설정 (사이즈 조절용)
            string tempMessage = GetRandomStringFromList(messageList);
            TransparentChatBox.GetComponent<TMPro.TMP_Text>().text = tempMessage;
            yield return new WaitForEndOfFrame();
            float newYOffset = TransparentChatBox.GetComponent<RectTransform>().sizeDelta.y + SpaceBetweenChats;

            // 맨 위의 채팅 비활성화
            ChatGOs[(index + ChatGOs.Count + 1) % ChatGOs.Count].SetActive(false);

            // 나머지 채팅을 위로 이동
            foreach (GameObject go in ChatGOs)
            {
                if (go.activeSelf)
                {
                    var rectTransform = go.GetComponent<RectTransform>();
                    var targetPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + newYOffset);
                    rectTransform.DOAnchorPos(targetPosition, ChatBubbleRiseDuration);
                }
            }
            yield return new WaitForSeconds(ChatBubbleRiseDuration);

            // 새로운 메시지 생성
            yield return StartCoroutine(MakeRandomChat(index, tempMessage));

            index++;
            if (index == ChatGOs.Count)
                index = 0;

            float temp = Random.Range(0, MaxChatDelayTime);
            yield return new WaitForSeconds(temp);
        }
    }


    IEnumerator MakeRandomChat(int nowBoxIndex, string message)
    {
        GameObject Go = ChatGOs[nowBoxIndex];
        TMPro.TMP_Text nameText = Go.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>();
        TMPro.TMP_Text chatText = Go.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>();
        float yPoz = TransparentChatBox.transform.GetComponent<RectTransform>().sizeDelta.y / 2f + ChatBoxYPos;

        //활성화 시 크기와 스케일 값 조절
        Go.SetActive(true);
        Go.transform.localScale = Vector3.zero;
        Go.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChatBoxXPos, yPoz, 0);

        // 이름과 채팅 내용 설정
        nameText.text = GetRandomStringFromList(ChatMessage_NameDic[BroadCastType.MaxCount_Name]);
        chatText.text = message;

        // ClearChatGO에 텍스트 설정
        TransparentChatBox.GetComponent<TMPro.TMP_Text>().text = chatText.text;

        //다시 커지는 애니메이션
        var tween = Go.transform.DOScale(Vector3.one, TimeForChatGetBigger);
        yield return tween.WaitForCompletion();
    }

    string GetRandomStringFromList(List<string> list)
    {
        int rand = Random.Range(0, list.Count);
        return list[rand];
        
    }
}
