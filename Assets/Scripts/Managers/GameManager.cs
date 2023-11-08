using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager
{
    string[] daysOfWeek = { "월요일", "화요일", "수요일", "목요일", "금요일", "토요일", "일요일" };
    public IEnumerator StartSchedule()
    {
        int beforeSubsAmount = Managers.Data._myPlayerData.nowSubCount;
        int beforeHeart = Managers.Data._myPlayerData.NowHeart;
        int beforeStar = Managers.Data._myPlayerData.NowStar;

        // 월요일은 안아파
        isSick = false; SickDayOne = false;

        for (int i = 0; i < 7; i++)
        {
            CarryOutOneDayWork(Managers.Data._SevenDayScheduleDatas[i], i);
            Debug.Log("-----------------------------------------");
            UI_MainBackUI.instance.UpdateUItexts();
            yield return new WaitForSeconds(1f);
        }

        //매 월 마지막 주차는 월세 나감
        if (Managers.Data._myPlayerData.NowWeek % 4 == 0)
        {
            Managers.Data._myPlayerData.nowGoldAmount -= Managers.Data.GetNowMonthExpense();
            Debug.Log($"월세 {Managers.Data.GetNowMonthExpense() } 원 차감");
        }

        int aftersubsAmount = Managers.Data._myPlayerData.nowSubCount;
        int afterHeart = Managers.Data._myPlayerData.NowHeart;
        int afterStar = Managers.Data._myPlayerData.NowStar;
        Debug.Log($"1주일 총 구독자 변화량 :     {aftersubsAmount - beforeSubsAmount}");
        Debug.Log($"1주일 하트 구독자 변화량 :   {afterHeart - beforeHeart}");
        Debug.Log($"1주일 별 구독자 변화량 :     {afterStar - beforeStar}");

        for(int i =0;i<7;i++)
        {
            Managers.Data._SevenDayScheduleDatas[i] = null;
            Managers.Data._SeveDayScrollVarValue[i] = 0;
        }

        UI_MainBackUI.instance.UpdateUItexts();

        if (Managers.Data._myPlayerData.NowWeek % 5 != 0) Managers.UI_Manager.ShowPopupUI<UI_RandomEvent>();
        else Managers.UI_Manager.ShowPopupUI<UI_Merchant>();
    }

    void CarryOutOneDayWork(OneDayScheduleData oneDay, int Day)
    {
        //휴식 하는게 아니라면 아플 수 있음
        if(oneDay.scheduleType != ScheduleType.Rest)
        {
            CheckPossibilityOfCatching_Aya();
        }

        //아프면 그냥 누워있을 거임
        if(isSick)
        {
            Debug.Log($"{daysOfWeek[Day]} 아픔");
            CarryOutSickDay();
        }
        //안아프면 모든 일정에 대해 대성공이 뜰 수 있음
        else
        {
            Debug.Log($"{daysOfWeek[Day]} 스케줄 {Managers.Data._SevenDayScheduleDatas[Day].KorName} 시작");
            float bonusMultiplier = 1.0f;
            if (CheckPossibilityOfBigSuccess())
            {
                BigSuccess = true;
                bonusMultiplier = 1.5f;// 50% 상승을 위한 상수값
                Debug.Log("대성공");
            }

            float nowWeekmag = Managers.Data.GetNowWeekBonusMag();

            int OneDayNewSubs = CalculateSubAfterDay(Managers.Data._myPlayerData.nowSubCount,
                oneDay.FisSubsUpValue, oneDay.PerSubsUpValue, nowWeekmag * bonusMultiplier);

            int OneDayIncome = Mathf.CeilToInt(Managers.Data._myPlayerData.nowSubCount * oneDay.InComeMag * bonusMultiplier);

            if (oneDay.scheduleType == ScheduleType.BroadCast)
            {
                Managers.Data._myPlayerData.nowSubCount += OneDayNewSubs;
                Managers.Data._myPlayerData.nowGoldAmount += OneDayIncome;
                Debug.Log($"구독+ : {OneDayNewSubs}" + $" / 골드 + : {OneDayIncome}");
            }

            CalculateBonusWithType(oneDay.broadcastType, OneDayNewSubs, OneDayIncome);

            float HeartVariance; float StarVariance;
            if (oneDay.scheduleType == ScheduleType.Rest)
            {
                HeartVariance = oneDay.HeartVariance * bonusMultiplier;
                StarVariance = oneDay.StarVariance * bonusMultiplier;
            }
            else
            {
                HeartVariance = oneDay.HeartVariance * StrengthBonus();
                StarVariance = oneDay.StarVariance * MentalBonus();
            }

            Debug.Log($"건강 변화량 : ({Mathf.Clamp((HeartVariance) + Managers.Data._myPlayerData.NowHeart, 0, 100) - Managers.Data._myPlayerData.NowHeart}," +
            $" {Mathf.Clamp((StarVariance) + Managers.Data._myPlayerData.NowStar, 0, 100) - Managers.Data._myPlayerData.NowStar}), " +
            $"결과 ( {Mathf.Clamp((HeartVariance) + Managers.Data._myPlayerData.NowHeart, 0, 100)}, " +
            $"{Mathf.Clamp((StarVariance) + Managers.Data._myPlayerData.NowStar, 0, 100)})");

            Managers.Data._myPlayerData.NowHeart = Mathf.Clamp(Mathf.CeilToInt(HeartVariance) + Managers.Data._myPlayerData.NowHeart, 0, 100);
            Managers.Data._myPlayerData.NowStar = Mathf.Clamp(Mathf.CeilToInt(StarVariance) + Managers.Data._myPlayerData.NowStar, 0, 100);

            float[] tempstat = new float[6];
            for(int i = 0;i<6;i++)
            {
                tempstat[i] = oneDay.Six_Stats[i] * bonusMultiplier;
            }

            Managers.Data._myPlayerData.UpStat(tempstat);
        }

        if (isSick)         UI_MainBackUI.instance.StampSeal(Day, 2);
        else if(BigSuccess) UI_MainBackUI.instance.StampSeal(Day, 0);
        else                UI_MainBackUI.instance.StampSeal(Day, 1);
    }


    #region Calculate
    int CalculateSubAfterDay(int now, float fix, float per, float bonus)
    {
        float temp = (now + fix) * ((float)(100 + per) / 100f) * bonus;
        int result = Mathf.CeilToInt(temp);
        return result - now;
    }

    void CalculateBonusWithType(BroadCastType broadCastType, int DaySub, int DayIncome)
    {
        StatName temp;
        if (broadCastType == BroadCastType.Healing || broadCastType == BroadCastType.LOL
            || broadCastType == BroadCastType.Horror || broadCastType == BroadCastType.Challenge)
            temp = StatName.Game;
        else if (broadCastType == BroadCastType.Sing || broadCastType == BroadCastType.PlayInst || broadCastType == BroadCastType.Compose)
            temp = StatName.Song;
        else
            temp = StatName.Draw;

        CalculateBonus(temp, DaySub, DayIncome);
    }


    void CalculateBonus(StatName statname, int DaySub, int DayIncome)
    {
        Bonus tempBonus = Managers.Data.GetProperty(statname);

        Managers.Data._myPlayerData.nowGoldAmount += Mathf.CeilToInt(DayIncome * (tempBonus.IncomeBonus) / 100f);

        Managers.Data._myPlayerData.nowSubCount += Mathf.CeilToInt(DaySub * (tempBonus.IncomeBonus) / 100f);
        Debug.Log($"특성 골드 보너스 : {Mathf.CeilToInt(DayIncome * (tempBonus.IncomeBonus) / 100f)} 특성 구독자 보너스 증가량 : {Mathf.CeilToInt(DaySub * (tempBonus.IncomeBonus) / 100f)}");
    }

    float StrengthBonus()
    {
        int temp = (int)Math.Floor(Managers.Data._myPlayerData.SixStat[3]);
        float result = (float)(temp / 10);
        result *= 0.05f;
        result = 1 - result;
        return result;
    }

    float MentalBonus()
    {
        int temp = (int)Math.Floor(Managers.Data._myPlayerData.SixStat[4]);
        float result = (float)(temp / 10);
        result *= 0.05f;
        result = 1 - result;
        return result;
    }

    #endregion

    #region SuccessAndFail
    bool isSick = false;        bool SickDayOne = false;
    bool caughtCold = false;    bool caughtDepression = false;

    /// <summary>
    /// 컨디션이 좋지 않다면 아야 함
    /// </summary>
    void CheckPossibilityOfCatching_Aya()
    {
        if(Managers.Data._myPlayerData.NowHeart < 50 || Managers.Data._myPlayerData.NowStar < 50)
        {
            if(Managers.Data._myPlayerData.NowHeart < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtCold = true;
                    return;
                }
            }
            else if(Managers.Data._myPlayerData.NowStar < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    SickDayOne = true;
                    caughtDepression = true;
                    return;
                }
            }
            else if(Managers.Data._myPlayerData.NowHeart < 50)
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
    
    
    /// <summary>
    /// 아픈 날 진행
    /// </summary>
    void CarryOutSickDay()
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
        Managers.Data._myPlayerData.NowHeart += RestHeartStarValue;
        Managers.Data._myPlayerData.NowStar  += RestHeartStarValue;
        Debug.Log($"건강 변화량 : ({Mathf.Clamp((RestHeartStarValue) + Managers.Data._myPlayerData.NowHeart, 0, 100) - Managers.Data._myPlayerData.NowHeart}," +
            $" {Mathf.Clamp((RestHeartStarValue) + Managers.Data._myPlayerData.NowStar, 0, 100) - Managers.Data._myPlayerData.NowStar}), " +
            $"결과 ( {Mathf.Clamp((RestHeartStarValue) + Managers.Data._myPlayerData.NowHeart, 0, 100)}, " +
            $"{Mathf.Clamp((RestHeartStarValue) + Managers.Data._myPlayerData.NowStar, 0, 100)})");

        if (caughtCold)
        {
            Debug.Log("엘라 감기 걸림");
        }
        else
        {
            Debug.Log("엘라 우울함");
        }
    }

    bool BigSuccess = false;

    bool CheckPossibilityOfBigSuccess()
    {
        int LuckGrade = ((int)Managers.Data._myPlayerData.SixStat[5]) / 10;
        if (UnityEngine.Random.Range(0, 100) < (LuckGrade*5))
        {
            return true;
        }

        return false;
    }

    #endregion
}
