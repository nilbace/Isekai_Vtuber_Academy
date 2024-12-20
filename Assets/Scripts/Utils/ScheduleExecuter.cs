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

    //영수증 전달용 변수
    //0대성공 1성공 2실패
    [HideInInspector] public int[] SuccessTimeContainer = new int[3];

    public Action<int> SetAniSpeedAction;

    //아픈 첫날 체크용
    //두번쩃날에는 이미 false임
    bool FirstSickDay = false;
    //두번쨋날이 끝나면 false로 바뀜
    bool isSick = false; 
    bool caughtCold = false; 
    bool caughtDepression = false;
    void SetAniSpeed(int speed)
    {
        SetAniSpeedAction?.Invoke(speed);
    }

    public IEnumerator StartSchedule()
    {
        //상태 초기화
        isSick = false; FirstSickDay = false;
        BeforeScheduleData.FillDatas();
        UI_MainBackUI.instance.StopScreenAni();
        for (int i = 0; i < 3; i++)
        {
            SuccessTimeContainer[i] = 0;
        }
     

        //스케쥴 실행
        for (int i = 0; i < 7; i++)
        {
            bool isFastMode = UI_MainBackUI.instance.IsFastMode;
            yield return StartCoroutine(ExecuteOneDayWork(Managers.Data._SevenDayScheduleDatas[i], i, isFastMode));
            
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
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
        UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        ChattingManager.Inst.gameObject.SetActive(false);

        AfterSchedule();
    }



    void AfterSchedule()
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
                //Managers.instance.ShowEndingStory();
                Managers.instance.ShowMainStory();
                break;
        }


    }

    private void FinishTextAni()
    {
        //foreach(var item in FloatingTextPozs)
        //{
        //    item.StopAllCoroutines();
        //    item.text.alpha = 0;
        //}
    }
    public IEnumerator ExecuteOneDayWork(OneDayScheduleData oneDay, int DayIndex, bool isFastMode)
    {
        FinishTextAni();
        //초기화
        bool todaySick = false;
        BigSuccess = false;
        UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.transparent);
        float bigSuccessMultiplier =1f;

        //애니메이션 연출용 변화 스텟 기억용 변수
        List<(StatName stat, float value)> ChangedList = new List<(StatName stat, float value)>();

        //FastMode라면 2배속, 아니면 1배속
        SetAniSpeed(isFastMode ? 2 : 1);

        //튜토리얼중이라면
        if (UI_Tutorial.instance != null)
        {
            //토요일에 실패
            if (DayIndex == 5)
            {
                isSick = true;
                FirstSickDay = true;
                caughtCold = true;
            }
        }
        //튜토리얼이 아닐 때
        else
        {
            //휴식 하는게 아니라면 아플 수 있음
            if (oneDay.ContentType != ContentType.Rest && !isSick)
            {
                Check_illnessProbability();
            }
        }

        //아픈 첫번쨋날, 두번쨋날 실행되는 부분
        if (isSick)
        {
            var beforeStar = Managers.Data.PlayerData.NowStar;
            var beforeHeart = Managers.Data.PlayerData.NowHeart;

            todaySick = true;
            ExecuteSickDay(oneDay);
            //실패 카운트 증가
            SuccessTimeContainer[2]++;

            var heartDiff = Managers.Data.PlayerData.NowHeart - beforeHeart;
            var starDiff =  Managers.Data.PlayerData.NowStar- beforeStar;

            if(heartDiff != 0)
            {
                ChangedList.Add((StatName.Heart, heartDiff));
            }
            if(starDiff != 0)
            {
                ChangedList.Add((StatName.Star, starDiff));
            }
            Managers.Data.PlayerData.ChangeStatAndPlayUIAnimation(ChangedList);

            float waitTime = isFastMode ? TimeToStamp / 2 : TimeToStamp;
            if (isDev) waitTime = 0;
            yield return new WaitForSeconds(waitTime);
        }
        //아프지 않다면 모든 일정에 대해 대성공이 뜰 수 있음
        else
        {
            UI_MainBackUI.instance.StartScreenAnimation(oneDay.PathName, oneDay.RubiaAni);
            oneDay.CheckAndAddIfNotWatched();
            if(oneDay.ContentType == ContentType.BroadCast)
            {
                ChattingManager.Inst.gameObject.SetActive(true);
                ChattingManager.Inst.StartGenerateChattingByType(oneDay.broadcastType);
            }
            else
            {
                ChattingManager.Inst.gameObject.SetActive(false);
            }

            //대성공 체크
            bigSuccessMultiplier = 1.0f;
            
            //튜토리얼이라면
            if(UI_Tutorial.instance != null)
            {
                //수요일에 대성공
                if(DayIndex==2)
                {
                    BigSuccess = true;
                    bigSuccessMultiplier = Managers.instance.BigSuccessCoefficientValue;
                    //대성공 카운트 증가
                    SuccessTimeContainer[0]++;
                }
            }
            //튜토리얼이 아니라면
            //대성공 확률 체크 후 실행
            else
            {
                if (CheckSuccessProbability())
                {
                    BigSuccess = true;
                    bigSuccessMultiplier = Managers.instance.BigSuccessCoefficientValue;
                    //대성공 카운트 증가
                    SuccessTimeContainer[0]++;
                    Managers.Data.PersistentUser.BigSuccessCount++;
                }
                else
                {
                    //성공 카운트 증가
                    SuccessTimeContainer[1]++;
                }
            }

            //방송을 진행했다면 돈 구독자 증가
            if (oneDay.ContentType == ContentType.BroadCast)
            {
                int beforeSub = Managers.Data.PlayerData.nowSubCount;
                int beforeGold = Managers.Data.PlayerData.nowGoldAmount;

                Managers.Data.PersistentUser.BroadcastCount++;
                IncreaseSubsAndMoney(oneDay, bigSuccessMultiplier);

                int changedSub = Managers.Data.PlayerData.nowSubCount - beforeSub;
                int chagnedGold = Managers.Data.PlayerData.nowGoldAmount - beforeGold;

                ChangedList.Add((StatName.Sub, changedSub));
                ChangedList.Add((StatName.Gold, chagnedGold));
            }

            // 컨디션 변화
            float HeartVariance;
            float StarVariance;
            if (oneDay.ContentType == ContentType.Rest)
            {
                HeartVariance = oneDay.HeartVariance * bigSuccessMultiplier;
                StarVariance = oneDay.StarVariance * bigSuccessMultiplier;
            }
            else
            {
                HeartVariance = oneDay.HeartVariance * GetSubStatProperty(StatName.Strength);
                StarVariance = oneDay.StarVariance * GetSubStatProperty(StatName.Mental);
            }

            // 하트(Heart) 값 변경 및 리스트에 추가
            float newHeartValue = Mathf.Clamp(Mathf.CeilToInt(HeartVariance) + Managers.Data.PlayerData.NowHeart, 0, 100);
            if (newHeartValue != Managers.Data.PlayerData.NowHeart)
            {
                ChangedList.Add((StatName.Heart, newHeartValue - Managers.Data.PlayerData.NowHeart));
            }
            Managers.Data.PlayerData.NowHeart = newHeartValue;

            // 별(Star) 값 변경 및 리스트에 추가
            float newStarValue = Mathf.Clamp(Mathf.CeilToInt(StarVariance) + Managers.Data.PlayerData.NowStar, 0, 100);
            if (newStarValue != Managers.Data.PlayerData.NowStar)
            {
                ChangedList.Add((StatName.Star, newStarValue - Managers.Data.PlayerData.NowStar));
            }
            Managers.Data.PlayerData.NowStar = newStarValue;

            // 스탯 변화
            float[] tempstat = new float[6];
            for (int i = 0; i < 6; i++)
            {
                tempstat[i] = oneDay.Six_Stats[i] * bigSuccessMultiplier;
                float newStatValue = Mathf.Clamp(Mathf.CeilToInt(tempstat[i]) + Managers.Data.PlayerData.SixStat[i], 0, 200);

                // 스탯 변화가 있으면 리스트에 추가
                if (newStatValue != Managers.Data.PlayerData.SixStat[i])
                {
                    ChangedList.Add(((StatName)i, newStatValue - Managers.Data.PlayerData.SixStat[i]));
                }
                Managers.Data.PlayerData.SixStat[i] = newStatValue;
            }

            Managers.Data.PlayerData.ChangeStatAndPlayUIAnimation(ChangedList);

            float waitTime = isFastMode ? TimeToStamp / 2 : TimeToStamp;
            if (isDev) waitTime = 0;
            yield return new WaitForSeconds(waitTime);
        }

        


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
        //튜토리얼 상태일때
        if (UI_Tutorial.instance != null)
        {
            yield return new WaitForSeconds(0.2f);
            //수요일이라면
            if (DayIndex == 2)
            {
                Time.timeScale = 0;
                UI_Tutorial.instance.NextDialogue();
            }
            if (DayIndex == 5)
            {
                Time.timeScale = 0;
                UI_Tutorial.instance.NextDialogue();
            }
        }

    }


    #region Sick__BigSuccess


    //감기에 걸리거나 가출함
    void Check_illnessProbability()
    {
        if (Managers.Data.PlayerData.NowHeart < 50 || Managers.Data.PlayerData.NowStar < 50)
        {
            if (Managers.Data.PlayerData.NowHeart < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    FirstSickDay = true;
                    caughtCold = true;
                    return;
                }
            }
            else if (Managers.Data.PlayerData.NowStar < 25)
            {
                if (UnityEngine.Random.Range(0, 100) < 25)
                {
                    isSick = true;
                    FirstSickDay = true;
                    caughtDepression = true;
                    return;
                }
            }
            else if (Managers.Data.PlayerData.NowHeart < 50)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    isSick = true;
                    FirstSickDay = true;
                    caughtCold = true;
                    return;
                }
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    isSick = true;
                    FirstSickDay = true;
                    caughtDepression = true;
                    return;
                }
            }
            if (caughtCold && caughtDepression)
            {
                int randomValue = UnityEngine.Random.Range(0, 2);
                if (randomValue == 0)
                {
                    caughtCold = false;
                }
                else
                {
                    caughtDepression = false;
                }
            }
        }

        if (caughtCold) Managers.Data.PersistentUser.ColdCount++;
    }

    void ExecuteSickDay(OneDayScheduleData oneday)
    {
        if(caughtCold)
        {
            oneday.ScheduleType = ScheduleType.Caught;
            oneday.CheckAndAddIfNotWatched();
            UI_MainBackUI.instance.StartScreenAnimation("Cold");
        }
        else if(caughtDepression)
        {
            oneday.ScheduleType = ScheduleType.RunAway;
            oneday.CheckAndAddIfNotWatched();
            UI_MainBackUI.instance.StartScreenAnimation("RunAway");
        }
        if (FirstSickDay)
        {
            FirstSickDay = false;
        }
        else
        {
            isSick = false;
            caughtCold = false;
            caughtDepression = false;
        }
        int RestHeartStarValue = 10;

        Managers.Data.PlayerData.NowHeart += RestHeartStarValue;
        Managers.Data.PlayerData.NowStar += RestHeartStarValue;
    }

    bool BigSuccess = false;

    bool CheckSuccessProbability()
    {
        int BigSuccessProbability = (((int)Managers.Data.PlayerData.SixStat[5]) / 20)* Managers.instance.BigSuccessProbability;
        if (UnityEngine.Random.Range(0, 100) < (BigSuccessProbability))
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

        //3천은 그냥 상수
        int OneDayIncome = Mathf.CeilToInt(Mathf.Log10(beforeSub)*3000 * oneDay.InComeMag * bonusMultiplier);

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
        Managers.Data.PlayerData.nowSubCount += Mathf.CeilToInt(DaySub * (tempBonus.SubBonus) / 100f);
    }

    public float GetSubStatProperty(StatName statName)
    {
        int temp = 0;
        if (statName == StatName.Strength)
            temp = (int)Math.Floor(Managers.Data.PlayerData.SixStat[3]);

        else if (statName == StatName.Mental)
            temp = (int)Math.Floor(Managers.Data.PlayerData.SixStat[4]);

        float result = (float)(temp / 20);
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

    private Action weekOverAction;

    public Action WeekOverAction
    {
        get { return weekOverAction; }
        set { weekOverAction = value; }
    }

    public void FinishWeek()
    {
        Managers.Data.PlayerData.NowWeek++;
        Managers.Data.PlayerData.WeeklyCommunicationRewarded = false;
        UI_MainBackUI.instance.SetScreenAniSpeed(1);
        UI_MainBackUI.instance.StartScreenAnimation("WaitingArea");
        WeekOverAction?.Invoke();
        Managers.Data.SaveData();
    }
    #endregion
}
