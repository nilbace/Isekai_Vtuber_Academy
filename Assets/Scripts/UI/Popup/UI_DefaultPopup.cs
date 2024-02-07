using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;

public class UI_DefaultPopup : UI_Popup
{
    public static UI_DefaultPopup instance;
    
    static DefaultPopupState popupState;
    static string InfoText;
    static string ResultBTNText;
    public Image RubiaImage;
    public Ease ease;
    public float MoveTime;
    public Animator SurpriseAnimator;
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

        GetButton((int)Buttons.ResultBTN).onClick.AddListener(ResultBTN);
        Setting();
        if (popupState == DefaultPopupState.Merchant)
        {
            RubiaImage.transform.DOMoveX(-250, MoveTime).SetEase(ease);
        }
        else if(popupState == DefaultPopupState.RandomEvent)
        {
            SurpriseAnimator.gameObject.SetActive(true);
        }
    }

    void Setting()
    {
        GetText((int)Texts.EventText).text = InfoText;
        GetText((int)Texts.resultbtnTMP).text = ResultBTNText;
    }
    public static void MerchantAppear()
    {
        Managers.Sound.Play(Sound.knock);
        SetDefaultPopupUI(DefaultPopupState.Merchant, "실례합니다", "상인이 찾아왔다");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
    }

    public static void RandEventOccur()
    {
        SetDefaultPopupUI(DefaultPopupState.RandomEvent, "무슨 일이 일어난 것\n같다...", "사태 파악하기");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
    }

    public static void SetDefaultPopupUI(DefaultPopupState defaultPopupState, string infotext, string resulttext)
    {
        popupState = defaultPopupState;
        InfoText = infotext;
        ResultBTNText = resulttext;
    }

    void ResultBTN()
    {
        switch (popupState)
        {
            case DefaultPopupState.Normal:
                Managers.instance.ShowReceipt();
                break;
            case DefaultPopupState.Merchant:
                Managers.UI_Manager.ShowPopupUI<UI_Merchant>();
                break;
            case DefaultPopupState.RandomEvent:
                Managers.UI_Manager.ShowPopupUI<UI_RandomEvent>();
                break;
        }

    }
}
