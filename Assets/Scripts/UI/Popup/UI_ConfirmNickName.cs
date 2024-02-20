using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfirmNickName : UI_Popup
{
    public static UI_ConfirmNickName instance;
    const string text1 = "칭호를 선택하지 않고\n진행하시겠습니까?";
    const string text2 = "칭호는 최대 2개까지\n선택 가능합니다.\n이대로 진행하시겠습니까?";
    const string text3 = "로 이번\n회차를 시작하시겠습니까?";

    enum Buttons
    {
        YesBTN,
        NoBTN
    }
    enum Texts
    {
        EventText
    }

    enum Transforms
    {
        prefixParentTR,
        suffixParentTR,
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<RectTransform>(typeof(Transforms));

        int NicknameCount = 2 + UI_SelectNickName.instance.SelectedPrefix.NicknameIndex + UI_SelectNickName.instance.SelectedSuffix.NicknameIndex;

        if (NicknameCount == 0) GetText((int)Texts.EventText).text = text1;
        else if(NicknameCount == 1) GetText((int)Texts.EventText).text = text2;
        else GetText((int)Texts.EventText).text = "\'"+UI_SelectNickName.instance.SelectedPrefix.NicknameString + " " + UI_SelectNickName.instance.SelectedSuffix.NicknameString + "\'" + text3;

        GetButton((int)Buttons.YesBTN).onClick.AddListener(()=> UI_SelectNickName.instance.StartGame());
        GetButton((int)Buttons.YesBTN).onClick.AddListener(() => Managers.Sound.Play(Define.Sound.SmallBTN));
        GetButton((int)Buttons.NoBTN).onClick.AddListener(CloseBTN);
    }

   
}
