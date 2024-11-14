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

        Debug.Log("���� ����");
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
    #region ����

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
            Debug.Log("����");
            mainParser.ParseDialogueList(EndingDefaultStory());
        }
    }

    private List<Dialogue> EndingDefaultStory()
    {
        var newDialogue = new List<Dialogue>();

        newDialogue.Add(new Dialogue("����", "......", false));
        newDialogue.Add(new Dialogue("����", "�¿�...��...������ 20�ְ� �����ْ�.", false));
        newDialogue.Add(new Dialogue("����", "���ƿ� �Բ��Ѣ�20�ִ� ��Ē�?", false));
        int nowMainIndex = Data.PlayerData.MainStoryIndexs[(int)Data.PlayerData.GetHigestMainStatName()];
        Debug.Log(nowMainIndex);
        newDialogue.Add(new Dialogue("����", "�翡�� ���Ѻ��⿣...���Ǹ��� �ð��� ���� �� ���ْ�!", false));
        newDialogue.Add(new Dialogue("����", $"���ƴ� {Managers.Data.PlayerData.nowSubCount}���� ���ϼ����� �Ŵ����� �Ǿ���", false));
        switch (Managers.Data.PlayerData.GetHigestMainStatName())
        {
            case StatName.Game:
                newDialogue.Add(new Dialogue("����", $"���ӿ� ����� ���� ", false));
                newDialogue.Add(new Dialogue("����", $"{Managers.Data.PlayerData.SixStat[0]}��ŭ�Ǣ�ɷ��� �����ϰ� �Ǿ��ْ�!", false));
                break;
            case StatName.Song:
                newDialogue.Add(new Dialogue("����", $"�뷡�� ����� ���� ", false));
                newDialogue.Add(new Dialogue("����", $"{Managers.Data.PlayerData.SixStat[1]}��ŭ�Ǣ�ɷ��� �����ϰ� �Ǿ��ْ�!", false));
                break;
            case StatName.Draw:
                newDialogue.Add(new Dialogue("����", $"�׸��� ����� ���� ", false));
                newDialogue.Add(new Dialogue("����", $"{Managers.Data.PlayerData.SixStat[2]}��ŭ�Ǣ�ɷ��� �����ϰ� �Ǿ��ْ�!", false));
                break;
        }
        newDialogue.Add(new Dialogue("����", $"������ ��� ���� ��{Managers.Data.PlayerData.SixStat[3]}��ŭ�� �ٷ���,", false));
        newDialogue.Add(new Dialogue("����", $"������ ����� ���� ��{Managers.Data.PlayerData.SixStat[4]}��ŭ�� ��Ż��,", false));
        newDialogue.Add(new Dialogue("����", $"������ �⵵�� ���� ��{Managers.Data.PlayerData.SixStat[5]}��ŭ�� ����� ����ْ�!", false));
        newDialogue.Add(new Dialogue("����", $"�׷� ���� ���ư� � �ḻ�� ������ϰ� �� �� �˾ƺ��� ���ڒ�!", false));


        return newDialogue;
    }

    #endregion

    #endregion



}
