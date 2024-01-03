using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using static Define;
public class ChattingManager : MonoSingleton<ChattingManager>
{
    const string Message_NameURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=0&range=A2:J";

    List<string>[] Message_NameListArray = new List<string>[(int)BroadCastType.MaxCount_Name + 1];

    public float ChatBubbleRiseDuration = 0.3f;
    [Header("채팅 사이의 시간")]
    [SerializeField] float minChatDelayTime;
    public float maxChatDelayTime;
    [Header("채팅 사이 간격")]
    [SerializeField] float SpaceBetweenChats;

    

    [Header("채팅이 커지는 시간")]
    public float TimeForChatGetBigger;

    [Header("채팅 시작좌표")]
    [SerializeField] float ChatBubbleYPos;
    [SerializeField] float ChatBubbleXPos;

    private void Awake()
    {
        base.Awake();
    }

    List<GameObject> ChatGOs = new List<GameObject>();
    public GameObject ClearChatGO;

    [SerializeField] float _chatScale;

    void Start()
    {
        for (int i = 0; i < Message_NameListArray.Length; i++)
        {
            Message_NameListArray[i] = new List<string>();
        }

        //배열에 집어넣고 비활성화
        int childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            ChatGOs.Add(childObject);
        }
        StartCoroutine(RequestListDatasFromSheet());
    }

    public void OnEnable()
    {
        foreach(GameObject ChatGO in ChatGOs)
        {
            ChatGO.SetActive(false);
        }
    }


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
            LocateDataToMessageListArray(datas);
        }

        for (int i = 0; i < (int)BroadCastType.MaxCount_Name; i++)
        {
            Message_NameListArray[i] = AutoLineBreak(Message_NameListArray[i]);
        }
    }

    void LocateDataToMessageListArray(string datas)
    {
        string[] EachData = datas.Substring(0, datas.Length).Split('\t');
        for (int i = 0; i < (int)BroadCastType.MaxCount_Name + 1; i++)
        {
            if (EachData[i] != "")
            {
                Message_NameListArray[i].Add(EachData[i]);
            }
        }
    }


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
        StartCoroutine(StartGenerateChatting(Message_NameListArray[(int)broadCastType]));
    }


    

    IEnumerator StartGenerateChatting(List<string> messagelist)
    {
        int index = 0;

        while (true)
        {
            //투명 창 정보 세팅(사이즈 조절용)
            string tempMessage = GetRandomStringFromList(messagelist);
            ClearChatGO.GetComponent<TMPro.TMP_Text>().text = tempMessage;
            yield return new WaitForEndOfFrame();
            float newYoffset = ClearChatGO.GetComponent<RectTransform>().sizeDelta.y + SpaceBetweenChats;
            newYoffset *= _chatScale;

            //맨 위에놈 끔
            ChatGOs[(index + ChatGOs.Count + 1) % ChatGOs.Count].SetActive(false);

            //나머지 전부 위로 올림
            foreach (GameObject go in ChatGOs)
            {
                if (go.activeSelf)
                {
                    var rectTransform = go.GetComponent<RectTransform>();
                    var targetPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + newYoffset);
                    rectTransform.DOAnchorPos(targetPosition, ChatBubbleRiseDuration);
                }
            }
            yield return new WaitForSeconds(ChatBubbleRiseDuration);

            //새 메시지 밑에 생성
            yield return StartCoroutine(MakeRandomChat(index, tempMessage));

            index++;
            if (index == ChatGOs.Count) index = 0;

            float temp = Random.Range(minChatDelayTime, maxChatDelayTime);
            yield return new WaitForSeconds(temp);
        }
    }

    

    IEnumerator MakeRandomChat(int index, string message)
    {
        Vector3 targetScale = Vector3.one * _chatScale;
        GameObject Go = ChatGOs[index];

        Go.SetActive(true);
        Go.transform.localScale = Vector3.zero;
        Go.GetComponent<RectTransform>().anchoredPosition = new Vector3(ChatBubbleXPos, ClearChatGO.transform.GetComponent<RectTransform>().sizeDelta.y/2f* _chatScale + ChatBubbleYPos, 0);

        Go.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = GetRandomStringFromList(Message_NameListArray[(int)BroadCastType.MaxCount_Name]);
        Go.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = message;
        ClearChatGO.GetComponent<TMPro.TMP_Text>().text = Go.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text;

        var tween = Go.transform.DOScale(targetScale, TimeForChatGetBigger);
        yield return tween.WaitForCompletion();
    }

    string GetRandomStringFromList(List<string> list)
    {
        int rand = Random.Range(0, list.Count);
        return list[rand];
    }
}
