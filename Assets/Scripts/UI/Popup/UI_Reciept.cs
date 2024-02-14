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
        var tween = transform.DOLocalMoveY(0, Duration).SetEase(EaseStatus, 0, periodScale);
        tween.OnComplete(FocusifTutorial);
        GetButton((int)Buttons.FinishBTN).onClick.AddListener(FinishBTN);
        if (UI_Tutorial.instance != null)
            UI_Tutorial.instance.NextDialogue();
    }

    void FocusifTutorial()
    {
        if (UI_Tutorial.instance != null)
        {
            UI_Tutorial.instance.NextDialogue();
        }
    }

    public void SetReceipt()
    {
        GetText((int)Texts.NWeekTMP).text = $"{Managers.Data.PlayerData.NowWeek}주차 영수증";
        GetText((int)Texts.SuccessTimeTMP).text = $"{ScheduleExecuter.Inst.SuccessTimeContainer[0]}회"
                                                   + $"\n{ScheduleExecuter.Inst.SuccessTimeContainer[1]}회"
                                                   + $"\n{ScheduleExecuter.Inst.SuccessTimeContainer[2]}회";
        WeekReceiptData temp = ScheduleExecuter.Inst.BeforeScheduleData;

        int sub = Managers.Data.PlayerData.nowSubCount - temp.Subs;
        int income = Managers.Data.PlayerData.nowGoldAmount - temp.Gold;
        float[] stats = new float[6];
        for (int i = 0; i < 6; i++)
        {
            stats[i] = Managers.Data.PlayerData.SixStat[i] - temp.SixStat[i];
        }

        GetText((int)Texts.ReceieptDetailUpperTMP).text = $"+{sub}명"
                                                           + $"\n{income}원";
        GetText((int)Texts.ReceieptDetailLowerTMP).text = "";
        for (int i = 0; i < 6; i++)
        {
            GetText((int)Texts.ReceieptDetailLowerTMP).text += (stats[i] >= 0 ? (stats[i] == 0 ? "-" : "+") : "") + (stats[i] != 0 ? stats[i].ToString("F0") : "") + "\n";
        }
        GetText((int)Texts.ReceieptDetailLowerTMP).text.TrimEnd('\n');
    }



    void FinishBTN()
    {
        //삭제할 부분.
        var data = Managers.Data.PlayerData;
        temp += $"{data.NowWeek}주차 : 게임{data.SixStat[0]},구독{data.nowSubCount},돈{data.nowGoldAmount},근력{data.SixStat[3]},멘탈{data.SixStat[4]},행운{data.SixStat[5]}\n";
        Debug.Log(temp);
        //여기까지

        //if (Managers.Data.PlayerData.NowWeek == 20)
        //{
        //    Managers.UI_Manager.ShowPopupUI<UI_Ending>();
        //    return;
        //}
        if (UI_Tutorial.instance == null)
            Managers.UI_Manager.CloseALlPopupUI();
        else
            Managers.UI_Manager.ClosePopupUI();

        Managers.Sound.Play(Define.Sound.NextWeekBTN);

        ScheduleExecuter.Inst.FinishWeek();
    }

    static string temp = "";
}
