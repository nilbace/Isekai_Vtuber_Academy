using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DefaultPopup : UI_Popup
{
    public static UI_DefaultPopup instance;
    public static string InfoText;
    public static string ResultBTNText;
    private void Awake()
    {
        instance = this;
    }
    enum Buttons
    {
        ResultBTN
    }
    enum Texts
    {
        EventText, resultbtnTMP

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

        GetButton(0).onClick.AddListener(ResultBTN);
        Setting();
    }

    void Setting()
    {
        GetText(0).text = InfoText;
        GetText(1).text = ResultBTNText;
    }

    void ResultBTN()
    {
        Managers.instance.ShowReceipt();
    }
}
