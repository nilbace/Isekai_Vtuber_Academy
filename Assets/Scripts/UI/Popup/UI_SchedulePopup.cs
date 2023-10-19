using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
using static Define;
using DG.Tweening;

/// <summary>
/// 스케쥴 관리와 방송 정보에 대한 정보가 담겨있는 스크립트
/// </summary>
public class UI_SchedulePopup : UI_Popup
{
    public static UI_SchedulePopup instance;
    public Transform ParentTR;
    public GameObject UISubContent;
    bool SubContentSelectPhase = false;
    private ScrollRect scrollRect;

    enum Buttons
    {
        MondayBTN,
        TuesdayBTN,
        WednesdayBTN,
        ThursdayBTN,
        FridayBTN,
        SaturdayBTN,
        SundayBTN,

        BroadCastBTN,
        RestBTN,
        GoOutBTN,

        StartScheduleBTN,
        BackBTN,

    }
    enum Texts
    {
        PointText,
        ScoreText,
    }
    enum GameObjects
    {
        Days7,
        Contents3,
        SubContents,
        Sub0, Sub1, Sub2, Sub3, 
    }
    enum Images
    {
        ItemIcon,
        ScheduleSlotSelected,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Init();
    }

    Sprite[] DaysImages = new Sprite[4];

    /// <summary>
    /// 방송 휴식 외출 선택
    /// </summary>
    void State_SelectType()
    {
        SubContentSelectPhase = false;
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
    }

    /// <summary>
    /// 하위 컨텐츠 선택
    /// </summary>
    void State_SelectSubContent()
    {
        SubContentSelectPhase = true;
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents).SetActive(true);
    }
    public override void Init()
    {
        base.Init();

        DaysImages = Resources.LoadAll<Sprite>("Days");

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));


        for (int i = 0; i<7; i++)
        {
            int inttemp = i;
            Button temp = GetButton(i);
            temp.onClick.AddListener( () => ClickDay(inttemp));
        }

        scrollRect = GetComponentInChildren<ScrollRect>();
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetButton((int)Buttons.BroadCastBTN).onClick.       AddListener(ClickBroadCastBTN);
        GetButton((int)Buttons.RestBTN).onClick.            AddListener(ClickRestBTN);
        GetButton((int)Buttons.GoOutBTN).onClick.           AddListener(ClickGoOutBTN);
        GetButton((int)Buttons.StartScheduleBTN).onClick.   AddListener(()=>StartCoroutine(StartSchedule()));
        GetButton((int)Buttons.BackBTN).onClick.            AddListener(BackBTN);

        _SeveDayScrollVarValue = Managers.Data._SeveDayScrollVarValue;
        _SevenDayScheduleDatas = Managers.Data._SevenDayScheduleDatas;

        SetSelectImg();
        UpdateInteractableButton();
        ClickLastDay_PlusOne();
    }

    #region ScheduleCheck
    public enum SevenDays {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public enum ScheduleType
    {
        Null, BroadCast, Rest, GoOut
    }

    SevenDays _nowSelectedDay = SevenDays.Monday;
    OneDayScheduleData[] _SevenDayScheduleDatas = new OneDayScheduleData[7];
    float[] _SeveDayScrollVarValue =  new float[7];

    public void StoreScrollVarValue(float value)
    {
        _SeveDayScrollVarValue[(int)_nowSelectedDay] = value;
        Managers.Data._SeveDayScrollVarValue = _SeveDayScrollVarValue;
    }

    void SetSelectImg()
    {
        int i = 0;
        for (; i < 7; i++)
        {
            if (_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        if (i == 7) i = 6;
        GetImage(1).transform.DOMoveX(((int)_nowSelectedDay - 3) * 40, 0f);
    }
    
    void ClickDay(int i)
    {
        _nowSelectedDay = (SevenDays)i;
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] != null)
        {
            if(_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ScheduleType.BroadCast)
            {
                ClickBroadCastBTN();
            }
            else if (_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ScheduleType.Rest)
            {
                ClickRestBTN();
            }
            else
            {
                ClickGoOutBTN();
            }
        }
        else
        {
            State_SelectType();
        }
        UpdateColorAndSelected();
    }

    void UpdateInteractableButton()
    {
        int i = 0;

        for(;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        if (i == 7) i = 6;
        for(int j = 0; j<7;j++)
        {
            GetButton(j).interactable = (j <= i) ? true : false;
        }
    }
    [SerializeField] Ease ease;
    [SerializeField] float moveDuration;
    /// <summary>
    /// 색상 지정용 함수
    /// </summary>
    void UpdateColorAndSelected()
    {
        for(int i = 0; i<7;i++)
        {
            if(i == (int)_nowSelectedDay)
            {
                GetImage(1).transform.DOMoveX((i - 3) * 40, moveDuration).SetEase(ease);
            }

            if(_SevenDayScheduleDatas[i] == null)
            {
                GetButton(i).GetComponent<Image>().sprite = DaysImages[3];
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.BroadCast)
            {
                GetButton(i).GetComponent<Image>().sprite = DaysImages[0];
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.Rest)
            {
                GetButton(i).GetComponent<Image>().sprite = DaysImages[1];
            }
            else
            {
                GetButton(i).GetComponent<Image>().sprite = DaysImages[2];
            }
        }
    }

    #region Schedules
    public class OneDayScheduleData 
    {
        public string KorName;
        public string infotext;
        public ScheduleType scheduleType;
        public BroadCastType broadcastType;
        public RestType restType;
        public GoOutType goOutType;
        public float FisSubsUpValue;
        public float PerSubsUpValue;
        public float HealthPointChangeValue;
        public float MentalPointChageValue;
        public float InComeMag;
        public int MoneyCost;
        public int[] Six_Stats;

        public OneDayScheduleData()
        {
            KorName = "";
            this.scheduleType = ScheduleType.Null;
            this.broadcastType = BroadCastType.MaxCount;
            this.restType = RestType.MaxCount;
            this.goOutType = GoOutType.MaxCount;
            this.infotext = "";
            FisSubsUpValue = 0;
            PerSubsUpValue = 0;
            HealthPointChangeValue = 0;
            MentalPointChageValue = 0;
            InComeMag = 0;
            MoneyCost = 0;
            Six_Stats = new int[6];
        }
    }

    public enum BroadCastType
    {
        Game, Song, Chat, Horror, Cook, GameChallenge, NewClothe, MaxCount
    }

    public enum RestType
    { 
        hea1, hea2,hea3, men1, men2, men3, MaxCount
    }

    public enum GoOutType
    {
        game1, game2, game3,     song1, song2, song3,    chat1, chat2, chat3,
        hea1, hea2, hea3,        men1, men2, men3,       luck1, luck2, luck3,
        MaxCount
    }

    void ClickBroadCastBTN()
    {
        SubContentSelectPhase = true;
        State_SelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.BroadCast);
    }
    void ClickRestBTN()
    {
        SubContentSelectPhase = true;
        State_SelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.Rest);
    }

    void ClickGoOutBTN()
    {
        SubContentSelectPhase = true;
        State_SelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.GoOut);
    }

    List<OneDayScheduleData> nowSelectScheduleTypeList = new List<OneDayScheduleData>();
    void ChooseScheduleTypeAndFillList(ScheduleType type)
    {
        nowSelectScheduleTypeList.Clear();
        DeleteAllChildren();
        switch (type)
        {
            case ScheduleType.BroadCast:
                for (int i = 0; i < (int)BroadCastType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((BroadCastType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((BroadCastType)i),_SevenDayScheduleDatas[(int)_nowSelectedDay]);
                }
                break;

            case ScheduleType.Rest:
                for (int i = 0; i < (int)RestType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((RestType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((RestType)i), _SevenDayScheduleDatas[(int)_nowSelectedDay]);
                }
                break;

            case ScheduleType.GoOut:
                for (int i = 0; i < (int)GoOutType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((GoOutType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((GoOutType)i), _SevenDayScheduleDatas[(int)_nowSelectedDay]);
                }
                break;
        }
        StartCoroutine(moveScroll());
    }

    IEnumerator moveScroll()
    {
        yield return new WaitForEndOfFrame();
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] != null)
        {
            scrollRect.horizontalScrollbar.value = _SeveDayScrollVarValue[(int)_nowSelectedDay];
        }
    }

    /// <summary>
    /// 세부 컨텐츠들 전부 삭제 > 다시 만들기 전에
    /// </summary>
    public void DeleteAllChildren()
    {
        int childCount = ParentTR.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(ParentTR.GetChild(i).gameObject);
        }
    }

    public void SetDaySchedule(OneDayScheduleData data)
    {
        _SevenDayScheduleDatas[(int)_nowSelectedDay] = data;
        Managers.Data._SevenDayScheduleDatas = _SevenDayScheduleDatas;
        //저장소 저장

        ClickLastDay_PlusOne();
        UpdateInteractableButton();
    }
    #endregion

    void ClickLastDay_PlusOne()
    {
        int i = 0;
        for (;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        if (i == 7) i = (int)_nowSelectedDay;
        ClickDay(i);
        UpdateColorAndSelected();
    }

    IEnumerator StartSchedule()
    {
        for(int i = 0;i<7;i++)
        {
            if (_SevenDayScheduleDatas[i] == null) yield break;
        }

        int beforeSubsAmount = Managers.Data._myPlayerData.nowSubCount;
        int beforeHeart = Managers.Data._myPlayerData.NowHeart;
        int beforeStar = Managers.Data._myPlayerData.NowStar;
        for (int i =0; i<7; i++)
        {
            CarryOutOneDayWork(_SevenDayScheduleDatas[i]);
            Debug.Log($"{i+1}일차 스케쥴 종료");
            UI_MainBackUI.instance.UpdateUItexts();
            yield return new WaitForSeconds(0.1f);
        }
        int aftersubsAmount = Managers.Data._myPlayerData.nowSubCount;
        int afterHeart = Managers.Data._myPlayerData.NowHeart;
        int afterStar = Managers.Data._myPlayerData.NowStar;
        Debug.Log($"1주일 총 구독자 변화량 :     {aftersubsAmount - beforeSubsAmount}");
        Debug.Log($"1주일 하트 구독자 변화량 :   {afterHeart - beforeHeart}");
        Debug.Log($"1주일 별 구독자 변화량 :     {afterStar - beforeStar}");

        UI_MainBackUI.instance.UpdateUItexts();

        if (Managers.Data._myPlayerData.NowWeek % 5 != 0) Managers.UI_Manager.ShowPopupUI<UI_RandomEvent>();
        else Managers.UI_Manager.ShowPopupUI<UI_Merchant>();

    }

    void CarryOutOneDayWork(OneDayScheduleData oneDay)
    {
        float nowWeekmag = Managers.Data.GetNowWeekBonusMag();

        int newSubs = CalculateSubAfterDay(Managers.Data._myPlayerData.nowSubCount,
            oneDay.FisSubsUpValue, oneDay.PerSubsUpValue, nowWeekmag);

        Managers.Data._myPlayerData.nowGoldAmount += Mathf.CeilToInt(Managers.Data._myPlayerData.nowSubCount * oneDay.InComeMag);
        Debug.Log($"골드 증가량 : {Mathf.CeilToInt(Managers.Data._myPlayerData.nowSubCount * oneDay.InComeMag)}");

        if(oneDay.scheduleType == ScheduleType.BroadCast)
        {
            Managers.Data._myPlayerData.nowSubCount += newSubs;
            Debug.Log($"구독자 증가량 : {newSubs}");
        }

        if(oneDay.broadcastType == BroadCastType.Game || oneDay.broadcastType == BroadCastType.Song || oneDay.broadcastType == BroadCastType.Chat)
        {
            CalculateBonus((StatName)Enum.Parse(typeof(StatName) ,oneDay.broadcastType.ToString()) , newSubs, Mathf.CeilToInt(Managers.Data._myPlayerData.nowSubCount * oneDay.InComeMag));
        }

        Debug.Log($"하트 변화량 : {Mathf.Clamp(Mathf.CeilToInt(oneDay.HealthPointChangeValue) + Managers.Data._myPlayerData.NowHeart, 0, 100)- Managers.Data._myPlayerData.NowHeart}" +
            $"현재 하트 : {Mathf.Clamp(Mathf.CeilToInt(oneDay.HealthPointChangeValue) + Managers.Data._myPlayerData.NowHeart, 0, 100)}");

        Debug.Log($"별 변화량 : {Mathf.Clamp(Mathf.CeilToInt(oneDay.MentalPointChageValue) + Managers.Data._myPlayerData.NowStar, 0, 100)- Managers.Data._myPlayerData.NowStar}" +
            $"현제 별 : {Mathf.Clamp(Mathf.CeilToInt(oneDay.MentalPointChageValue) + Managers.Data._myPlayerData.NowStar, 0, 100)}");

        Managers.Data._myPlayerData.NowHeart = Mathf.Clamp(Mathf.CeilToInt(oneDay.HealthPointChangeValue) + Managers.Data._myPlayerData.NowHeart, 0, 100);
        Managers.Data._myPlayerData.NowStar = Mathf.Clamp(Mathf.CeilToInt(oneDay.MentalPointChageValue)  + Managers.Data._myPlayerData.NowStar, 0, 100);
        

        Managers.Data._myPlayerData.SixStat[0] += oneDay.Six_Stats[0];
        Managers.Data._myPlayerData.SixStat[1] += oneDay.Six_Stats[1];
        Managers.Data._myPlayerData.SixStat[2] += oneDay.Six_Stats[2];
        Managers.Data._myPlayerData.SixStat[3] += oneDay.Six_Stats[3];
        Managers.Data._myPlayerData.SixStat[4] += oneDay.Six_Stats[4];
        Managers.Data._myPlayerData.SixStat[5] += oneDay.Six_Stats[5];
    }

    int CalculateSubAfterDay(int now, float fix, float per, float bonus)
    {
        float temp = (now + fix) * ((float)(100 + per) / 100f) * bonus;
        int result = Mathf.CeilToInt(temp);
        return result - now;
    }

    void CalculateBonus(StatName statname, int DaySub, int DayIncome)
    {
        Bonus tempBonus = Managers.Data.GetProperty(statname);

        Managers.Data._myPlayerData.nowGoldAmount += Mathf.CeilToInt(DayIncome * (tempBonus.IncomeBonus)/100f);

        Managers.Data._myPlayerData.nowSubCount += Mathf.CeilToInt(DaySub * ( tempBonus.IncomeBonus) / 100f);
        Debug.Log($"골드 보너스 증가량 : {Mathf.CeilToInt(DayIncome * (tempBonus.IncomeBonus) / 100f)}  구독자 보너스 증가량 : {Mathf.CeilToInt(DaySub * ( tempBonus.IncomeBonus) / 100f)}");
    }

    private void OnDisable()
    {
        UI_MainBackUI.instance.ShowCreateScheduleBTN();
        for(int i =0;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i]!= null)
            {
                Managers.Data._myPlayerData.nowGoldAmount += _SevenDayScheduleDatas[i].MoneyCost;
            }
            UI_MainBackUI.instance.UpdateUItexts();
        }
    }

    void BackBTN()
    {
        if(SubContentSelectPhase)
        {
            SubContentSelectPhase = false;
            GetGameObject((int)GameObjects.Contents3).SetActive(true);
            GetGameObject((int)GameObjects.SubContents).SetActive(false);
        }
        else
        {
            Managers.UI_Manager.ClosePopupUI();
        }
    }
    #endregion
}