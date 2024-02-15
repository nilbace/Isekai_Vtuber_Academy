using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BeforeSelectNickName : UI_Popup
{
    enum Buttons
    {
        ResultBTN
    }
    void Start()
    {
        Init();   
    }

    void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ResultBTN).onClick.AddListener(ShowSelectNickName);
    }

    void ShowSelectNickName()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.instance.ShowSelectNickName();
    }

}
