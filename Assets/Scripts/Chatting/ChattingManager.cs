using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using static UI_SchedulePopup;

public class ChattingManager : MonoBehaviour//
{
    #region URL&StringLists

    const string NameURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=A2:A";
    List<string> ViewersNameList = new List<string>();

    const string GaneChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=B2:B";
    List<string> GameChatList = new List<string>();

    const string SingChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=C2:C";
    List<string> SingChatList = new List<string>();

    const string JustChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=D2:D";
    List<string> JustChatList = new List<string>();

    const string HorrorChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=E2:E";
    List<string> HorrorChatList = new List<string>();

    const string CookChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=F2:F";
    List<string> CookChatList = new List<string>();

    const string ChallengeChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=G2:G";
    List<string> ChallengeChatList = new List<string>();

    const string NewCloChatURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&range=H2:H";
    List<string> NewCloChatList = new List<string>();

    #endregion

    public static ChattingManager instance;
    private void Awake()
    {
        instance = this;
    }

    List<GameObject> ChatGOs = new List<GameObject>();
    public GameObject ClearChatGO;

    [SerializeField] float _chatScale;

    void Start()
    {
        //배열에 집어넣고 비활성화
        int childCount = gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            GameObject childObject = childTransform.gameObject;
            ChatGOs.Add(childObject);
            childObject.SetActive(false);
        }
        StartCoroutine(RequestListDatasFromSheet());
    }
    


    IEnumerator RequestListDatasFromSheet()
    {
        //비동기 방식으로 서버에서 데이터를 읽어옴
        Coroutine chatCoroutine = StartCoroutine(RequestAndSetDatas(GaneChatURL, GameChatList));
        Coroutine nameCoroutine = StartCoroutine(RequestAndSetDatas(NameURL, ViewersNameList));
        Coroutine singCoroutine = StartCoroutine(RequestAndSetDatas(SingChatURL, SingChatList));
        Coroutine justchatcoroutine = StartCoroutine(RequestAndSetDatas(JustChatURL, JustChatList));
        Coroutine horrorCoroutine = StartCoroutine(RequestAndSetDatas(HorrorChatURL, HorrorChatList));
        Coroutine cookCorout = StartCoroutine(RequestAndSetDatas(CookChatURL, CookChatList));
        Coroutine ChallengeCorou = StartCoroutine(RequestAndSetDatas(ChallengeChatURL, ChallengeChatList));

        //코루틴이 모두 완료될 때까지 기다림
        yield return chatCoroutine;
        yield return nameCoroutine;
        yield return singCoroutine;
        yield return justchatcoroutine;
        yield return horrorCoroutine;
        yield return cookCorout;
        yield return ChallengeCorou;

    }

    IEnumerator RequestAndSetDatas(string www, List<string> list)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length - 1).Split('\n');

        foreach(string line in lines)
        {
            string templine = line.Substring(0, line.Length - 1);
            int count = 0;
            string modifiedLine = "";

            foreach (char c in templine)
            {
                modifiedLine += c;

                count++;

                //9글자 이상일때 띄어쓰기거나
                if (count >= 9 && c == ' ')
                {
                    modifiedLine = modifiedLine.Substring(0, modifiedLine.Length - 1);
                    modifiedLine += "\n";
                    count = 0;
                }
                //11글자가 넘어가면 강제로 줄바꿈
                else if(count > 11)
                {
                    modifiedLine += "\n";
                    count = 0;
                }
            }

            // 마지막 글자가 줄바꿈 문자인 경우 제거
            if (modifiedLine.EndsWith("\n"))
            {
                modifiedLine = modifiedLine.Substring(0, modifiedLine.Length - 1);
            }
            list.Add(modifiedLine);
        }
    }

    //방송 타입을 받아서 방송 시작함
    public void StartGenerateChattingByType(BroadCastType broadCastType)
    {
        switch (broadCastType)
        {
            case BroadCastType.Game:
                StartCoroutine(StartGenerateChatting(GameChatList));
                break;

            case BroadCastType.Song:
                StartCoroutine(StartGenerateChatting(SingChatList));
                break;

            case BroadCastType.Chat:
                StartCoroutine(StartGenerateChatting(JustChatList));
                break;

            case BroadCastType.Horror:
                StartCoroutine(StartGenerateChatting(HorrorChatList));
                break;

            case BroadCastType.Cook:
                StartCoroutine(StartGenerateChatting(CookChatList));
                break;

            case BroadCastType.GameChallenge:
                StartCoroutine(StartGenerateChatting(ChallengeChatList));
                break;

            case BroadCastType.NewClothe:
                StartCoroutine(StartGenerateChatting(NewCloChatList));
                break;
        }

    }

    [Header("채팅 사이의 시간")]
    [SerializeField] float minChatDelayTime;
    [SerializeField] float maxChatDelayTime;
    [Header("채팅 사이 간격")]
    [SerializeField] float SpaceBetweenChats;


    //채팅 시작 구현
    IEnumerator StartGenerateChatting(List<string> messagelist)
    {
        int index = 0;
        
        while(true)
        {
            //새 창 정보 세팅(사이즈 조절용)
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
                    rectTransform.DOAnchorPos(targetPosition, 0.3f);
                }
            }
            yield return new WaitForSeconds(0.3f);

            //새 메시지 밑에 생성
            yield return StartCoroutine(MakeRandomChat(index, tempMessage));

            index++;
            if (index == ChatGOs.Count) index = 0;

            float temp = Random.Range(minChatDelayTime, maxChatDelayTime);
            yield return new WaitForSeconds(temp);
        }
    }

    [Header("채팅이 커지는 시간")]
    [SerializeField] float TimeForChatGetBigger;
    float ChatGenerateYPoz = -40.54f;

    /// <summary>
    /// qwer
    /// </summary>
    /// <param name="index"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    IEnumerator MakeRandomChat(int index, string message)
    {
        Vector3 targetScale = Vector3.one * _chatScale;
        GameObject Go = ChatGOs[index];

        Go.SetActive(true);
        Go.transform.localScale = Vector3.zero;
        Go.GetComponent<RectTransform>().anchoredPosition = new Vector3(-46f, ClearChatGO.transform.GetComponent<RectTransform>().sizeDelta.y/2f* _chatScale + ChatGenerateYPoz, 0);

        Go.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = GetRandomStringFromList(ViewersNameList);
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
