using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_StatProperty : UI_Popup
{
    public float Tier0Poz;
    public float Tier1to10Poz;
    public float StatInfoInitalPoz;
    public float StatInfoInterval;
    public float TextGroupX;
    public float TextGroup2X;

    

    public static UI_StatProperty instance;
    enum Buttons
    {
        
    }
    enum Texts
    {
        BigStatValueTMP, StatInfoTMP, StatValueTMP, StatNameTMP, ExtraInfoTMP
    }

    enum GameObjects
    { 
        TextGroup, TextGroup2,
        StatInfo_SelectBox
    }

    enum Images
    {
        UpperStatIcon, Stat_Cover
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
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
    }

    public void Setting(StatName stat)
    {
        GetGameObject((int)GameObjects.StatInfo_SelectBox).transform.localPosition = new Vector3(0, StatInfoInitalPoz, 0);

        float nowStatValue = Managers.Data.PlayerData.SixStat[(int)stat];
        int SelectedStatTier = GetStatTier_div_20(nowStatValue);
        Bonus nowBonus = Managers.Data.GetMainProperty(stat);

        GetImage((int)Images.UpperStatIcon).sprite = Resources.Load<Sprite>($"Icon/{stat}");
        GetText((int)Texts.StatValueTMP).text = Managers.Data.PlayerData.SixStat[(int)stat].ToString("F0");
        GetImage((int)Images.Stat_Cover).transform.localScale = new Vector3(1 - (float)Managers.Data.PlayerData.SixStat[(int)stat] / 200f, 1, 1);
        GetGameObject((int)GameObjects.StatInfo_SelectBox).transform.localPosition += new Vector3(0, StatInfoInterval * SelectedStatTier, 0);
        GetText((int)Texts.BigStatValueTMP).text = (SelectedStatTier * 20).ToString();

        //선택 박스 내 글귀
        //영준아 여기 고치고 주석 이거 한줄 통째로 지워줘
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatIcons.Sub)} +{nowBonus.SubBonus}%, {GetIconString(StatIcons.Gold)} +{nowBonus.IncomeBonus}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat) + " 실력";
        }
        else if(stat == StatName.Strength)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatIcons.Heart)} 감소량 -{(SelectedStatTier * Managers.instance.Str_Men_ValuePerLevel * 100).RoundToString()}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }
        else if(stat == StatName.Mental)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatIcons.Star)} 감소량 -{(SelectedStatTier *Managers.instance.Str_Men_ValuePerLevel * 100 ).RoundToString()}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }
        else
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatIcons.BigSuccess)} 대성공 확률 {SelectedStatTier * Managers.instance.BigSuccessProbability}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }

        GetText((int)Texts.ExtraInfoTMP).text = SetExtraInfoTMP(stat);

        int tempTier = 1;
        int indexofEmptyText = IndexofEmptyPlace(SelectedStatTier);
        TMPro.TMP_Text[] StatInfoTexts = GetGameObject((int)GameObjects.TextGroup).GetComponentsInChildren<TMPro.TMP_Text>();
        TMPro.TMP_Text[] StatInfoTexts2 = GetGameObject((int)GameObjects.TextGroup2).GetComponentsInChildren<TMPro.TMP_Text>();

        //하단 글귀들 글자 및 위치 조절
        for (int i = 0; i < 11; i++)
        {
            if (i == indexofEmptyText)
            {
                StatInfoTexts[i].text = "";
                StatInfoTexts2[i].text = "";
                StatInfoTexts[i].rectTransform.sizeDelta = new Vector2(200, 10);
                StatInfoTexts2[i].rectTransform.sizeDelta = new Vector2(200, 10);
                continue;
            }
            StatInfoTexts[i].text = GetStatText(tempTier, stat);
            StatInfoTexts2[i].text = GetStatText2(tempTier, stat);
            tempTier++;
        }

        if (SelectedStatTier != 0)
        {
            GetGameObject((int)GameObjects.StatInfo_SelectBox).SetActive(true);
            GetGameObject((int)GameObjects.TextGroup).transform.localPosition = new Vector3(TextGroupX, Tier1to10Poz, 0);
            GetGameObject((int)GameObjects.TextGroup2).transform.localPosition = new Vector3(TextGroup2X, Tier1to10Poz, 0);
        }
        else
        {
            GetGameObject((int)GameObjects.StatInfo_SelectBox).SetActive(false);
            GetGameObject((int)GameObjects.TextGroup).transform.localPosition = new Vector3(TextGroupX, Tier0Poz, 0);
            GetGameObject((int)GameObjects.TextGroup2).transform.localPosition = new Vector3(TextGroup2X, Tier0Poz, 0);
        }

        
    }

    //위쪽 박스 아래 설명부분
    //영준아 여기 고치고 주석 이거 한줄 통째로 지워줘
    string SetExtraInfoTMP(StatName stat)
    {
        string temp = "";
        switch (stat)
        {
            case StatName.Game:
                temp = "<sprite=0>게임 관련 방송 진행 시\n아래에 표기된 보너스 수치를 획득합니다.";
                break;
            case StatName.Song:
                temp = "<sprite=1>노래 관련 방송 진행 시\n아래에 표기된 보너스 수치를 획득합니다.";
                break;
            case StatName.Draw:
                temp = "<sprite=2>그림 관련 방송 진행 시\n아래에 표기된 보너스 수치를 획득합니다.";
                break;
            case StatName.Strength:
                temp = "일정 진행 시 아래에 표기된 수치만큼\n<sprite=11>의 소모량이 감소합니다.";
                break;
            case StatName.Mental:
                temp = "일정 진행 시 아래에 표기된 수치만큼\n<sprite=12>의 소모량이 감소합니다.";
                break;
            case StatName.Luck:
                temp = $"일정 진행 시 아래의 확률로 <sprite=8>대성공합니다.\n<sprite=8>대성공 시 얻는 모든 수치가 {LuckBounsValue()}% 추가됩니다.";
                break;
        }

        return temp;
    }

    string LuckBounsValue()
    {
        return Math.Round((Managers.instance.BigSuccessCoefficientValue - 1) * 100).ToString();
    }



    int GetStatTier_div_20(float Value)
    {
        return (int)(Math.Floor(Value / 20));
    }

    string GetStatText(int tier, StatName stat)
    {
        int nowgrade = tier * 20;
        Bonus temp2 = Managers.Data.GetMainProperty(tier * 20);

        string temp = "";
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
            temp = $"{nowgrade} :";
        else if (stat == StatName.Strength )
            temp = $"{nowgrade} :";
        else if (stat == StatName.Mental)
            temp = $"{nowgrade} :";
        else
            temp = $"{nowgrade} :";

        return temp;
    }

    //뒤에 배경으로 깔리는 선택박스 외부 글자들
    //20~200까지 스텟들 글귀
    //영준아 여기 고치고 주석 이거 한줄 통째로 지워줘
    string GetStatText2(int tier, StatName stat)
    {
        int nowgrade = tier * 20;
        Bonus temp2 = Managers.Data.GetMainProperty(tier * 20);

        string temp = "";
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
            temp = $"  {GetIconString(StatIcons.Sub)} +{temp2.SubBonus}%, {GetIconString(StatIcons.Gold)} +{temp2.IncomeBonus}%";
        else if (stat == StatName.Strength)
            temp = $"  {GetIconString(StatIcons.Heart)} 감소량 -{tier * Mathf.RoundToInt(Managers.instance.Str_Men_ValuePerLevel*100)}%";
        else if (stat == StatName.Mental)
            temp = $"  {GetIconString(StatIcons.Star)} 감소량 -{tier * Mathf.RoundToInt(Managers.instance.Str_Men_ValuePerLevel * 100)}%";
        else
            temp = $"  {GetIconString(StatIcons.BigSuccess)} 대성공 확률 {tier * Managers.instance.BigSuccessProbability}%";

        return temp;
    }

    int IndexofEmptyPlace(int RealTier)
    {
        int temp = RealTier - 1;
        if (temp == -1) return 0;
        return temp;
    }
}
