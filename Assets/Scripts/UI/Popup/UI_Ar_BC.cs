using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Ar_BC : UI_Popup
{
    public Transform Parent;
    UI_Photo[] PhotoGroup;
    enum Buttons
    {
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
        GetButton(0).onClick.AddListener(CloseBTN);
        PhotoGroup = GetComponentsInChildren<UI_Photo>();

        for (int i = 0; i < 9; i++)
        {
            PhotoGroup[i].Set((BroadCastType)i);
        }
        for (int i = 9; i < 15; i++)
        {
            PhotoGroup[i].Set((RestType)(i-9));
        }
        for (int i = 15; i < 21; i++)
        {
            PhotoGroup[i].Set((GoOutType)(i-15));
        }
        PhotoGroup[21].SetCold();
        PhotoGroup[22].SetRunAway();
    }
}
