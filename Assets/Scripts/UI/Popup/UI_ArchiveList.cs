using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public enum ArchiveState
{ 
    BroadCast, EventCutscene, Ending
}

public class UI_ArchiveList : UI_Popup
{
    public Transform Parent;
    UI_Photo[] PhotoGroup;
    public static ArchiveState archiveState;
    public TMPro.TMP_Text PanelStateTMP;
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
        SetListByState();
    }

    void SetListByState()
    {
        PhotoGroup = GetComponentsInChildren<UI_Photo>();
        switch (archiveState)
        {
            case ArchiveState.BroadCast:
                PanelStateTMP.text = "πÊº€»≠∏È";
                for (int i = 0; i < 9; i++)
                {
                    PhotoGroup[i].Set((BroadCastType)i);
                }
                for (int i = 9; i < 15; i++)
                {
                    PhotoGroup[i].Set((RestType)(i - 9));
                }
                for (int i = 15; i < 21; i++)
                {
                    PhotoGroup[i].Set((GoOutType)(i - 15));
                }
                PhotoGroup[21].SetCold();
                PhotoGroup[22].SetRunAway();
                break;
            case ArchiveState.EventCutscene:
                PanelStateTMP.text = "¿Ã∫•∆Æƒ∆æ¿";
                for (int i = 0; i < 23; i++)
                {
                    if(i< (int)RandEventName.MaxCount)
                    {
                        PhotoGroup[i].Set((RandEventName)i);
                    }
                    else
                    {
                        PhotoGroup[i].gameObject.SetActive(false);
                    }
                }

                break;
            case ArchiveState.Ending:
                PanelStateTMP.text = "ø£µ˘ƒ∆æ¿";
                break;
        }

    }

    public static void CloseTwoPopup()
    {
        Managers.UI_Manager.ClosePopupUI();
        Managers.UI_Manager.ClosePopupUI();
    }
}
