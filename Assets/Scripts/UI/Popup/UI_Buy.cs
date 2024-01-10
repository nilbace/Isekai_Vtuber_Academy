using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Buy : UI_Popup
{
    Item item;
    enum Buttons
    {
        YesBTN, NoBTN
    }
    enum Texts
    {
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
        item = MerChantItem.BuyUIItem;

        GetButton(0).onClick.AddListener(BuyItem);
        GetButton(1).onClick.AddListener(CloseBTN);
        GetText(0).text = item.ItemInfoText;

        void BuyItem()
        {
            Managers.Data._myPlayerData.nowGoldAmount -= item.Cost;

            for (int i = 0; i < 6; i++)
            {
                Managers.Data._myPlayerData.SixStat[i] += item.SixStats[i];
            }
            Managers.Data._myPlayerData.RubiaKarma += item.Karma;

            UI_MainBackUI.instance.UpdateUItexts();
            Managers.Sound.Play(Sound.Buy);
            Managers.UI_Manager.ClosePopupUI();
            Managers.Data._myPlayerData.BoughtItems.Add(item.ItemName);
            UI_Merchant.instance.UpdateTexts();
        }
    }

}
