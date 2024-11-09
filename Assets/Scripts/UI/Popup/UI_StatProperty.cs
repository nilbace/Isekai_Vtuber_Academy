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

        //���� �ڽ� �� �۱�
        //���ؾ� ���� ��ġ�� �ּ� �̰� ���� ��°�� ������
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatName.Sub)} +{nowBonus.SubBonus}%, {GetIconString(StatName.Gold)} +{nowBonus.IncomeBonus}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat) + " �Ƿ�";
        }
        else if(stat == StatName.Strength)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatName.Heart)} ���ҷ� -{(SelectedStatTier * Managers.instance.Str_Men_ValuePerLevel * 100).RoundToString()}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }
        else if(stat == StatName.Mental)
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatName.Star)} ���ҷ� -{(SelectedStatTier *Managers.instance.Str_Men_ValuePerLevel * 100 ).RoundToString()}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }
        else
        {
            GetText((int)Texts.StatInfoTMP).text = $"  {GetIconString(StatName.BigSuccess)} �뼺�� Ȯ�� {SelectedStatTier * Managers.instance.BigSuccessProbability}%";
            GetText((int)Texts.StatNameTMP).text = GetStatKorName(stat);
        }

        GetText((int)Texts.ExtraInfoTMP).text = SetExtraInfoTMP(stat);

        int tempTier = 1;
        int indexofEmptyText = IndexofEmptyPlace(SelectedStatTier);
        TMPro.TMP_Text[] StatInfoTexts = GetGameObject((int)GameObjects.TextGroup).GetComponentsInChildren<TMPro.TMP_Text>();
        TMPro.TMP_Text[] StatInfoTexts2 = GetGameObject((int)GameObjects.TextGroup2).GetComponentsInChildren<TMPro.TMP_Text>();

        //�ϴ� �۱͵� ���� �� ��ġ ����
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

    //���� �ڽ� �Ʒ� ����κ�
    //���ؾ� ���� ��ġ�� �ּ� �̰� ���� ��°�� ������
    string SetExtraInfoTMP(StatName stat)
    {
        string temp = "";
        switch (stat)
        {
            case StatName.Game:
                temp = "<sprite=0>���� ���� ��� ���� ��\n�Ʒ��� ǥ��� ���ʽ� ��ġ�� ȹ���մϴ�.";
                break;
            case StatName.Song:
                temp = "<sprite=1>�뷡 ���� ��� ���� ��\n�Ʒ��� ǥ��� ���ʽ� ��ġ�� ȹ���մϴ�.";
                break;
            case StatName.Draw:
                temp = "<sprite=2>�׸� ���� ��� ���� ��\n�Ʒ��� ǥ��� ���ʽ� ��ġ�� ȹ���մϴ�.";
                break;
            case StatName.Strength:
                temp = "���� ���� �� �Ʒ��� ǥ��� ��ġ��ŭ\n<sprite=11>�� �Ҹ��� �����մϴ�.";
                break;
            case StatName.Mental:
                temp = "���� ���� �� �Ʒ��� ǥ��� ��ġ��ŭ\n<sprite=12>�� �Ҹ��� �����մϴ�.";
                break;
            case StatName.Luck:
                temp = $"���� ���� �� �Ʒ��� Ȯ���� <sprite=8>�뼺���մϴ�.\n<sprite=8>�뼺�� �� ��� ��� ��ġ�� {LuckBounsValue()}% �߰��˴ϴ�.";
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

    //�ڿ� ������� �򸮴� ���ùڽ� �ܺ� ���ڵ�
    //20~200���� ���ݵ� �۱�
    //���ؾ� ���� ��ġ�� �ּ� �̰� ���� ��°�� ������
    string GetStatText2(int tier, StatName stat)
    {
        int nowgrade = tier * 20;
        Bonus temp2 = Managers.Data.GetMainProperty(tier * 20);

        string temp = "";
        if (stat == StatName.Game || stat == StatName.Song || stat == StatName.Draw)
            temp = $"  {GetIconString(StatName.Sub)} +{temp2.SubBonus}%, {GetIconString(StatName.Gold)} +{temp2.IncomeBonus}%";
        else if (stat == StatName.Strength)
            temp = $"  {GetIconString(StatName.Heart)} ���ҷ� -{tier * Mathf.RoundToInt(Managers.instance.Str_Men_ValuePerLevel*100)}%";
        else if (stat == StatName.Mental)
            temp = $"  {GetIconString(StatName.Star)} ���ҷ� -{tier * Mathf.RoundToInt(Managers.instance.Str_Men_ValuePerLevel * 100)}%";
        else
            temp = $"  {GetIconString(StatName.BigSuccess)} �뼺�� Ȯ�� {tier * Managers.instance.BigSuccessProbability}%";

        return temp;
    }

    int IndexofEmptyPlace(int RealTier)
    {
        int temp = RealTier - 1;
        if (temp == -1) return 0;
        return temp;
    }
}
