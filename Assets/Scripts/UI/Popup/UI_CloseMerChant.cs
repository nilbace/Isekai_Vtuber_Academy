using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CloseMerChant : UI_Popup
{
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

        GetButton(0).onClick.AddListener(FarewellMerchant);
        GetButton(1).onClick.AddListener(CloseBTN);
        GetText(0).text = "상인을 돌려보내시겠습니까?";

        void FarewellMerchant()
        {
            Managers.UI_Manager.CloseALlPopupUI();
            Managers.instance.ShowReceipt();
        }
    }

}
