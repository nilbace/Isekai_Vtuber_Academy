using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

/// <summary>
/// ���� �Ŵ��� ���� ����
/// </summary>
public class ScheduleExecuter : MonoSingleton<ScheduleExecuter>
{
    public bool isDev;
    const float TimeToStamp = 2.3f;
    const float TimeStampToNext = 0.7f;
    public WeekReceiptData BeforeScheduleData = new WeekReceiptData();

    //������ ���޿� ����
    //0�뼺�� 1���� 2����
    [HideInInspector] public int[] SuccessTimeContainer = new int[3];

    public Action<int> SetAniSpeedAction;

    //���� ù�� üũ��
    //�ι��������� �̹� false��
    bool FirstSickDay = false;
    //�ι�¶���� ������ false�� �ٲ�
    bool isSick = false; 
    bool caughtCold = false; 
    bool caughtDepression = false;

    public PlusText[] FloatingTextPozs;

    void SetAniSpeed(int speed)
    {
        SetAniSpeedAction?.Invoke(speed);
    }

    public IEnumerator StartSchedule()
    {
        //���� �ʱ�ȭ
        isSick = false; FirstSickDay = false;
        BeforeScheduleData.FillDatas();
        UI_MainBackUI.instance.StopScreenAni();
        for (int i = 0; i < 3; i++)
        {
            SuccessTimeContainer[i] = 0;
        }
     

        //������ ����
        for (int i = 0; i < 7; i++)
        {
            bool isFastMode = UI_MainBackUI.instance.IsFastMode;
            yield return StartCoroutine(ExecuteOneDayWork(Managers.Data._SevenDayScheduleDatas[i], i, isFastMode));
            
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
            float waitTime = isFastMode ? TimeStampToNext / 2 : TimeStampToNext;
            yield return new WaitForSeconds(waitTime);
            ChattingManager.Inst.gameObject.SetActive(false);
        }

        //������ ������ �ʱ�ȭ
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
                Managers.instance.ShowMainStory();
                break;
        }


    }

    private void FinishTextAni()
    {
        foreach(var item in FloatingTextPozs)
        {
            item.StopAllCoroutines();
            item.text.alpha = 0;
        }
    }
    public IEnumerator ExecuteOneDayWork(OneDayScheduleData oneDay, int DayIndex, bool isFastMode)
    {
        //�ʱ�ȭ
        bool todaySick = false;
        BigSuccess = false;
        UI_Stamp.Inst.SetStamp(UI_Stamp.StampState.transparent);
        float bigSuccessMultiplier =1f;

        //FastMode��� 2���, �ƴϸ� 1���
        SetAniSpeed(isFastMode ? 2 : 1);

        //Ʃ�丮�����̶��
        if (UI_Tutorial.instance != null)
        {
            //����Ͽ� ����
            if (DayIndex == 5)
            {
                isSick = true;
                FirstSickDay = true;
                caughtCold = true;
            }
        }
        //Ʃ�丮���� �ƴ� ��
        else
        {
            //�޽� �ϴ°� �ƴ϶�� ���� �� ����
            if (oneDay.ContentType != ContentType.Rest && !isSick)
            {
                Check_illnessProbability();
            }
        }

        //���� ù��¶��, �ι�¶�� ����Ǵ� �κ�
        if (isSick)
        {
            todaySick = true;
            ExecuteSickDay(oneDay);
            //���� ī��Ʈ ����
            SuccessTimeContainer[2]++;

            float waitTime = isFastMode ? TimeToStamp / 2 : TimeToStamp;
            if (isDev) waitTime = 0;
            yield return new WaitForSeconds(waitTime);
        }
        //������ �ʴٸ� ��� ������ ���� �뼺���� �� �� ����
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

            //�뼺�� üũ
            bigSuccessMultiplier = 1.0f;
            
            //Ʃ�丮���̶��
            if(UI_Tutorial.instance != null)
            {
                //�����Ͽ� �뼺��
                if(DayIndex==2)
                {
                    BigSuccess = true;
                    bigSuccessMultiplier = Managers.instance.BigSuccessCoefficientValue;
                    //�뼺�� ī��Ʈ ����
                    SuccessTimeContainer[0]++;
                }
            }
            //Ʃ�丮���� �ƴ϶��
            //�뼺�� Ȯ�� üũ �� ����
            else
            {
                if (CheckSuccessProbability())
                {
                    BigSuccess = true;
                    bigSuccessMultiplier = Managers.instance.BigSuccessCoefficientValue;
                    //�뼺�� ī��Ʈ ����
                    SuccessTimeContainer[0]++;
                    Managers.Data.PersistentUser.BigSuccessCount++;
                }
                else
                {
                    //���� ī��Ʈ ����
                    SuccessTimeContainer[1]++;
                }
            }

            

            List<(StatName stat, float value)> ChangedList = new List<(StatName stat, float value)>();

            

            //����� �����ߴٸ� �� ������ ����
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

            

            // ����� ��ȭ
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

            // ��Ʈ(Heart) �� ���� �� ����Ʈ�� �߰�
            float newHeartValue = Mathf.Clamp(Mathf.CeilToInt(HeartVariance) + Managers.Data.PlayerData.NowHeart, 0, 100);
            if (newHeartValue != Managers.Data.PlayerData.NowHeart)
            {
                ChangedList.Add((StatName.Heart, newHeartValue - Managers.Data.PlayerData.NowHeart));
            }
            Managers.Data.PlayerData.NowHeart = newHeartValue;

            // ��(Star) �� ���� �� ����Ʈ�� �߰�
            float newStarValue = Mathf.Clamp(Mathf.CeilToInt(StarVariance) + Managers.Data.PlayerData.NowStar, 0, 100);
            if (newStarValue != Managers.Data.PlayerData.NowStar)
            {
                ChangedList.Add((StatName.Star, newStarValue - Managers.Data.PlayerData.NowStar));
            }
            Managers.Data.PlayerData.NowStar = newStarValue;

            // ���� ��ȭ
            float[] tempstat = new float[6];
            for (int i = 0; i < 6; i++)
            {
                tempstat[i] = oneDay.Six_Stats[i] * bigSuccessMultiplier;
                float newStatValue = Mathf.Clamp(Mathf.CeilToInt(tempstat[i]) + Managers.Data.PlayerData.SixStat[i], 0, 200);

                // ���� ��ȭ�� ������ ����Ʈ�� �߰�
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

        


        //UI�ϴ� �� ���̱�
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
        //Ʃ�丮�� �����϶�
        if (UI_Tutorial.instance != null)
        {
            yield return new WaitForSeconds(0.2f);
            //�������̶��
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



    public List<PlusText> GetRandomFloatingTextPoz(int number)
    {
        List<PlusText> randomPositions = new List<PlusText>();
        List<int> indices = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < number; i++)
        {
            // �ʿ��� ���, �ε��� ����Ʈ�� �缳���Ͽ� �ߺ� ���� �ٽ� ���� �����ϵ��� ��
            if (indices.Count == 0)
            {
                indices = new List<int> { 0, 1, 2, 3 };
            }

            int randIndex = UnityEngine.Random.Range(0, indices.Count);
            int selected = indices[randIndex];

            randomPositions.Add(FloatingTextPozs[selected]);
            indices.RemoveAt(randIndex);
        }

        return randomPositions;
    }




    #region Sick__BigSuccess


    //���⿡ �ɸ��ų� ������
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

        //3õ�� �׳� ���
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

    //�����
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
