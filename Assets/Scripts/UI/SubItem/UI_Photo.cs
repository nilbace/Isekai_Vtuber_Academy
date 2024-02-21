using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Photo : UI_Base
{
    public Button CoverBTN;
    public Image BaseImage;
    public Image RedDot;
    public Sprite[] ScheduleTypeImgs;
    public Sprite[] EndingImgs;
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

            if (Managers.Data.PersistentUser.WatchedEndingName.ContainsKey((EndingName)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((EndingName)TaskEnum));
        }
        else if(TaskEnum is RandEventName)
        {
            int index = (int)TaskEnum;
            BaseImage.sprite = GetProperRandCutsceneIMG(Managers.RandEvent.EventDatasList[index].CutSceneName);

            if (Managers.Data.PersistentUser.WatchedRandEvent.ContainsKey((RandEventName)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowRandEventPopup(index));
        }
        else if(TaskEnum is ScheduleType)
        {
            BaseImage.sprite = ScheduleTypeImgs[(int)TaskEnum];

            //±× ½ºÄÉÁìÀ» ºÃ´Ù¸é
            if (Managers.Data.PersistentUser.WatchedScehdule.ContainsKey((ScheduleType)TaskEnum))
            {
                //Ä¿¹ö ¾ø¾Ö°í
                CoverBTN.gameObject.SetActive(false);
                //·¹µå´åÀº ºÃ´Ù¸é ²ö´Ù
                RedDot.gameObject.SetActive(!Managers.Data.PersistentUser.WatchedScehdule[(ScheduleType)TaskEnum]);
            }
            else
            {
                //·¹µå´åÀ» ²ö´Ù
                RedDot.gameObject.SetActive(false);
            }
            BaseImage.GetComponent<Button>().onClick.AddListener(()=> ShowBcPopup((BroadCastType)TaskEnum));

            if((int)TaskEnum == (int)ScheduleType.Caught)
            {
                BaseImage.GetComponent<Button>().onClick.RemoveAllListeners();
                BaseImage.GetComponent<Button>().onClick.AddListener(ShowColdPopup);
            }
            if ((int)TaskEnum == (int)ScheduleType.RunAway)
            {
                BaseImage.GetComponent<Button>().onClick.RemoveAllListeners();
                BaseImage.GetComponent<Button>().onClick.AddListener(ShowRunawayPopup);
            }
        }
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
