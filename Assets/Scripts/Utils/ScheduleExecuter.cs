using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ScheduleExecuter : MonoSingleton<ScheduleExecuter>
{
    public bool isDev;
    public float TimeToStamp;
    public float TimeStampToNext;
    public WeekReceiptData BeforeScheduleData = new WeekReceiptData();
    public int BeforeGold = 0;

    //0대성공 1성공 2실패
    public int[] SuccessTime = new int[3];

    private void Awake()
    {
        base.Awake();
    }

    public IEnumerator StartSchedule()
    {
        //상태 초기화
        isSick = false; SickDayOne = false;
        BeforeScheduleData.FillDatas();
        for (int i = 0; i < 3; i++)
        {
            SuccessTime[i] = 0;
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

        UI_MainBackUI.instance.SetStamp(-1);
        UI_MainBackUI.instance.StartScreenAnimation("Exit", "");
        UI_MainBackUI.instance.UpdateUItexts();
        ChattingManager.Inst.gameObject.SetActive(false);

        if (Managers.Data.PlayerData.NowWeek == 20)
            Managers.UI_Manager.ShowPopupUI<UI_Ending>();
        else if (Managers.Data.PlayerData.MerchantAppearanceWeek())
            Managers.UI_Manager.ShowPopupUI<UI_Merchant>();
        else if (Managers.Data.PlayerData.MainStoryApperanceWeek())
            Managers.instance.ShowMainStory();
        else
            Managers.UI_Manager.ShowPopupUI<UI_RandomEvent>();
    }

    public IEnumerator ExecuteOneDayWork(OneDayScheduleData oneDay, int DayIndex, bool isFastMode)
    {
        //초기화
        bool todaySick = false;
        BigSuccess = false;
        UI_MainBackUI.instance.SetStamp(-1);
        
        if(isFastMode)
        {
            UI_MainBackUI.instance.ScreenAnimator.speed = UI_MainBackUI.instance.ScreenAniSpeed * 2;
            UI_MainBackUI.instance.RubiaAnimator.speed = UI_MainBackUI.instance.ScreenAniSpeed * 2;
            ChattingManager.Inst.MaxChatDelayTime = 0.05f;
            ChattingManager.Inst.TimeForChatGetBigger = 0.04f;
            ChattingManager.Inst.ChatBubbleRiseDuration = 0.15f;
        }
        else
        {
            UI_MainBackUI.instance.ScreenAnimator.speed = UI_MainBackUI.instance.ScreenAniSpeed;
            UI_MainBackUI.instance.RubiaAnimator.speed = UI_MainBackUI.instance.ScreenAniSpeed;
            ChattingManager.Inst.MaxChatDelayTime = 0.1f;
            ChattingManager.Inst.TimeForChatGetBigger = 0.08f;
            ChattingManager.Inst.ChatBubbleRiseDuration = 0.3f;
        }

        //휴식 하는게 아니라면 아플 수 있음
        if (oneDay.scheduleType != TaskType.Rest && !isSick)
        {
            Check_illnessProbability();
        }

        //아프면 그냥 누워있을 거임
        if (isSick)
        {
            todaySick = true;
            ExecuteSickDay();
            SuccessTime[2]++;
        }
        //아프지 않다면 모든 일정에 대해 대성공이 뜰 수 있음
        else
        {
            UI_MainBackUI.instance.StartScreenAnimation(oneDay.PathName, oneDay.RubiaAni);
            oneDay.CheckAndAddIfNotWatched();
            if(oneDay.scheduleType == TaskType.BroadCast)
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
                SuccessTime[0]++;
            }
            else
            {
                SuccessTime[1]++;
            }

            //방송을 진행했다면 돈 구독자 증가
            if (oneDay.scheduleType == TaskType.BroadCast)
            {
                IncreaseSubsAndMoney(oneDay, bonusMultiplier);
            }

            //컨디션 변화
            float HeartVariance; float StarVariance;
            if (oneDay.scheduleType == TaskType.Rest)
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
            UI_MainBackUI.instance.SetStamp(0);
            Managers.Sound.Play(Define.Sound.Fail);
        }
        else if (BigSuccess)
        {
            UI_MainBackUI.instance.BottomSeal(DayIndex, 0);
            UI_MainBackUI.instance.SetStamp(2);
            Managers.Sound.Play(Define.Sound.BigSuccess);
        }
        else
        {
            UI_MainBackUI.instance.BottomSeal(DayIndex, 1);
            UI_MainBackUI.instance.SetStamp(1);
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
