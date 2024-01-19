using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

public class Managers : MonoBehaviour
{
    [Header("스텟 관련")]
    public int MainStat_ValuePerLevel;
    public float Str_Men_ValuePerLevel;
    static Managers s_instance;    
    public static Managers instance {get{Init(); return s_instance;}}
    

    ResourceManager _resource = new ResourceManager();
    UI_Manager _ui_manager = new UI_Manager();
    SoundManager _sound = new SoundManager();
    DataManager _data = new DataManager();

    

    REventManager _RE = new REventManager();
    public static ResourceManager Resource{get{return instance._resource;}}
    public static UI_Manager UI_Manager{get{return instance._ui_manager;}}
    public static SoundManager Sound{get{return instance._sound;}}
    public static DataManager Data { get { return instance._data; } }
    public static REventManager RandEvent { get { return instance._RE; } }


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
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        UI_Manager.Clear();
    }

    const string DayDatasURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=1890750354&range=B2:Q";
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
        UI_Manager.CloseALlPopupUI();
        yield return new WaitForEndOfFrame();
        UI_Manager.ShowPopupUI<UI_Reciept>();
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

    public MainStory ChooseMainStory()
    {
        MainStory mainStory;
        string temp = Data.PlayerData.GetHigestStatName().ToString();
        temp += (Data.PlayerData.NowWeek / 4).ToString();

        Debug.Log(temp);
        Enum.TryParse(temp, out mainStory);

        return mainStory;
    }


    #endregion


    //한 주차가 끝났을때 호출
    public Action WeekOverAction;

    public void FinishWeek()
    {
        WeekOverAction?.Invoke();
        Data.PlayerData.NowWeek++;
        Data.SaveData();
        UI_MainBackUI.instance.UpdateUItexts();
    }
}
