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
    public float StatInfoPoz;

    public static UI_StatProperty instance;
    enum Buttons
    {
        CloseBTN,
    }
    enum Texts
    {
        BigStatValueTMP, StatInfoTMP, StatValueTMP
    }

    enum GameObjects
    { 
        TextGroup,
        StatInfo_SelectBox
    }

    enum Images
    {
        StatIcon, Stat_Cover
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

        GetButton((int)Buttons.CloseBTN).onClick.AddListener(Close);
    }

    public void Setting(StatName stat)
    {
        GetGameObject((int)GameObjects.StatInfo_SelectBox).transform.localPosition = new Vector3(0, -52.6f, 0);
        GetText((int)Texts.StatInfoTMP).text = stat.ToString();

        float nowStatValue = Managers.Data._myPlayerData.SixStat[(int)stat];
        int SelectedStatTier = GetStatTier_div_20(nowStatValue);
        Bonus nowBonus = Managers.Data.GetMainProperty(stat);

        //아이콘 세팅
        GetImage((int)Images.StatIcon).sprite = Resources.Load<Sprite>($"Icon/{stat}");
        GetText((int)Texts.StatValueTMP).text = Managers.Data._myPlayerData.SixStat[(int)stat].ToString("F0");
        GetImage((int)Images.Stat_Cover).transform.localScale = new Vector3(1 - (float)Managers.Data._myPlayerData.SixStat[(int)stat] / 200f, 1, 1);


        //위치 조절
        GetGameObject((int)GameObjects.StatInfo_SelectBox).transform.localPosition += new Vector3(0, StatInfoPoz * SelectedStatTier, 0);
        //내부 글자 조절
        GetText((int)Texts.BigStatValueTMP).text = (SelectedStatTier * 20).ToString();
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
            GetText((int)Texts.StatInfoTMP).text = $"구독자 수 + {nowBonus.SubBonus}%, 돈 획득량 +{nowBonus.IncomeBonus}%";


        int tempTier = 1;
        int indexofEmptyText = IndexofEmptyPlace(SelectedStatTier);
        TMPro.TMP_Text[] StatInfoTexts = GetGameObject((int)GameObjects.TextGroup).GetComponentsInChildren<TMPro.TMP_Text>();
        for (int i = 0; i < 11; i++)
        {
            if (i == indexofEmptyText)
            {
                StatInfoTexts[i].text = "";
                StatInfoTexts[i].rectTransform.sizeDelta = new Vector2(200, 10);
                continue;
            }
            StatInfoTexts[i].text = GetMainStatText(tempTier);
            tempTier++;
        }

        if (SelectedStatTier != 0)
        {
            GetGameObject((int)GameObjects.StatInfo_SelectBox).SetActive(true);
            GetGameObject((int)GameObjects.TextGroup).transform.localPosition = new Vector3(-46, Tier1to10Poz, 0);
        }
        else
        {
            GetGameObject((int)GameObjects.StatInfo_SelectBox).SetActive(false);
            GetGameObject((int)GameObjects.TextGroup).transform.localPosition = new Vector3(-46, Tier0Poz, 0);
        }
    }

 

    int GetStatTier_div_20(float Value)
    {
        return (int)(Math.Floor(Value / 20));
    }

    string GetMainStatText(int tier)
    {
        int nowgrade = tier * 20;
        Bonus temp2 = Managers.Data.GetMainProperty(tier * 20);

        string temp = $"{nowgrade} : 구독자 수 + {temp2.SubBonus}%, 돈 획득량 +{temp2.IncomeBonus}%";

        return temp;
    }

    int IndexofEmptyPlace(int RealTier)
    {
        int temp = RealTier - 1;
        if (temp == -1) return 0;
        return temp;
    }


    void Close()
    {
        Managers.UI_Manager.ClosePopupUI();
    }

   
}
