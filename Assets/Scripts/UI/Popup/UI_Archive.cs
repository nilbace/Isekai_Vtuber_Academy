using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Archive : UI_Popup
{
    public static UI_Archive instance;
    public Image[] Reddot;
    enum Buttons
    {
        TVBTN,
        EventBTN,
        EndingBTN,
        CloseBTN
    }
    private void Awake()
    {
        instance = this;
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

        UpdateRedDot();
    }

    public void UpdateRedDot()
    {
        //안본게 하나라도 남아있으면
        if(Managers.Data.PersistentUser.WatchedScehdule.ContainsValue(false))
        {
            Reddot[0].gameObject.SetActive(true);
        }
        else
        {
            Reddot[0].gameObject.SetActive(false);
        }

        if (Managers.Data.PersistentUser.WatchedRandEvent.ContainsValue(false))
        {
            Reddot[1].gameObject.SetActive(true);
        }
        else
        {
            Reddot[1].gameObject.SetActive(false);
        }

        if (Managers.Data.PersistentUser.WatchedEndingName.ContainsValue(false))
        {
            Reddot[2].gameObject.SetActive(true);
        }
        else
        {
            Reddot[2].gameObject.SetActive(false);
        }
    }

    void BroadcastBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        UI_ArchiveList.archiveState = ArchiveState.BroadCast;
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
    }

    void EventBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        UI_ArchiveList.archiveState = ArchiveState.EventCutscene;
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
    }

    void EndingBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        UI_ArchiveList.archiveState = ArchiveState.Ending;
        Managers.UI_Manager.ShowPopupUI<UI_ArchiveList>();
    }
}
