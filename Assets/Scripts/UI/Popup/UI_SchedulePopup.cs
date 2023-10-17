using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
using static Define;

/// <summary>
/// 스케쥴 관리와 방송 정보에 대한 정보가 담겨있는 스크립트
/// </summary>
public class UI_SchedulePopup : UI_Popup
    
{
    public static UI_SchedulePopup instance; 

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

        LeftPageBTN,
        RightPageBTN,

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
        SubContents4,
        Sub0, Sub1, Sub2, Sub3, 
    }

    enum Images
    {
        ItemIcon,
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

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GetGameObject((int)GameObjects.SubContents4).SetActive(false);

        for (int i = 0; i<7; i++)
            //7일들
        {
            int inttemp = i;
            Button temp = GetButton(i);
            temp.onClick.AddListener( () => ClickDay(inttemp));
        }

        GetButton((int)Buttons.BroadCastBTN).onClick.AddListener(BroadCastBTN);
        GetButton((int)Buttons.RestBTN).onClick.AddListener(RestBTN);
        GetButton((int)Buttons.GoOutBTN).onClick.AddListener(GoOutBTN);

        GetButton((int)Buttons.LeftPageBTN).onClick.AddListener(GoLeftPage);
        GetButton((int)Buttons.RightPageBTN).onClick.AddListener(GoRightPage);

        ClickDay(0); //기본 월요일 선택
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
    

    void ClickDay(int i)
    {
        _nowSelectedDay = (SevenDays)i;
        RenewalDayBTNColor();
    }

    public Color Orange;

    void RenewalDayBTNColor()
        //색상 지정용 함수
    {
        for(int i = 0; i<7;i++)
        {
            if(i == (int)_nowSelectedDay)
            {
                GetButton(i).GetComponent<Image>().color = Color.red;
            }
            else if(_SevenDayScheduleDatas[i] == null)
            {
                GetButton(i).GetComponent<Image>().color = Color.white;
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.BroadCast)
            {
                GetButton(i).GetComponent<Image>().color = Color.blue;
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.Rest)
            {
                GetButton(i).GetComponent<Image>().color = Color.green;
            }
            else
            {
                GetButton(i).GetComponent<Image>().color = Orange;
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

        public void PrintData()
        {
            Debug.Log("Korean Name: " + KorName);
            Debug.Log("Info Text: " + infotext);
            Debug.Log("Schedule Type: " + scheduleType);
            Debug.Log("Broadcast Type: " + broadcastType);
            Debug.Log("Rest Type: " + restType);
            Debug.Log("Go Out Type: " + goOutType);
            Debug.Log("Fis Subs Up Value: " + FisSubsUpValue);
            Debug.Log("Per Subs Up Value: " + PerSubsUpValue);
            Debug.Log("Health Point Change Value: " + HealthPointChangeValue);
            Debug.Log("Mental Point Change Value: " + MentalPointChageValue);
            Debug.Log("Income Magnitude: " + InComeMag);
            Debug.Log("Money Cost: " + MoneyCost);
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

    

    void BroadCastBTN()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents4).SetActive(true);
        ChooseScheduleTypeAndFillList(ScheduleType.BroadCast);

    }
    void RestBTN()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents4).SetActive(true);
        ChooseScheduleTypeAndFillList(ScheduleType.Rest);
    }

    void GoOutBTN()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents4).SetActive(true);
        ChooseScheduleTypeAndFillList(ScheduleType.GoOut);
    }


    List<OneDayScheduleData> nowSelectScheduleTypeList = new List<OneDayScheduleData>();
    void ChooseScheduleTypeAndFillList(ScheduleType type)
    {
        nowSelectScheduleTypeList.Clear();
        switch (type)
        {
            case ScheduleType.BroadCast:
                for (int i = 0; i < (int)BroadCastType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((BroadCastType)i));
                }
                
                break;

            case ScheduleType.Rest:
                for (int i = 0; i < (int)RestType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((RestType)i));
                }
                break;

            case ScheduleType.GoOut:
                for (int i = 0; i < (int)GoOutType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((GoOutType)i));
                }
                break;
        }
        _nowpage = 0;
        Renewal4SubContentsBTN();
    }

    int _nowpage = 0; int _MaxPage;


    void Renewal4SubContentsBTN()
    {
        _MaxPage = nowSelectScheduleTypeList.Count / 4;

        if(_nowpage == 0)
        {
            GetButton((int)Buttons.LeftPageBTN).interactable = false;
        }
        else
        {
            GetButton((int)Buttons.LeftPageBTN).interactable = true;
        }

        if(_nowpage == _MaxPage)
        {
            GetButton((int)Buttons.RightPageBTN).interactable = false;
        }
        else
        {
            GetButton((int)Buttons.RightPageBTN).interactable = true;
        }

        for (int i = 0;i<4;i++)
        {
            if (isIndexExist(i))
            {
                if(_SevenDayScheduleDatas[(int)_nowSelectedDay] == null)
                {
                    GetGameObject(3 + i).
                    GetOrAddComponent<UI_SubContent>().SetInfo(nowSelectScheduleTypeList[nowIndex(i)], 0);
                }
                else
                {
                    GetGameObject(3 + i).
                    GetOrAddComponent<UI_SubContent>().SetInfo(nowSelectScheduleTypeList[nowIndex(i)], _SevenDayScheduleDatas[(int)_nowSelectedDay].MoneyCost);
                }
                
            }
            else
            {
                GetGameObject(3 + i).
                    GetOrAddComponent<UI_SubContent>().SetInfo(null, 0);
            }
                
        }

    }

    void GoLeftPage()
    {
        _nowpage--;
        Renewal4SubContentsBTN();
    }

    void GoRightPage()
    {
        _nowpage++;
        Renewal4SubContentsBTN();
    }

    bool isIndexExist(int i)
    {
        int temp = i + (4 * _nowpage);
        if (nowSelectScheduleTypeList.Count-1 >= temp)
            return true;
        else
            return false;
    }

    int nowIndex(int i)
    {
        return i + (4 * _nowpage);
    }

    public void SetDaySchedule(OneDayScheduleData data)
    {
        _SevenDayScheduleDatas[(int)_nowSelectedDay] = data;
        ChangeNowSelectDayToNearestAndCheckFull();
    }
    #endregion

    void ChangeNowSelectDayToNearestAndCheckFull()
    {
        for(int i = 0;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
            if(i==6)
            {
                StartCoroutine(StartSchedule());
            }
        }

        RenewalDayBTNColor();
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents4).SetActive(false);
    }

    IEnumerator StartSchedule()
    {
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
        Debug.Log($"1주일 총 구독자 변화량 : {aftersubsAmount - beforeSubsAmount}");
        Debug.Log($"1주일 하트 구독자 변화량 : {afterHeart - beforeHeart}");
        Debug.Log($"1주일 별 구독자 변화량 : {afterStar - beforeStar}");

        UI_MainBackUI.instance.UpdateUItexts();
        UI_MainBackUI.instance.ShowOrCloseCreateSchedulePopup();


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
        for(int i =0;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i]!= null)
            {
                Managers.Data._myPlayerData.nowGoldAmount += _SevenDayScheduleDatas[i].MoneyCost;
            }
            UI_MainBackUI.instance.UpdateUItexts();
        }
    }

    #endregion
}