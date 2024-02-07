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
}
