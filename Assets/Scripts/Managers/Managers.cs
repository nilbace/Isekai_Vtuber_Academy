using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

public class Managers : MonoBehaviour
{
    [SerializeField] private int _mainStatUpValuePerTier;
    [SerializeField] private float _str_MenDownValuePerTier;
    [SerializeField] private int _bigSuccessProbabilityPerTier;
    [SerializeField] private float _bigSuccessCoefficientValuePerTier;

    public int MainStat_ValuePerLevel => _mainStatUpValuePerTier;
    public float Str_Men_ValuePerLevel => _str_MenDownValuePerTier;
    public int BigSuccessProbability => _bigSuccessProbabilityPerTier;
    public float BigSuccessCoefficientValue => _bigSuccessCoefficientValuePerTier;



    static Managers s_instance;    
    public static Managers instance {get{Init(); return s_instance;}}
    

    ResourceManager _resource = new ResourceManager();
    UI_Manager _ui_manager = new UI_Manager();
    SoundManager _sound = new SoundManager();
    DataManager _data = new DataManager();
    NicknameManager _nickname = new NicknameManager();
    SceneMManager _scene = new SceneMManager();
    

    REventManager _RE = new REventManager();
    public static ResourceManager Resource{get{return instance._resource;}}
    public static UI_Manager UI_Manager{get{return instance._ui_manager;}}
    public static SoundManager Sound{get{return instance._sound;}}
    public static DataManager Data { get { return instance._data; } }
    public static REventManager RandEvent { get { return instance._RE; } }
    public static NicknameManager NickName { get { return instance._nickname; } }
    public static SceneMManager Scene { get { return instance._scene; } }


    void Awake()
    {
        Init();
        StartCoroutine(LoadDatas());
    }


    static void Init(){
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go==null)
            {
                go = new GameObject{name = "@Managers"};
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._sound.Init();
            s_instance._data.Init();
            s_instance._scene.Init();
        }
    }

    const string DayDatasURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=1890750354&range=B2:S";
    const string RandEventURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=185260022&range=A2:AA";
    const string MerchantURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=1267834452&range=A2:L";

    IEnumerator LoadDatas()
    {
        Coroutine cor1 = StartCoroutine(s_instance._data.RequestAndSetDayDatas(DayDatasURL));
        Coroutine cor2 = StartCoroutine(s_instance._data.RequestAndSetRandEventDatas(RandEventURL)); 
        Coroutine cor3 = StartCoroutine(s_instance._data.RequestAndSetItemDatas(MerchantURL));

        yield return cor1;
        yield return cor2;
        yield return cor3;

        Debug.Log("시작 가능");
    }


    #region UI
    public void ShowReceipt()
    {
        StartCoroutine(ShowReceiptCor());
    }
    IEnumerator ShowReceiptCor()
    {
        if(UI_Tutorial.instance == null)
            UI_Manager.CloseALlPopupUI();

        yield return new WaitForEndOfFrame();
        UI_Manager.ShowPopupUI<UI_Reciept>();
    }

    public void ShowSelectNickName()
    {
        StartCoroutine(ShowSelectNickNameCor());
    }
    IEnumerator ShowSelectNickNameCor()
    {
        UI_Manager.ClosePopupUI();
             yield return new WaitForEndOfFrame();
        UI_Manager.ShowPopupUI<UI_SelectNickName>();
    }

    public void CloseTitle()
    {
        if (Managers.Data.PersistentUser.WatchedTutorial == false)
            StartCoroutine(ShowTutorialCor());
        else
            Managers.UI_Manager.ClosePopupUI();
    }
    IEnumerator ShowTutorialCor()
    {
        UI_Manager.CloseALlPopupUI();
        yield return new WaitForEndOfFrame();
        UI_Manager.ShowPopupUI<UI_Tutorial>();
    }

    public void StartSchedule()
    {
        StartCoroutine(ScheduleExecuter.Inst.StartSchedule());
    }

    public void ShowMainStory()
    {
        StartCoroutine(ShowMainStoryCor());
    }
    IEnumerator ShowMainStoryCor()
    {
        UI_Manager.ShowPopupUI<UI_Communication>();
        yield return new WaitForEndOfFrame();
        MainStoryParser.Inst.StartStory(ChooseMainStory());
    }

    public int ChooseMainStory()
    {
        var HighestMainStat = Data.PlayerData.GetHigestMainStatName();

        return Data.PlayerData.MainStoryIndexs[(int)HighestMainStat];
    }
    #region 엔딩

    public void ShowEndingStory()
    {
        StartCoroutine(ShowEndingStoryCor());
    }
    private    IEnumerator ShowEndingStoryCor()
    {
        UI_Manager.ClosePopupUI();

        var story = UI_Manager.ShowPopupUI<UI_Communication>();
        story.isEndingStory = true;
        var mainParser = story.GetComponent<MainStoryParser>();
        yield return new WaitForEndOfFrame();
        if (mainParser != null)
        {
            Debug.Log("성공");
            mainParser.ParseDialogueList(EndingDefaultStory());
        }
    }

    private List<Dialogue> EndingDefaultStory()
    {
        var newDialogue = new List<Dialogue>();

        newDialogue.Add(new Dialogue("뮹뮹", "......", false));
        newDialogue.Add(new Dialogue("뮹뮹", "뮤웅...뮹...€어느덧 20주가 지났다뮹.", false));
        newDialogue.Add(new Dialogue("뮹뮹", "루비아와 함께한€20주는 어땟냐뮹?", false));
        int nowMainIndex = Data.PlayerData.MainStoryIndexs[(int)Data.PlayerData.GetHigestMainStatName()];
        Debug.Log(nowMainIndex);
        newDialogue.Add(new Dialogue("뮹뮹", "곁에서 지켜보기엔...€훌륭한 시간을 보낸 것 같다뮹!", false));
        newDialogue.Add(new Dialogue("뮹뮹", $"루비아는 {Managers.Data.PlayerData.nowSubCount}명의 €하수인을 거느리게 되었고", false));
        switch (Managers.Data.PlayerData.GetHigestMainStatName())
        {
            case StatName.Game:
                newDialogue.Add(new Dialogue("뮹뮹", $"게임에 재능을 보여 ", false));
                newDialogue.Add(new Dialogue("뮹뮹", $"{Managers.Data.PlayerData.SixStat[0]}만큼의€능력을 보유하게 되었다뮹!", false));
                break;
            case StatName.Song:
                newDialogue.Add(new Dialogue("뮹뮹", $"노래에 재능을 보여 ", false));
                newDialogue.Add(new Dialogue("뮹뮹", $"{Managers.Data.PlayerData.SixStat[1]}만큼의€능력을 보유하게 되었다뮹!", false));
                break;
            case StatName.Draw:
                newDialogue.Add(new Dialogue("뮹뮹", $"그림에 재능을 보여 ", false));
                newDialogue.Add(new Dialogue("뮹뮹", $"{Managers.Data.PlayerData.SixStat[2]}만큼의€능력을 보유하게 되었다뮹!", false));
                break;
        }
        newDialogue.Add(new Dialogue("뮹뮹", $"꾸준한 운동을 통해 €{Managers.Data.PlayerData.SixStat[3]}만큼의 근력을,", false));
        newDialogue.Add(new Dialogue("뮹뮹", $"차분한 명상을 통해 €{Managers.Data.PlayerData.SixStat[4]}만큼의 멘탈을,", false));
        newDialogue.Add(new Dialogue("뮹뮹", $"간절한 기도을 통해 €{Managers.Data.PlayerData.SixStat[5]}만큼의 행운을 얻었다뮹!", false));
        newDialogue.Add(new Dialogue("뮹뮹", $"그럼 이제 루비아가 어떤 결말을 €맞이하게 될 지 알아보러 가자뮹!", false));


        return newDialogue;
    }

    #endregion

    #endregion



}
