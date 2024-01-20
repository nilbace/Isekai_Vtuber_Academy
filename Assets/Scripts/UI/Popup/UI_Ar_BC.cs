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

        int i = 0;
        foreach(UI_Photo photo in PhotoGroup)
        {
            photo.Set((BroadCastType)i);
            i++;
        }
    }
}