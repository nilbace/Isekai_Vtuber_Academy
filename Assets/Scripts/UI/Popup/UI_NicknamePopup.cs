using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NicknamePopup : UI_Popup
{
    public static string InfoText;
    public static string ResultBTNText;
    public static NickName nickName;
    enum Buttons
    {
        ResultBTN
    }
    enum Texts
    {
        NicknameText,
        EffectText,
        EventText,

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

        GetButton(0).onClick.AddListener(CloseBTN);
        GetText(0).text = nickName.NicknameString;
        GetText(1).text = GenerateEffectString();
        GetText(2).text = nickName.ConditionString;
    }
  
    string GenerateEffectString()
    {
        var sixstat = nickName.GetSixStat();
        string temp = "";
        for (int i = 0; i < 3; i++)
        {
            if(sixstat[i] != 0)
            {
                temp += $"<sprite={i}>+{sixstat[i]} ";
            }
        }
        for (int i = 3; i < 6; i++)
        {
            if (sixstat[i] != 0)
            {
                temp += $"<sprite={i}>+{sixstat[i]} ";
            }
        }
        if (nickName.SubCount != 0)
        {
            temp += $"<sprite=6>+{nickName.SubCount} ";
        }
        if(nickName.MoneyValue != 0)
        {
            temp += $"<sprite=7>+{nickName.MoneyValue}";
        }

        return temp;
    }
}
