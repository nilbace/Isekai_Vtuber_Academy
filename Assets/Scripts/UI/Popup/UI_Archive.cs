using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Archive : UI_Popup
{
    enum Buttons
    {
        TVBTN,
        EventBTN,
        EndingBTN,
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

        GetButton((int)Buttons.TVBTN).onClick.AddListener(BCBTN);
        GetButton((int)Buttons.EventBTN).onClick.AddListener(InstaBTN);
        GetButton((int)Buttons.EndingBTN).onClick.AddListener(InstaBTN);
        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
    }

    void BCBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC>();
    }

    void EltubeBTN()
    {
        //Managers.Sound.Play(Define.Sound.SmallBTN);
        //Managers.UI_Manager.ShowPopupUI<UI_Eltube>();
    }

    void InstaBTN()
    {
        //Managers.Sound.Play(Define.Sound.SmallBTN);
        //Managers.UI_Manager.ShowPopupUI<UI_Insta>();
    }
}
