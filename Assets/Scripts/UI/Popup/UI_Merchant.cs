using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Merchant : UI_Popup
{
    public static UI_Merchant instance;
    public GameObject ItemBTN;
    List<Item> itemList = new List<Item>();

    enum Buttons
    {
        CloseBTN,
    }
    enum Texts
    {
        EventText,
        GameStatTMP,
        SongStatTMP,
        DrawStatTMP,
        StrStatTMP,
        MenStatTMP,
        LuckStatTMP,
        MoneyAmountTMP
    }

    enum Images
    {
        CutScene,
    }

    enum GameObjects
    {
        Content,
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
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
        Bind<GameObject>(typeof(GameObjects));

        Transform parent = GetGameObject((int)GameObjects.Content).transform;

        foreach (Item temp in Managers.Data.ItemList)
        {
            if (temp.EntWeek == Managers.Data.PlayerData.NowWeek)
            {
                itemList.Add(temp);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            GetText(i + 1).text = Managers.Data.PlayerData.SixStat[i].RoundToString();
        }
        GetText((int)Texts.MoneyAmountTMP).text = Util.FormatMoney(Managers.Data.PlayerData.nowGoldAmount);

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject Go = Instantiate(ItemBTN, parent, false);
            Go.GetComponent<MerChantItem>().Setting(itemList[i]);
        }

        GetButton((int)Buttons.CloseBTN).onClick.AddListener(Close);
    }

    void Close()
    {
        Managers.UI_Manager.ShowPopupUI<UI_CloseMerChant>();
        Managers.Sound.Play(Sound.SmallBTN);
    }

    public void UpdateTexts()
    {
        Transform parent = GetGameObject((int)GameObjects.Content).transform;

        for(int i = 0; i<parent.childCount; i++)
        {
            parent.GetChild(i).GetComponent<MerChantItem>().Setting(itemList[i]);
        }

        for (int i = 0; i < 6; i++) 
        {
            GetText(i+1).text = Managers.Data.PlayerData.SixStat[i].RoundToString();
        }
        GetText((int)Texts.MoneyAmountTMP).text = Util.FormatMoney(Managers.Data.PlayerData.nowGoldAmount);
    }
}
