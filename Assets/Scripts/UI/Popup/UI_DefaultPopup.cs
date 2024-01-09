using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DefaultPopup : UI_Popup
{
    public static UI_DefaultPopup instance;

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
    }

    public void SetText(string EventInfoText, string ResultInfoText)
    {
        GetText(0).text = EventInfoText;
        GetText(1).text = ResultInfoText;
    }

    void ResultBTN()
    {
        Managers.instance.ShowReceipt();
    }
}
