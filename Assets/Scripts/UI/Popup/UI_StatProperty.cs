using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_StatProperty : UI_Popup
{
    public static UI_StatProperty instance;
    enum Buttons
    {
        CloseBTN,
    }
    enum Texts
    {
        StatTMP,
    }

 
    private void Start()
    {
        instance = this;
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));
        GetText((int)Texts.StatTMP).text = UI_MainBackUI.instance.NowSelectStatProperty.ToString();
        if(UI_MainBackUI.instance.NowSelectStatProperty == StatName.Game)
        {
            Bonus temp = Managers.Data.GetMainProperty(StatName.Game);
            GetText((int)Texts.StatTMP).text += "\n구독자 보너스 : "  + temp.SubBonus.ToString();
            GetText((int)Texts.StatTMP).text += "\n수익 보너스 : "    + temp.IncomeBonus.ToString();
        }
        else if (UI_MainBackUI.instance.NowSelectStatProperty == StatName.Song)
        {
            Bonus temp = Managers.Data.GetMainProperty(StatName.Song);
            GetText((int)Texts.StatTMP).text += "\n구독자 보너스 : " + temp.SubBonus.ToString();
            GetText((int)Texts.StatTMP).text += "\n수익 보너스 : " + temp.IncomeBonus.ToString();
        }
        else if (UI_MainBackUI.instance.NowSelectStatProperty == StatName.Draw)
        {
            Bonus temp = Managers.Data.GetMainProperty(StatName.Draw);
            GetText((int)Texts.StatTMP).text += "\n구독자 보너스 : " + temp.SubBonus.ToString();
            GetText((int)Texts.StatTMP).text += "\n수익 보너스 : " + temp.IncomeBonus.ToString();
        }


        GetButton((int)Buttons.CloseBTN).onClick.AddListener(Close);
    }

    public void SetInfo(StatName statName)
    {

    }

 

    void Close()
    {
        Managers.UI_Manager.ClosePopupUI();
    }

   
}
