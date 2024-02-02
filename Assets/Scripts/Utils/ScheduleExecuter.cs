using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// 게임 매니저 역할 겸임
/// </summary>
public class ScheduleExecuter : MonoSingleton<ScheduleExecuter>
{
    public bool isDev;
    const float TimeToStamp = 2.3f;
    const float TimeStampToNext = 0.7f;
    public WeekReceiptData BeforeScheduleData = new WeekReceiptData();
    public int BeforeGold = 0;

    //영수증 전달용 변수
    //0대성공 1성공 2실패
    [HideInInspector] public int[] SuccessTimeContainer = new int[3];

    public Action<int> SetAniSpeedAction;

    private void Start()
    {
        //if (!Managers.Data.PersistentUser.WatchedTutorial)
        //{
        //    Managers.UI_Manager.ShowPopupUI<UI_Tutorial>();
        //}
    }
    void SetAniSpeed(int speed)
    {
        SetAniSpeedAction?.Invoke(speed);
    }

    public IEnumerator StartSchedule()
    {
        //상태 초기화
        isSick = false; SickDayOne = false;
        BeforeScheduleData.FillDatas();
        for (int i = 0; i < 3; i++)
        {
            SuccessTimeContainer[i] = 0;
        }
     

        //스케쥴 실행
        for (int i = 0; i < 7; i++)
        {
            bool isFastMode = UI_MainBackUI.instance.IsFastMode;
            yield return StartCoroutine(ExecuteOneDayWork(Managers.Data._SevenDayScheduleDatas[i], i, isFastMode));
            
            UI_MainBackUI.instance.UpdateUItexts();
            float waitTime = isFastMode ? TimeStampToNext / 2 : TimeStampToNext;
            yield return new WaitForSeconds(waitTime);
            ChattingManager.Inst.gameObject.SetActive(false);
        }

        //스케쥴 데이터 초기화
        for (int i = 0; i < 7; i++)
        {
            Managers.Data._SevenDayScheduleDatas[i] = null;
            Managers.Data._SeveDayScrollVarValue[i] = 0;
        }

        UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.transparent);
        UI_MainBackUI.instance.StartScreenAnimation("Exit", "");
        UI_MainBackUI.instance.UpdateUItexts();
        ChattingManager.Inst.gameObject.SetActive(false);

        EndSchedule();
    }

    void EndSchedule()
    {
        var NowWeek = Managers.Data.PlayerData.NowWeek;

        switch (NowWeek)
        {
            case 1:
                UI_DefaultPopup.RandEventOccur();
                break;
            case 2:
                Managers.instance.ShowReceipt();
                break;
            case 3:
                Managers.instance.ShowReceipt();
                break;
            case 4:
                Managers.instance.ShowMainStory();
                break;
            case 5:
                UI_DefaultPopup.MerchantAppear();
                break;
            case 6:
                Managers.instance.ShowReceipt();
                break;
            case 7:
                UI_DefaultPopup.RandEventOccur();
                break;
            case 8:
                Managers.instance.ShowMainStory();
                break;
            case 9:
                Managers.instance.ShowReceipt();
                break;
            case 10:
                UI_DefaultPopup.MerchantAppear();
                break;
            case 11:
                Managers.instance.ShowReceipt();
                break;
            case 12:
                Managers.instance.ShowMainStory();
                break;
            case 13:
                UI_DefaultPopup.RandEventOccur();
                break;
            case 14:
                Managers.instance.ShowReceipt();
                break;
            case 15:
                UI_DefaultPopup.MerchantAppear();
                break;
            case 16:
                Managers.instance.ShowMainStory();
                break;
            case 17:
                Managers.instance.ShowReceipt();
                break;
            case 18:
                UI_DefaultPopup.RandEventOccur();
                break;
            case 19:
                Managers.instance.ShowReceipt();
                break;
            case 20:
                Managers.instance.ShowReceipt();
                break;
        }


    }

    public IEnumerator ExecuteOneDayWork(OneDayScheduleData oneDay, int DayIndex, bool isFastMode)
    {
        //초기화
        bool todaySick = false;
        BigSuccess = false;
        UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.transparent);

        //FastMode라면 2배속, 아니면 1배속
        SetAniSpeed(isFastMode ? 2 : 1);

        //휴식 하는게 아니라면 아플 수 있음
        if (oneDay.scheduleType != ContentType.Rest && !isSick)
        {
            Check_illnessProbability();
        }

        //아프면 그냥 누워있을 거임
        if (isSick)
        {
            todaySick = true;
            ExecuteSickDay();
            SuccessTimeContainer[2]++;
        }
        //아프지 않다면 모든 일정에 대해 대성공이 뜰 수 있음
        else
        {
            UI_MainBackUI.instance.StartScreenAnimation(oneDay.PathName, oneDay.RubiaAni);
            oneDay.CheckAndAddIfNotWatched();
            if(oneDay.scheduleType == ContentType.BroadCast)
            {
                ChattingManager.Inst.gameObject.SetActive(true);
                ChattingManager.Inst.StartGenerateChattingByType(oneDay.broadcastType);
            }
            else
            {
                ChattingManager.Inst.gameObject.SetActive(false);
            }

            //대성공 체크
            float bonusMultiplier = 1.0f;
            if (CheckSuccessProbability())
            {
                BigSuccess = true;
                bonusMultiplier = 1.5f;// 50% 상승을 위한 상수값
                SuccessTimeContainer[0]++;
            }
            else
            {
                SuccessTimeContainer[1]++;
            }

            //방송을 진행했다면 돈 구독자 증가
            if (oneDay.scheduleType == ContentType.BroadCast)
            {
                IncreaseSubsAndMoney(oneDay, bonusMultiplier);
            }

            //컨디션 변화
            float HeartVariance; float StarVariance;
            if (oneDay.scheduleType == ContentType.Rest)
            {
                HeartVariance = oneDay.HeartVariance * bonusMultiplier;
                StarVariance = oneDay.StarVariance * bonusMultiplier;
            }
            else
            {
                HeartVariance = oneDay.HeartVariance * GetSubStatProperty(StatName.Strength);
                StarVariance = oneDay.StarVariance * GetSubStatProperty(StatName.Mental);
            }
            Managers.Data.PlayerData.NowHeart = Mathf.Clamp(Mathf.CeilToInt(HeartVariance) + Managers.Data.PlayerData.NowHeart, 0, 100);
            Managers.Data.PlayerData.NowStar = Mathf.Clamp(Mathf.CeilToInt(StarVariance) + Managers.Data.PlayerData.NowStar, 0, 100);


            //스텟 변화
            float[] tempstat = new float[6];
            for (int i = 0; i < 6; i++)
            {
                tempstat[i] = oneDay.Six_Stats[i] * bonusMultiplier;
            }
            Managers.Data.PlayerData.ChangeStat(tempstat);
        }

        float waitTime = isFastMode ? TimeToStamp / 2 : TimeToStamp;
        if (isDev) waitTime = 0;
        yield return new WaitForSeconds(waitTime);

        //UI하단 씰 붙이기
        if (todaySick || isSick)
        {
            UI_MainBackUI.instance.BottomSeal(DayIndex, 2);
            UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.Fail);
            Managers.Sound.Play(Define.Sound.Fail);
        }
        else if (BigSuccess)
        {
            UI_MainBackUI.instance.BottomSeal(DayIndex, 0);
            UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.BicSuccess);
            Managers.Sound.Play(Define.Sound.BigSuccess);
        }
        else
        {
            UI_MainBackUI.instance.BottomSeal(DayIndex, 1);
            UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.Success);
            Managers.Sound.Play(Define.Sound.Success);
        }
    }


    #region Sick__BigSuccess
    bool isSick = false; bool SickDayOne = false;
    bool caughtCold = false; bool caughtDepression = false;


    void Check_illnessProbability()
    {
        if (Managers.Data.PlayerData.NowHeart < 50 || Managers.Data.PlayerData.NowStar < 50)
        {
            if (Managers.Data.PlayerData.NowHeart < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtCold = true;
                    return;
                }
            }
            else if (Managers.Data.PlayerData.NowStar < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtDepression = true;
                    return;
                }
            }
            else if (Managers.Data.PlayerData.NowHeart < 50)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtCold = true;
                    return;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtDepression = true;
                    return;
                }
            }
        }
    }

    void ExecuteSickDay()
    {
        if (SickDayOne)
        {
            SickDayOne = false;
        }
        else
        {
            isSick = false;
        }
        int RestHeartStarValue = 10;
        Managers.Data.PlayerData.NowHeart += RestHeartStarValue;
        Managers.Data.PlayerData.NowStar += RestHeartStarValue;
    }

    bool BigSuccess = false;

    bool CheckSuccessProbability()
    {
        int LuckGrade = ((int)Managers.Data.PlayerData.SixStat[5]) / 10;
        if (UnityEngine.Random.Range(0, 100) < (LuckGrade * 5))
        {
            return true;
        }

        return false;
    }

    #endregion


    #region DoOneDaySchedule


    void IncreaseSubsAndMoney(OneDayScheduleData oneDay, float bonusMultiplier)
    {
        int beforeSub = Managers.Data.PlayerData.nowSubCount;

        int OneDayNewSubs = CalculateSubAfterDay(beforeSub, oneDay.FisSubsUpValue, oneDay.PerSubsUpValue, bonusMultiplier);

        int OneDayIncome = Mathf.CeilToInt(Mathf.Log10(beforeSub)*300 * oneDay.InComeMag * bonusMultiplier);

        Managers.Data.PlayerData.nowSubCount += OneDayNewSubs;
        Managers.Data.PlayerData.nowGoldAmount += OneDayIncome;

        CalculateBonus(oneDay.broadcastType, OneDayNewSubs, OneDayIncome);
    }


    public int CalculateSubAfterDay(int now, float fix, float per, float bonus)
    {
        float temp = (now + fix) * ((float)(100 + per) / 100f);
        int result = Mathf.CeilToInt(temp);
        result -= now;

        float result2 = result * bonus;

        return Mathf.CeilToInt(result2);
    }

    public void CalculateBonus(BroadCastType broadCastType, int DaySub, int DayIncome)
    {
        CalculateBonus(GetStatNameByBroadCastType(broadCastType), DaySub, DayIncome);
    }

    //실행부
    void CalculateBonus(StatName statname, int DaySub, int DayIncome)
    {
        Bonus tempBonus = Managers.Data.GetMainProperty(statname);

        Managers.Data.PlayerData.nowGoldAmount += Mathf.CeilToInt(DayIncome * (tempBonus.IncomeBonus) / 100f);
        Managers.Data.PlayerData.nowSubCount += Mathf.CeilToInt(DaySub * (tempBonus.IncomeBonus) / 100f);
    }

    public float GetSubStatProperty(StatName statName)
    {
        int temp = 0;
        if (statName == StatName.Strength)
            temp = (int)Math.Floor(Managers.Data.PlayerData.SixStat[3]);

        else if (statName == StatName.Mental)
            temp = (int)Math.Floor(Managers.Data.PlayerData.SixStat[4]);

        float result = (float)(temp / 10);
        result *= Managers.instance.Str_Men_ValuePerLevel;
        result = 1 - result;
        return result;
    }


    #endregion

    #region Actions

    public Action GameStart;
    public void StartGame()
    {
        GameStart?.Invoke();
        
    }

    public Action WeekOverAction;

    public void FinishWeek()
    {
        WeekOverAction?.Invoke();
        Managers.Data.PlayerData.NowWeek++;
        Managers.Data.SaveData();
    }
    #endregion
}
