using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Photo : UI_Base
{
    public Button CoverBTN;
    public Image BaseImage;
    public Sprite[] BroadCastImgs;
    public Sprite[] RestImgs;
    public Sprite[] GoOutImgs;
    public Sprite[] EndingImgs;
    public Sprite ColdImg;
    public Sprite RunAwayImg;
    public Sprite[] RandEventCutScene;


    void Start()
    {
        Init();
    }

    public override void Init() {
        
    }

    public void Set(object TaskEnum)
    {
        if (TaskEnum is EndingName)
        {
            BaseImage.sprite = EndingImgs[(int)TaskEnum];

            if (Managers.Data.PersistentUser.WatchedEndingName.Contains((EndingName)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((EndingName)TaskEnum));
        }
        else if(TaskEnum is RandEventName)
        {
            int index = (int)TaskEnum;
            BaseImage.sprite = GetProperRandCutsceneIMG(Managers.RandEvent.EventDatasList[index].CutSceneName);

            if (Managers.Data.PersistentUser.WatchedRandEvent.Contains((RandEventName)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowRandEventPopup(index));
        }
        else if(TaskEnum is BroadCastType)
        {
            BaseImage.sprite = BroadCastImgs[(int)TaskEnum];

            if (Managers.Data.PersistentUser.WatchedBroadCast.Contains((BroadCastType)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(()=> ShowBcPopup((BroadCastType)TaskEnum));
        }
        else if(TaskEnum is RestType)
        {
            BaseImage.sprite = RestImgs[(int)TaskEnum];
            if (Managers.Data.PersistentUser.WatchedRest.Contains((RestType)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((RestType)TaskEnum));
        }
        else if(TaskEnum is GoOutType)
        {
            BaseImage.sprite = GoOutImgs[(int)TaskEnum];
            for (int i = (int)TaskEnum * 3; i <= (int)TaskEnum * 3 + 2; i++)
            {
                if (Managers.Data.PersistentUser.WatchedGoOut.Contains((GoOutType)i))
                {
                    CoverBTN.gameObject.SetActive(false);
                    break;
                }
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((GoOutType)((int)TaskEnum*3)));
        }
    }

    public void SetCold()
    {
        BaseImage.sprite = ColdImg;
        if (Managers.Data.PersistentUser.WatchedCaught)
        {
            CoverBTN.gameObject.SetActive(false);
        }

        BaseImage.GetComponent<Button>().onClick.AddListener(ShowColdPopup);
    }

    public void SetRunAway()
    {
        BaseImage.sprite = RunAwayImg;
        if (Managers.Data.PersistentUser.WatchedRunAway)
        {
            CoverBTN.gameObject.SetActive(false);
        }

        BaseImage.GetComponent<Button>().onClick.AddListener(ShowRunawayPopup);
    }


    void ShowBcPopup(object broadCastType)
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_Ar_BC_Popup.tasktype = broadCastType;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }

    void ShowRandEventPopup(int index)
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_RandomEvent._eventData = Managers.RandEvent.GetWeekEventByName((RandEventName)index);
        Debug.Log(Managers.RandEvent.GetWeekEventByName((RandEventName)index).eventName);
        UI_RandomEvent.ArchiveMode = true;
        Managers.UI_Manager.ShowPopupUI<UI_RandomEvent>();
    }

    void ShowEndingPopup(object endingType)
    {
        Managers.Sound.Play(Sound.SmallBTN);
    }
    void ShowColdPopup()
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_Ar_BC_Popup.isCold = true;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }

    void ShowRunawayPopup()
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_Ar_BC_Popup.isRunAway = true;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }

    Sprite GetProperRandCutsceneIMG(string name)
    {
        Sprite temp= null;
        for (int i = 0; i < RandEventCutScene.Length; i++)
        {
            if (RandEventCutScene[i].name == name) temp = RandEventCutScene[i];
        }
        return temp;
    }
}
