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

            //BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((RestType)TaskEnum));
        }
        else if(TaskEnum is GoOutType)
        {
            BaseImage.sprite = BroadCastImgs[(int)TaskEnum];

            if (Managers.Data.PersistentUser.WatchedBroadCast.Contains((BroadCastType)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((BroadCastType)TaskEnum));
        }
    }

    void ShowBcPopup(BroadCastType broadCastType)
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_Ar_BC_Popup.broadCast = broadCastType;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }

    void ShowRestPopup(RestType restType)
    {
        Managers.Sound.Play(Sound.SmallBTN);
        //UI_Ar_BC_Popup.restt = restType;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }

    void UnCollectedBC()
    {

    }
}
