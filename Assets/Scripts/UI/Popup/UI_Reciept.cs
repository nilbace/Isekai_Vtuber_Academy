using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;
using System;

public class UI_Reciept : UI_Popup
{
    public Ease EaseStatus;
    public float periodScale;
    public float Duration;
    enum Buttons
    {
        FinishBTN
    }
    enum Texts
    {
        NWeekTMP,
        SuccessTimeTMP,
        ReceieptDetailUpperTMP,
        ReceieptDetailLowerTMP
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Managers.Sound.Play(Define.Sound.Receipt);
        SetReceipt();
        transform.DOLocalMoveY(0, Duration).SetEase(EaseStatus, 0, periodScale);
        GetButton((int)Buttons.FinishBTN).onClick.AddListener(FinishBTN);
    }

    public void SetReceipt()
    {
        GetText((int)Texts.NWeekTMP).text = $"{Managers.Data._myPlayerData.NowWeek}주차 영수증";
        GetText((int)Texts.SuccessTimeTMP).text = $"{ScheduleExecuter.Inst.SuccessTime[0]}회"
                                                   + $"\n{ScheduleExecuter.Inst.SuccessTime[1]}회"
                                                   + $"\n{ScheduleExecuter.Inst.SuccessTime[2]}회";
        WeekReceiptData temp = ScheduleExecuter.Inst.BeforeScheduleData;

        int sub = Managers.Data._myPlayerData.nowSubCount - temp.Subs;
        int income = Managers.Data._myPlayerData.nowGoldAmount - temp.Gold;
        float[] stats = new float[6];
        for (int i = 0; i < 6; i++)
        {
            stats[i] = Managers.Data._myPlayerData.SixStat[i] - temp.SixStat[i];
        }

        GetText((int)Texts.ReceieptDetailUpperTMP).text = $"+{sub}명"
                                                           + $"\n{income}원";
        GetText((int)Texts.ReceieptDetailLowerTMP).text = "";
        for (int i = 0; i < 6; i++)
        {
            if(stats[i]==0)
            {
                GetText((int)Texts.ReceieptDetailLowerTMP).text += "-" + "\n";
            }
            else
            {
                GetText((int)Texts.ReceieptDetailLowerTMP).text += "+" + stats[i].ToString("F0") + "\n";
            }
            

            if (i == 5) GetText((int)Texts.ReceieptDetailLowerTMP).text.TrimEnd('\n');
        }

    }



    void FinishBTN()
    {
        Managers.instance.FinishWeek();
        Managers.Data._myPlayerData.NowWeek++;
        UI_MainBackUI.instance.EndScheduleAndSetUI();
        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Sound.Play(Define.Sound.NextWeekBTN);
    }
}
