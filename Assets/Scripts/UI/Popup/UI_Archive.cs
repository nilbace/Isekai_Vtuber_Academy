using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Archive : UI_Popup
{
    enum Buttons
    {
        EltubeBTN,
        InstaBTN,
        CloseBTN
    }
    

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.EltubeBTN).onClick.AddListener(EltubeBTN);
        GetButton((int)Buttons.InstaBTN).onClick.AddListener(InstaBTN);
        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
    }

    void EltubeBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_Eltube>();
    }

    void InstaBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_Insta>();
    }

}
