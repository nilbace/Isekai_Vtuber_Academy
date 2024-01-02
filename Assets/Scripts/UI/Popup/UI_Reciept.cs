using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Reciept : UI_Popup
{
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

        SetReceipt();

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
        Managers.Data._myPlayerData.NowWeek++;
        UI_MainBackUI.instance.UpdateUItexts();
        UI_MainBackUI.instance.EndScheduleAndSetUI();
        Managers.Data.SaveData();
        Managers.UI_Manager.CloseALlPopupUI();
    }
}
