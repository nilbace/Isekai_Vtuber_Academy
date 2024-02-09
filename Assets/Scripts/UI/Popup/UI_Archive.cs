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

        GetButton((int)Buttons.TVBTN).onClick.AddListener(BroadcastBTN);
        GetButton((int)Buttons.EventBTN).onClick.AddListener(EventBTN);
        GetButton((int)Buttons.EndingBTN).onClick.AddListener(EndingBTN);
        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
    }

    void BroadcastBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
        UI_ArchiveList.archiveState = ArchiveState.BroadCast;
    }

    void EventBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
        UI_ArchiveList.archiveState = ArchiveState.EventCutscene;
    }

    void EndingBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
        UI_ArchiveList.archiveState = ArchiveState.Ending;
    }
}
